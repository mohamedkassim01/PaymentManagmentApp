using System.ComponentModel.DataAnnotations;

namespace SystemPayment.API.DTO
{
	public class PaymentTypeCreateDto
	{
		[Required(ErrorMessage = "نوع الدفع بالعربية مطلوب.")]
		[MaxLength(100, ErrorMessage = "نوع الدفع باللغة العربية لايمكن ان يتجاوز 100 حرف")]
		public string Name { get; set; }

		[Required(ErrorMessage = "نوع الدفع بالإنجليزية مطلوب.")]
		[MaxLength(100, ErrorMessage = "نوع الدفع باللغة بالإنجليزية لايمكن ان يتجاوز 100 حرف")]
		public string NameEn { get; set; }

		[Required(ErrorMessage = "نوع الدفع الافتراضي مطلوب.")]
		public bool IsDefault { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "عدد الدفعات لابد ان يكون اكبر من 0.")]
		public int PaymentNo { get; set; }
	}
}
