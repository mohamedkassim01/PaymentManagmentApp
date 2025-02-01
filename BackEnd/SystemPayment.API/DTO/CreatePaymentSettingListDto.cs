using System.ComponentModel.DataAnnotations;

namespace SystemPayment.API.DTO
{
	public class PaymentCreateSettingListDto
	{
		[Required(ErrorMessage = "Payment Type ID is required.")]
		public int PaymentTypeId { get; set; }

		[Required(ErrorMessage = "Education Year is required.")]
		public int EducationYearId { get; set; }
		
		[Required(ErrorMessage = "Payment Number is required.")]
		public int PaymentNumber { get; set; }

		[Required(ErrorMessage = "Payment Percentage is required.")]
		[Range(1, 100, ErrorMessage = "Payment Percentage must be between 1 and 100.")]
		public decimal PaymentPercentage { get; set; }

		[Required(ErrorMessage = "Payment Start Date is required.")]
		public DateTime PaymentStartDate { get; set; }

		[Required(ErrorMessage = "Payment End Date is required.")]
		public DateTime PaymentEndDate { get; set; }

		[Required(ErrorMessage = "List of Branches is required.")]
		public List<int> Branches { get; set; }

		[Required(ErrorMessage = "List of Education Types is required.")]
		public List<int> EducationTypes { get; set; }
	}
}
