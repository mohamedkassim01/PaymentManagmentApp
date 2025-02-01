using Microsoft.EntityFrameworkCore;
using SystemPayment.API.DataModels.Seed;

namespace SystemPayment.API.DataModels.Extensions
{
	public static class DatabaseExtensions
	{
		public static async Task InitialiseDatabaseAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			try
			{
				await context.Database.MigrateAsync();
				await SeedAsync(context);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error occurred when initialize database: {ex.Message}");
			}
		}
		private static async Task SeedAsync(ApplicationDbContext context)
		{
			await SeedPaymentTypesAsync(context);
			await SeedEducationYearsAsync(context);
			await SeedBranchsAsync(context);
			await SeedEducationTypesAsync(context);
		}
		private static async Task SeedEducationYearsAsync(ApplicationDbContext context)
		{
			if (!await context.EducationYears.AnyAsync())
			{
				await context.EducationYears.AddRangeAsync(InitialData.EducationYears);
				await context.SaveChangesAsync();
			}
		}
		private static async Task SeedPaymentTypesAsync(ApplicationDbContext context)
		{
			if (!await context.PaymentTypes.AnyAsync())
			{
				await context.PaymentTypes.AddRangeAsync(InitialData.PaymentTypes);
				await context.SaveChangesAsync();
			}
		}
		private static async Task SeedBranchsAsync(ApplicationDbContext context)
		{
			if (!await context.Branches.AnyAsync())
			{
				await context.Branches.AddRangeAsync(InitialData.Branchs);
				await context.SaveChangesAsync();
			}
		}

		private static async Task SeedEducationTypesAsync(ApplicationDbContext context)
		{
			if (!await context.EducationTypes.AnyAsync())
			{
				await context.EducationTypes.AddRangeAsync(InitialData.EducationTypes);
				await context.SaveChangesAsync();
			}
		}

	}
}
