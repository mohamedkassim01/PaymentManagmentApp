namespace SystemPayment.API.Repositories.Interface
{
	public interface IPaymentTypeRepository : IRepository<PaymentType>
	{
		public Task RemoveDefualtFromPaymentAsync();
		public Task<bool> AnyDefualtPaymentAsync();
	}
}
