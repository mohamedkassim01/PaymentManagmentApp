using System.ComponentModel.DataAnnotations;

namespace SystemPayment.API.DTO
{
	public class EducationYearCreateDto
	{

		[Required(ErrorMessage = "Year is required.")]
		[Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
		public int Year { get; set; }
	}
}
