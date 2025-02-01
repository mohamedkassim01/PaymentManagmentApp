namespace SystemPayment.API.DataModels
{
	public class PaymentSetting
	{
		public int Id { get; set; }

		public int PaymentTypeId { get; set; } 
		public PaymentType PaymentType { get; set; }

		public int BranchId { get; set; }
		public Branch Branch { get; set; }

		public int EducationTypeId { get; set; }
		public EducationType EducationType { get; set; }

		public int EducationYearId { get; set; }
		public EducationYear EducationYear { get; set; }

		public int PaymentNumber { get; set; }
		public decimal PaymentPercentage { get; set; }
		public DateTime PaymentStartDate { get; set; }
		public DateTime PaymentEndDate { get; set; }

		public bool IsDeleted { get; set; } = false;
		public DateTime? DeletionDate { get; set; }
		public DateTime CreationTime { get; set; } = DateTime.UtcNow;
	}
}
