using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemPayment.API.DTO;
using SystemPayment.API.Repositories.Interface;
using SystemPayment.API.Response;

namespace SystemPayment.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BranchController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public BranchController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		// GET: api/Branch
		[HttpGet("GetAll")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var branches = await _unitOfWork.Branches.GetAllAsync(e => !e.IsDeleted);
			var branchesDto = _mapper.Map<IEnumerable<BranchDto>>(branches);
			return Ok(new ApiResponse<IEnumerable<BranchDto>>(branchesDto));
		}

		// GET: api/Branch/{id}
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<BranchDto>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(int id)
		{
			var branch = await _unitOfWork.Branches.GetByIdAsync(id);
			if (branch == null)
				return NotFound(new ApiResponse<Branch>("Branch not found.", StatusCodes.Status404NotFound));
			
			var branchDto = _mapper.Map<BranchDto>(branch);
			return Ok(new ApiResponse<BranchDto>(branchDto));
		}

		// POST: api/Branch
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<BranchDto>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] BranchCreateDto branchCreateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (await _unitOfWork.Branches.IsExist(e => e.Name.Trim() == branchCreateDto.Name.Trim()))
				return BadRequest(new ApiResponse<Branch>("This Branch is Exist.", StatusCodes.Status400BadRequest));

			var branch = _mapper.Map<Branch>(branchCreateDto);

			await _unitOfWork.Branches.AddAsync(branch);
			await _unitOfWork.CompleteAsync();

			var branchDto = _mapper.Map<BranchDto>(branch);
			return CreatedAtAction(nameof(GetById), new { id = branchDto.Id }, new ApiResponse<BranchDto>(branchDto));

		}

		// PUT: api/Branch/{id}
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ApiResponse<BranchDto>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<BranchDto>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update(int id, [FromBody] BranchUpdateDto branchUpdateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (branchUpdateDto == null || id != branchUpdateDto.Id)
				return BadRequest(new ApiResponse<Branch>("Mismatched ID.", StatusCodes.Status400BadRequest));

			var branch = await _unitOfWork.Branches.GetByIdAsync(id);
			if (branch == null)
				return NotFound(new ApiResponse<Branch>("Branch not found.", StatusCodes.Status404NotFound));

			var updatedBranch = _mapper.Map(branchUpdateDto, branch);

			_unitOfWork.Branches.Update(updatedBranch);
			await _unitOfWork.CompleteAsync();

			return NoContent();
		}

		// DELETE: api/Branch/{id}
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ApiResponse<BranchDto>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			var branch = await _unitOfWork.Branches.GetByIdAsync(id);
			if (branch == null)
				return NotFound(new ApiResponse<Branch>("Branch not found.", StatusCodes.Status404NotFound));

			branch.IsDeleted = true;
			branch.DeletionDate = DateTime.UtcNow;
			_unitOfWork.Branches.Update(branch);
			await _unitOfWork.CompleteAsync();

			return NoContent();
		}
	}
}

