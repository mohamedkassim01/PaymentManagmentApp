namespace SystemPayment.API.DTO
{
	public class PaymentSettingFilterDto
	{
		public int? PaymentTypeId { get; set; }
		public List<int> BranchIds { get; set; } = new();
		public List<int> EducationTypeIds { get; set; } = new();
		public int? EducationYearId { get; set; }
	}
}
