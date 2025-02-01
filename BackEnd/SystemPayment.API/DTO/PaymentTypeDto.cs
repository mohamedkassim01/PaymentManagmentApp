namespace SystemPayment.API.DTO
{
	public class PaymentTypeDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string NameEn { get; set; }
		public bool IsDefault { get; set; }
		public int PaymentNo { get; set; }
	}
}
