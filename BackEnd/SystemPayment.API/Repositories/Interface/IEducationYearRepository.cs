using Microsoft.EntityFrameworkCore;
using SystemPayment.API.DataModels;

namespace SystemPayment.API.Repositories.Interface
{
	public interface IEducationYearRepository : IRepository<EducationYear>
	{
		Task<IEnumerable<EducationYear>> GetAllWithOrderAsync();
	}
}
