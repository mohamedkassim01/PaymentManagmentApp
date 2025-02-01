namespace SystemPayment.API.DataModels
{
	public class EducationYear
	{
		public int Id { get; set; }
		public int Year { get; set; }
		public bool IsDeleted { get; set; } = false;
		public DateTime? DeletionDate { get; set; }
		public DateTime CreationTime { get; set; } = DateTime.UtcNow;
	}
}