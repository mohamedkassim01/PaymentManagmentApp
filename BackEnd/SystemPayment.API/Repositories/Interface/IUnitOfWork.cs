namespace SystemPayment.API.Repositories.Interface
{
	public interface IUnitOfWork : IDisposable
	{
		IPaymentTypeRepository PaymentTypes { get; }
		IBranchRepository Branches { get; }
		IEducationYearRepository EducationYears { get; }
		IEducationTypeRepository EducationTypes { get; }
		IPaymentSettingRepository PaymentSettings { get; }
		Task<int> CompleteAsync();
	}
}
