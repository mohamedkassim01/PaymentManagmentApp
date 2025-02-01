using Microsoft.EntityFrameworkCore;
using SystemPayment.API.DataModels;
using SystemPayment.API.Repositories.Interface;

namespace SystemPayment.API.Repositories.Implementation
{
	public class EducationYearRepository : Repository<EducationYear>, IEducationYearRepository
	{
		public EducationYearRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<EducationYear>> GetAllWithOrderAsync()
		{
			return await _dbSet.AsNoTracking().Where(ps => !ps.IsDeleted).OrderBy(o => o.Year).ToListAsync();
		}
	}
}
