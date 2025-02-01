using System.ComponentModel.DataAnnotations;

namespace SystemPayment.API.DTO
{
	public class BranchUpdateDto
	{

		[Required(ErrorMessage = "Id is required.")]
		public int Id { get; set; }

		// Name is required and cannot exceed 100 characters
		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
		public string Name { get; set; }

	}
}
