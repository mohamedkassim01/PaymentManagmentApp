using System.ComponentModel.DataAnnotations;

namespace SystemPayment.API.DTO
{
	public class EducationTypeUpdateDto
	{
		[Required(ErrorMessage = "Id is required.")]
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
		public string Name { get; set; }

	}
}
