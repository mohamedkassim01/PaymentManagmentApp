using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SystemPayment.API.DataModels;
using SystemPayment.API.DTO;
using SystemPayment.API.Repositories.Interface;
using SystemPayment.API.Response;

namespace SystemPayment.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EducationYearController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public EducationYearController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		// GET: api/EducationYear
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var educationYears = await _unitOfWork.EducationYears.GetAllWithOrderAsync();
			var educationYearsDto = _mapper.Map<IEnumerable<EducationYearDto>>(educationYears);
			return Ok(new ApiResponse<IEnumerable<EducationYearDto>>(educationYearsDto));
		}

		// GET: api/EducationYear/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var educationYear = await _unitOfWork.EducationYears.GetByIdAsync(id);
			if (educationYear == null)
				return NotFound(new ApiResponse<EducationYear>("Education Year not found.", StatusCodes.Status404NotFound));

			var educationYearDto = _mapper.Map<EducationYearDto>(educationYear);
			return Ok(new ApiResponse<EducationYearDto>(educationYearDto));
		}

		// POST: api/EducationYear
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] EducationYearCreateDto educationYearCreateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (await _unitOfWork.EducationYears.IsExist(e => e.Year.Equals(educationYearCreateDto.Year)))
				return BadRequest(new ApiResponse<EducationYear>("This Education Year is Exist.", StatusCodes.Status400BadRequest));

			var educationYear = _mapper.Map<EducationYear>(educationYearCreateDto);

			await _unitOfWork.EducationYears.AddAsync(educationYear);
			await _unitOfWork.CompleteAsync();

			var educationYearDto = _mapper.Map<EducationYearDto>(educationYear);
			return CreatedAtAction(nameof(GetById), new { id = educationYearDto.Id }, new ApiResponse<EducationYearDto>(educationYearDto));
		}

		// PUT: api/EducationYear/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] EducationYearUpdateDto educationYearUpdateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (educationYearUpdateDto == null || id != educationYearUpdateDto.Id)
				return BadRequest(new ApiResponse<EducationYear>("Mismatched ID.", StatusCodes.Status400BadRequest));

			var educationYear = await _unitOfWork.EducationYears.GetByIdAsync(id);
			if (educationYear == null)
				return NotFound(new ApiResponse<EducationYear>("Education Year not found.", StatusCodes.Status404NotFound));

			var updatedEducationYear = _mapper.Map(educationYearUpdateDto, educationYear);
			_unitOfWork.EducationYears.Update(updatedEducationYear);
			await _unitOfWork.CompleteAsync();

			return NoContent();
		}

		// DELETE: api/EducationYear/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var educationYear = await _unitOfWork.EducationYears.GetByIdAsync(id);
			if (educationYear == null)
				return NotFound(new ApiResponse<EducationYear>("Education Year not found.", StatusCodes.Status404NotFound));

			educationYear.IsDeleted = true;
			educationYear.DeletionDate = DateTime.UtcNow;
			_unitOfWork.EducationYears.Update(educationYear);
			await _unitOfWork.CompleteAsync();

			return NoContent();
		}
	}
}
