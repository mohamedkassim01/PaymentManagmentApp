using Microsoft.EntityFrameworkCore;
using SystemPayment.API.DataModels;
using SystemPayment.API.Repositories.Interface;

namespace SystemPayment.API.Repositories.Implementation
{
	public class EducationTypeRepository : Repository<EducationType>, IEducationTypeRepository
	{
		public EducationTypeRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
