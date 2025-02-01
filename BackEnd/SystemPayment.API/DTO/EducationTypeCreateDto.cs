using System.ComponentModel.DataAnnotations;

namespace SystemPayment.API.DTO
{
	public class EducationTypeCreateDto
	{

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
		public string Name { get; set; }

	}
}
