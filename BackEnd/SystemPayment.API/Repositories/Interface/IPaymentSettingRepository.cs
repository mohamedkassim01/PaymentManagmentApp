using Microsoft.EntityFrameworkCore;
using SystemPayment.API.DataModels;
using SystemPayment.API.DTO;

namespace SystemPayment.API.Repositories.Interface
{
	public interface IPaymentSettingRepository : IRepository<PaymentSetting>
	{
		Task<PaymentSetting?> GetPaymentSettingWithDetailsAsync(int id);
		Task<IEnumerable<PaymentSetting>> GetBranchsAndEducationPaymentAsync(PaymentCheckDto paymentCheckDto);
		Task<IEnumerable<PaymentSetting>> GetByBranchIdAsync(int branchId);
		Task<IEnumerable<PaymentSetting>> GetByEducationTypeIdAsync(int educationTypeId);
		Task<IEnumerable<PaymentSetting>> GetPaymentByFilterAsync(PaymentSettingFilterDto filter);
		Task<IEnumerable<int>> GetMissingPaymentSettingsAsync(List<int> ids);
		List<PaymentSetting> ConvertCreateListToPaymentSettings(List<PaymentCreateSettingListDto> createPaymentSettingListDto);

	}
}
