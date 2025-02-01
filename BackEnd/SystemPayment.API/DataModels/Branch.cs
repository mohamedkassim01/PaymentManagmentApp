public class Branch
{
	public int Id { get; set; }
	public string Name { get; set; }
	public bool IsDeleted { get; set; } = false;
	public DateTime? DeletionDate { get; set; }
	public DateTime CreationTime { get; set; } = DateTime.UtcNow;

}
