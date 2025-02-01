using System.ComponentModel.DataAnnotations;

namespace SystemPayment.API.DTO
{
	public class BranchCreateDto
	{
		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
		public string Name { get; set; }
	}
}
