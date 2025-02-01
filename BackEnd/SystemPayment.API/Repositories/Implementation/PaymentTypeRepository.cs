using Microsoft.EntityFrameworkCore;
using SystemPayment.API.DataModels;
using SystemPayment.API.Repositories.Interface;

namespace SystemPayment.API.Repositories.Implementation
{
	public class PaymentTypeRepository : Repository<PaymentType>, IPaymentTypeRepository
	{
		public PaymentTypeRepository(ApplicationDbContext context) : base(context)
		{
		}
		public async Task RemoveDefualtFromPaymentAsync()
		{
			await _dbSet.Where(e => e.IsDefault && !e.IsDeleted)
			.ForEachAsync(e => e.IsDefault = false);
		}
		public async Task<bool> AnyDefualtPaymentAsync()
		{
			return await _dbSet.AnyAsync(e => e.IsDefault && !e.IsDeleted);
		}
	}
}
