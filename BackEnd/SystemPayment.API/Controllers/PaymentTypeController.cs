using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SystemPayment.API.DTO;
using SystemPayment.API.Repositories.Interface;
using SystemPayment.API.Response;

namespace SystemPayment.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentTypeController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public PaymentTypeController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		// GET: api/PaymentType
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var paymentTypes = await _unitOfWork.PaymentTypes.GetAllAsync(e => !e.IsDeleted);
			var paymentTypeDtos = _mapper.Map<IEnumerable<PaymentTypeDto>>(paymentTypes);
			return Ok(new ApiResponse<IEnumerable<PaymentTypeDto>>(paymentTypeDtos));
		}

		// GET: api/CheckDefaultPaymentTypeExist
		[HttpGet("CheckDefaultPaymentTypeExist/{exceptId}")]
		public async Task<IActionResult> CheckDefaultPaymentTypeExist(int exceptId)
		{
			var isExist = await _unitOfWork.PaymentTypes.IsExist(e => !e.IsDeleted && e.IsDefault && e.Id != exceptId);
			return Ok(new ApiResponse<bool>(isExist));
		}
		// GET: api/PaymentType/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var paymentType = await _unitOfWork.PaymentTypes.GetByIdAsync(id);
			if (paymentType == null)
				return NotFound(new ApiResponse<PaymentType>("نوع الدفع غير موجود.", StatusCodes.Status404NotFound));

			var paymentTypeDto = _mapper.Map<PaymentTypeDto>(paymentType);
			return Ok(new ApiResponse<PaymentTypeDto>(paymentTypeDto));
		}

		// POST: api/PaymentType
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] PaymentTypeCreateDto paymentTypeCreateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (await _unitOfWork.PaymentTypes.IsExist(e => !e.IsDeleted && e.Name.Trim() == paymentTypeCreateDto.Name.Trim()))
				return BadRequest(new ApiResponse<PaymentType>("نوع الدفع موجود مسبقا.", StatusCodes.Status400BadRequest));

			var paymentType = _mapper.Map<PaymentType>(paymentTypeCreateDto);

			if (paymentType.IsDefault)
				await _unitOfWork.PaymentTypes.RemoveDefualtFromPaymentAsync();

			await _unitOfWork.PaymentTypes.AddAsync(paymentType);
			await _unitOfWork.CompleteAsync();
			var createdPaymentTypeDto = _mapper.Map<PaymentTypeDto>(paymentType);
			return CreatedAtAction(nameof(GetById), new { id = paymentType.Id }, new ApiResponse<PaymentTypeDto>(createdPaymentTypeDto, StatusCodes.Status201Created));
		}

		// PUT: api/PaymentType/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] PaymentTypeUpdateDto paymentTypeUpdateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (paymentTypeUpdateDto == null || id != paymentTypeUpdateDto.Id)
				return BadRequest(new ApiResponse<string>("يوجد خطأ في الرقم التعريفي.", StatusCodes.Status400BadRequest));

			if (await _unitOfWork.PaymentTypes.IsExist(e => !e.IsDeleted && e.Id != paymentTypeUpdateDto.Id && e.Name.Trim() == paymentTypeUpdateDto.Name.Trim()))
				return BadRequest(new ApiResponse<string>("إسم نوع الدفع موجود مسبقا.", StatusCodes.Status400BadRequest));

			var paymentType = await _unitOfWork.PaymentTypes.GetByIdAsync(id);
			if (paymentType == null)
				return NotFound(new ApiResponse<string>("نوع الدفع غير موجود.", StatusCodes.Status404NotFound));

			if (paymentTypeUpdateDto.PaymentNo != paymentType.PaymentNo)
				if (await _unitOfWork.PaymentSettings.IsExist(e => !e.IsDeleted && e.PaymentTypeId == paymentType.Id))
					return BadRequest(new ApiResponse<string>("لا يمكنك التعديل علي عدد الدفعات لانها مستخدمة", StatusCodes.Status400BadRequest));


			if (paymentTypeUpdateDto.IsDefault)
				await _unitOfWork.PaymentTypes.RemoveDefualtFromPaymentAsync();

			_mapper.Map(paymentTypeUpdateDto, paymentType);
			_unitOfWork.PaymentTypes.Update(paymentType);
			await _unitOfWork.CompleteAsync();
			return Ok(new ApiResponse<bool>(StatusCodes.Status200OK));
		}

		// DELETE: api/PaymentType/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var paymentType = await _unitOfWork.PaymentTypes.GetByIdAsync(id);
			if (paymentType == null)
				return NotFound(new ApiResponse<string>("نوع الدفع غير موجود.", StatusCodes.Status404NotFound));

			paymentType.IsDeleted = true;
			paymentType.DeletionDate = DateTime.UtcNow;
			_unitOfWork.PaymentTypes.Update(paymentType);
			await _unitOfWork.CompleteAsync();
			return Ok(new ApiResponse<bool>(StatusCodes.Status200OK));
		}
	}
}
