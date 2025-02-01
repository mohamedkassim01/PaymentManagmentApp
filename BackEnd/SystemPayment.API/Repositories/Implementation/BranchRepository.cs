using Microsoft.EntityFrameworkCore;
using SystemPayment.API.DataModels;
using SystemPayment.API.Repositories.Interface;

namespace SystemPayment.API.Repositories.Implementation
{
	public class BranchRepository : Repository<Branch>, IBranchRepository
	{
		public BranchRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
