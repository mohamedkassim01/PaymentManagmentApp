namespace SystemPayment.API.DTO
{
	public class PaymentSettingDto
	{
		public int Id { get; set; }
		public int PaymentTypeId { get; set; }
		public string PaymentTypeName { get; set; }
		public int BranchId { get; set; }
		public string BranchName { get; set; }
		public int EducationTypeId { get; set; }
		public string EducationTypeName { get; set; }
		public int EducationYearId { get; set; }
		public int EducationYear { get; set; }
		public int PaymentNumber { get; set; }
		public decimal PaymentPercentage { get; set; }
		public DateTime PaymentStartDate { get; set; }
		public DateTime PaymentEndDate { get; set; }
	}
}
