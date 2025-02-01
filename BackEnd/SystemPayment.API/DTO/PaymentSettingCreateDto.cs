using System.ComponentModel.DataAnnotations;
using SystemPayment.API.Validators;

namespace SystemPayment.API.DTO
{
	public class PaymentSettingCreateDto
	{
		[Required(ErrorMessage = "Branch is required.")]
		public int BranchId { get; set; }

		[Required(ErrorMessage = "EducationType is required.")]
		public int EducationTypeId { get; set; }

		[Required(ErrorMessage = "PaymentNumber is required.")]
		[Range(1, int.MaxValue, ErrorMessage = "PaymentNumber must be greater than 0.")]
		public int PaymentNumber { get; set; }

		[Required(ErrorMessage = "PaymentPercentage is required.")]
		[Range(0, 100, ErrorMessage = "PaymentPercentage must be between 0 and 100.")]
		public decimal PaymentPercentage { get; set; }

		[Required(ErrorMessage = "PaymentStartDate is required.")]
		[DataType(DataType.Date)]
		public DateTime PaymentStartDate { get; set; }

		[Required(ErrorMessage = "PaymentEndDate is required.")]
		[DataType(DataType.Date)]
		[DateGreaterThan(nameof(PaymentStartDate), ErrorMessage = "PaymentEndDate must be later than PaymentStartDate.")]
		public DateTime PaymentEndDate { get; set; }
	}
}
