using AutoMapper;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using SystemPayment.API.DataModels;
using SystemPayment.API.DTO;
using SystemPayment.API.Repositories.Interface;
using SystemPayment.API.Response;

namespace SystemPayment.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public PaymentController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAllPaymentSettings()
		{
			var paymentSettings = await _unitOfWork.PaymentSettings.GetAllAsync(e => !e.IsDeleted && !e.EducationYear.IsDeleted && !e.EducationType.IsDeleted && !e.Branch.IsDeleted, p => p.PaymentType, p => p.Branch, p => p.EducationType, p => p.EducationYear);
			var paymentSettingsDto = _mapper.Map<IEnumerable<PaymentSettingDto>>(paymentSettings);
			return Ok(new ApiResponse<IEnumerable<PaymentSettingDto>>(paymentSettingsDto));
		}

		// GET: api/GetBranchsOrEducationTypeHasPaymentTypeGrouped
		[HttpGet("GetBranchsOrEducationTypeHasPaymentTypeGrouped")]
		public async Task<IActionResult> GetBranchsOrEducationTypeHasPaymentTypeGrouped([FromQuery] PaymentCheckDto paymentCheckDto)
		{
			var paymentSettings = await _unitOfWork.PaymentSettings.GetBranchsAndEducationPaymentAsync(paymentCheckDto);
			var grouped = paymentSettings.GroupBy(e => new { e.BranchId, e.EducationYearId, e.PaymentTypeId, e.EducationTypeId }).Select(group => new PaymentCheckResultDto
			{
				BranchName = group.First().Branch.Name,
				EducationYear = group.First().EducationYear.Year,
				PaymentTypeName = group.First().PaymentType.Name,
				EducationTypeName = group.First().EducationType.Name,
			}).ToList();
			return Ok(new ApiResponse<IEnumerable<PaymentCheckResultDto>>(grouped));
		}

		[HttpPost("GetPaymentByFilter")]
		public async Task<IActionResult> GetPaymentByFilter(PaymentSettingFilterDto filterDto)
		{
			if (filterDto == null)
				return BadRequest(new ApiResponse<string>("Invalid filter data.", StatusCodes.Status400BadRequest));

			var paymentSettings = await _unitOfWork.PaymentSettings.GetPaymentByFilterAsync(filterDto);
			var paymentSettingsDto = _mapper.Map<IEnumerable<PaymentSettingDto>>(paymentSettings);
			return Ok(new ApiResponse<IEnumerable<PaymentSettingDto>>(paymentSettingsDto));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPaymentSettingById(int id)
		{
			var paymentSetting = await _unitOfWork.PaymentSettings.GetByIdAsync(id);
			if (paymentSetting == null)
				return NotFound(new ApiResponse<string>("Payment not found.", StatusCodes.Status404NotFound));

			var paymentSettingDto = _mapper.Map<PaymentSettingDto>(paymentSetting);
			return Ok(new ApiResponse<PaymentSettingDto>(paymentSettingDto));
		}

		[HttpPost("CreatePaymentByList")]
		public async Task<IActionResult> CreatePaymentByList([FromBody] List<PaymentCreateSettingListDto> paymentSettingListDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (paymentSettingListDto == null)
				return BadRequest(new ApiResponse<string>("Invalid data.", StatusCodes.Status400BadRequest));

			var totalPercentage = paymentSettingListDto.Sum(p => p.PaymentPercentage);
			if (totalPercentage != 100)
				return BadRequest(new ApiResponse<string>("إجمالي النسب لابد ان يكون 100.", StatusCodes.Status400BadRequest));

			var paymentSettings = _unitOfWork.PaymentSettings.ConvertCreateListToPaymentSettings(paymentSettingListDto);

			foreach (var item in paymentSettings)
			{
				var paymentExists = await _unitOfWork.PaymentSettings.FindFirstOrDefaultAsync(p =>
					p.PaymentTypeId == item.PaymentTypeId
					&& p.EducationYearId == item.EducationYearId
					&& p.BranchId == item.BranchId
					&& p.EducationTypeId == item.EducationTypeId
					&& p.PaymentNumber == item.PaymentNumber
					&& !p.IsDeleted);
				if (paymentExists != null)
				{
					paymentExists.PaymentPercentage = item.PaymentPercentage;
					paymentExists.PaymentStartDate = item.PaymentStartDate;
					paymentExists.PaymentEndDate = item.PaymentEndDate;
					item.Id = paymentExists.Id;
				}
			}
			if (paymentSettings.Where(r => r.Id == 0).Count() > 0)
				await _unitOfWork.PaymentSettings.AddRangeAsync(paymentSettings.Where(r => r.Id == 0));
			await _unitOfWork.CompleteAsync();

			return Ok(new ApiResponse<bool>(StatusCodes.Status200OK));
		}

		[HttpPut("UpdateListOfPayments")]
		public async Task<IActionResult> UpdateListOfPayments([FromBody] List<PaymentSettingUpdateDto> paymentSettingUpdates)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var totalPercentage = paymentSettingUpdates.Sum(p => p.PaymentPercentage);
			if (totalPercentage != 100)
				return BadRequest(new ApiResponse<string>("إجمالي النسب لابد ان يكون 100.", StatusCodes.Status400BadRequest));

			var updatedPaymentSettingIds = paymentSettingUpdates.Select(u => u.Id).ToList();
			var updatedPaymentSettings = _mapper.Map<List<PaymentSetting>>(paymentSettingUpdates);
			var existingPaymentSettings = await _unitOfWork.PaymentSettings.GetAllAsync(p => updatedPaymentSettingIds.Contains(p.Id));
			foreach (var updatedSetting in updatedPaymentSettings)
			{
				var existingSetting = existingPaymentSettings.FirstOrDefault(e => e.Id == updatedSetting.Id);
				if (existingSetting != null)
				{
					existingSetting.PaymentPercentage = updatedSetting.PaymentPercentage;
					existingSetting.PaymentStartDate = updatedSetting.PaymentStartDate;
					existingSetting.PaymentEndDate = updatedSetting.PaymentEndDate;
				}
			}
			_unitOfWork.PaymentSettings.UpdateRange(existingPaymentSettings);
			await _unitOfWork.CompleteAsync();

			return Ok(new ApiResponse<bool>(StatusCodes.Status200OK));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> SoftDeletePaymentSetting(int id)
		{
			var paymentSetting = await _unitOfWork.PaymentSettings.GetByIdAsync(id);
			if (paymentSetting == null)
				return NotFound(new ApiResponse<string>("Payment not found.", StatusCodes.Status404NotFound));

			paymentSetting.IsDeleted = true;
			paymentSetting.DeletionDate = DateTime.UtcNow;
			_unitOfWork.PaymentSettings.Update(paymentSetting);
			await _unitOfWork.CompleteAsync();

			return Ok(new ApiResponse<bool>(StatusCodes.Status200OK));
		}

	}
}
