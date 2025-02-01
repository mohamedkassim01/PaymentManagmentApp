namespace SystemPayment.API.DTO
{
	public class PaymentCheckDto
	{
		public int PaymentTypeId { get; set; }
		public int YearId { get; set; }
		public IEnumerable<int> Branchs { get; set; }
		public IEnumerable<int> EducationTypes { get; set; }
	}
}
