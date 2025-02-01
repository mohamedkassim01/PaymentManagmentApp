using SystemPayment.API.DataModels;
using SystemPayment.API.Repositories.Interface;

namespace SystemPayment.API.Repositories.Implementation
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;

		public IPaymentTypeRepository PaymentTypes { get; private set; }
		public IBranchRepository Branches { get; private set; }
		public IEducationTypeRepository EducationTypes { get; private set; }
		public IEducationYearRepository EducationYears { get; private set; }
		public IPaymentSettingRepository PaymentSettings { get; private set; }

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			PaymentTypes = new PaymentTypeRepository(context);
			Branches = new BranchRepository(context);
			EducationTypes = new EducationTypeRepository(context);
			PaymentSettings = new PaymentSettingRepository(context);
			EducationYears = new EducationYearRepository(context);
		}

		public async Task<int> CompleteAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
