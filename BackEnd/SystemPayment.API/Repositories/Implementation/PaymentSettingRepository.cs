using Microsoft.EntityFrameworkCore;
using System.Linq;
using SystemPayment.API.DataModels;
using SystemPayment.API.DTO;
using SystemPayment.API.Repositories.Interface;

namespace SystemPayment.API.Repositories.Implementation
{
	public class PaymentSettingRepository : Repository<PaymentSetting>, IPaymentSettingRepository
	{
		public PaymentSettingRepository(ApplicationDbContext context) : base(context)
		{
		}
		public async Task<IEnumerable<PaymentSetting>> GetBranchsAndEducationPaymentAsync(PaymentCheckDto paymentCheckDto)
		{
			// Logic to check if there's a matching payment setting.
			var result = await _dbSet.AsNoTracking()
				.Include(r => r.PaymentType)
				.Include(r => r.EducationType)
				.Include(r => r.EducationYear)
				.Include(r => r.Branch)
				.Where(p => p.PaymentTypeId == paymentCheckDto.PaymentTypeId
					&& p.EducationYearId == paymentCheckDto.YearId
					&& paymentCheckDto.Branchs.Contains(p.BranchId)
					&& paymentCheckDto.EducationTypes.Contains(p.EducationTypeId)
					&& !p.IsDeleted).ToListAsync();

			return result;
		}
		public async Task<IEnumerable<PaymentSetting>> GetPaymentByFilterAsync(PaymentSettingFilterDto filter)
		{
			var query = _dbSet.AsNoTracking()
				.Include(p => p.PaymentType)
				.Include(p => p.Branch)
				.Include(p => p.EducationType)
				.Include(p => p.EducationYear)
				.Where(p => !p.IsDeleted)
				.AsQueryable();

			if (filter.PaymentTypeId.HasValue)
				query = query.Where(p => p.PaymentTypeId == filter.PaymentTypeId.Value);

			if (filter.EducationYearId.HasValue)
				query = query.Where(p => p.EducationYearId == filter.EducationYearId.Value);

			if (filter.BranchIds != null && filter.BranchIds.Any())
				query = query.Where(p => filter.BranchIds.Contains(p.BranchId));

			if (filter.EducationTypeIds != null && filter.EducationTypeIds.Any())
				query = query.Where(p => filter.EducationTypeIds.Contains(p.EducationTypeId));

			return await query.ToListAsync();
		}
		public async Task<IEnumerable<int>> GetMissingPaymentSettingsAsync(List<int> ids)
		{
			var existingPaymentSettings = await _context.PaymentSettings
				.Where(p => !p.IsDeleted && ids.Contains(p.Id))
				.Select(ps => ps.Id)
				.ToListAsync();
			var missingIds = ids.Except(existingPaymentSettings).ToList();
			return missingIds;
		}
		public async Task<PaymentSetting?> GetPaymentSettingWithDetailsAsync(int id)
		{
			return await _dbSet.AsNoTracking()
				.Include(p => p.PaymentType)
				.Include(p => p.Branch)
				.Include(p => p.EducationType)
				.Include(p => p.EducationYear)
				.FirstOrDefaultAsync(ps => ps.Id == id && !ps.IsDeleted);
		}
		public List<PaymentSetting> ConvertCreateListToPaymentSettings(List<PaymentCreateSettingListDto> createPaymentSettingListDto)
		{
			var paymentSettings = createPaymentSettingListDto.SelectMany(paymentNo =>
			   paymentNo.Branches.SelectMany(branchId =>
				   paymentNo.EducationTypes.Select(educationTypeId => new PaymentSetting
				   {
					   BranchId = branchId,
					   EducationTypeId = educationTypeId,
					   EducationYearId = paymentNo.EducationYearId,
					   PaymentTypeId = paymentNo.PaymentTypeId,
					   PaymentNumber = paymentNo.PaymentNumber,
					   PaymentPercentage = paymentNo.PaymentPercentage,
					   PaymentStartDate = paymentNo.PaymentStartDate,
					   PaymentEndDate = paymentNo.PaymentEndDate,
					   CreationTime = DateTime.UtcNow,
					   IsDeleted = false
				   })
			   )
			).ToList();
			return paymentSettings;
		}
		// Example of a custom method to retrieve payment settings by BranchId
		public async Task<IEnumerable<PaymentSetting>> GetByBranchIdAsync(int branchId)
		{
			return await _dbSet.AsNoTracking().Where(ps => ps.BranchId == branchId && !ps.IsDeleted).ToListAsync();
		}

		// Example of a custom method to retrieve payment settings by EducationTypeId
		public async Task<IEnumerable<PaymentSetting>> GetByEducationTypeIdAsync(int educationTypeId)
		{
			return await _dbSet.AsNoTracking().Where(ps => ps.EducationTypeId == educationTypeId && !ps.IsDeleted).ToListAsync();
		}

	}
}
