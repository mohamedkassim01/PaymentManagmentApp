using System.ComponentModel.DataAnnotations;

namespace SystemPayment.API.DTO
{
	public class PaymentTypeUpdateDto
	{
		[Required(ErrorMessage = "Id is required.")]
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
		public string Name { get; set; }

		[MaxLength(100, ErrorMessage = "NameEn cannot exceed 100 characters.")]
		public string NameEn { get; set; }

		public bool IsDefault { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "PaymentNo must be greater than 0.")]
		public int PaymentNo { get; set; }
	}
}
