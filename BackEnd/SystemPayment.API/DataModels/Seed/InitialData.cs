namespace SystemPayment.API.DataModels.Seed
{

	internal static class InitialData
	{
		public static IEnumerable<PaymentType> PaymentTypes =>
			new List<PaymentType>
			{
				new PaymentType {  Name =  "دفعة واحدة", NameEn =  "1 Payment", PaymentNo = 1},
				new PaymentType { Name =  "دفعتان", NameEn =  "2 Payment", PaymentNo = 2},
				new PaymentType {  Name =  "3 دفعات", NameEn =  "3 Payment", PaymentNo = 3},
			};
		public static IEnumerable<Branch> Branchs =>
			new List<Branch>
			{
				new Branch { Name = "الإسكندرية" },
				new Branch { Name = "التجمع الخامس" }
			};
		public static IEnumerable<EducationType> EducationTypes =>
			new List<EducationType>
			{
				new EducationType {	Name = "مصري" },
				new EducationType { Name = "أهلي" },
				new EducationType {	Name = "عالمي" }
			};
		public static IEnumerable<EducationYear> EducationYears =>
			new List<EducationYear>
			{
				new EducationYear { Year = 2024 },
				new EducationYear { Year = 2025 },
				new EducationYear { Year = 2026 }
			};

	}
}
