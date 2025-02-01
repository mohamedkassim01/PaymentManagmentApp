using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SystemPayment.API.DTO;
using SystemPayment.API.Repositories.Interface;
using SystemPayment.API.Response;

namespace SystemPayment.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EducationTypeController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public EducationTypeController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		// GET: api/EducationType
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var educationTypes = await _unitOfWork.EducationTypes.GetAllAsync(e => !e.IsDeleted);
			var educationTypesDto = _mapper.Map<IEnumerable<EducationTypeDto>>(educationTypes);
			return Ok(new ApiResponse<IEnumerable<EducationTypeDto>>(educationTypesDto));
		}

		// GET: api/EducationType/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var educationType = await _unitOfWork.EducationTypes.GetByIdAsync(id);
			if (educationType == null)
				return NotFound(new ApiResponse<EducationType>("Education Type not found.", StatusCodes.Status404NotFound));

			var educationTypeDto = _mapper.Map<EducationTypeDto>(educationType);
			return Ok(new ApiResponse<EducationTypeDto>(educationTypeDto));
		}

		// POST: api/EducationType
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] EducationTypeCreateDto educationTypeCreateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (await _unitOfWork.EducationTypes.IsExist(e => e.Name.Trim() == educationTypeCreateDto.Name.Trim()))
				return BadRequest(new ApiResponse<EducationType>("This Education Type is Exist.", StatusCodes.Status400BadRequest));

			var educationType = _mapper.Map<EducationType>(educationTypeCreateDto);

			await _unitOfWork.EducationTypes.AddAsync(educationType);
			await _unitOfWork.CompleteAsync();

			var educationTypeDto = _mapper.Map<EducationTypeDto>(educationType);
			return CreatedAtAction(nameof(GetById), new { id = educationTypeDto.Id }, new ApiResponse<EducationTypeDto>(educationTypeDto));
		}

		// PUT: api/EducationType/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] EducationTypeUpdateDto educationTypeUpdateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (educationTypeUpdateDto == null || id != educationTypeUpdateDto.Id)
				return BadRequest(new ApiResponse<EducationType>("Mismatched ID.", StatusCodes.Status400BadRequest));

			var educationType = await _unitOfWork.EducationTypes.GetByIdAsync(id);
			if (educationType == null)
				return NotFound(new ApiResponse<EducationType>("Education Type not found.", StatusCodes.Status404NotFound));

			var updatedEducationType = _mapper.Map(educationTypeUpdateDto, educationType);
			_unitOfWork.EducationTypes.Update(updatedEducationType);
			await _unitOfWork.CompleteAsync();

			return NoContent();
		}

		// DELETE: api/EducationType/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var educationType = await _unitOfWork.EducationTypes.GetByIdAsync(id);
			if (educationType == null)
				return NotFound(new ApiResponse<EducationType>("Education Type not found.", StatusCodes.Status404NotFound));

			educationType.IsDeleted = true;
			educationType.DeletionDate = DateTime.UtcNow;
			_unitOfWork.EducationTypes.Update(educationType);
			await _unitOfWork.CompleteAsync();

			return NoContent();
		}
	}
}
