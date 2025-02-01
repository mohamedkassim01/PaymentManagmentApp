public class PaymentType
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string NameEn { get; set; }
	public bool IsDefault { get; set; } = false;
	public int PaymentNo { get; set; }
	public bool IsDeleted { get; set; } = false;
	public DateTime? DeletionDate { get; set; }
	public DateTime CreationTime { get; set; } = DateTime.UtcNow;
}
