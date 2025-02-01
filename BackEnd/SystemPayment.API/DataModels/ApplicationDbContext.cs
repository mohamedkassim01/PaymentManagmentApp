using Microsoft.EntityFrameworkCore;

namespace SystemPayment.API.DataModels
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<PaymentSetting> PaymentSettings { get; set; }
		public DbSet<Branch> Branches { get; set; }
		public DbSet<EducationType> EducationTypes { get; set; }
		public DbSet<EducationYear> EducationYears { get; set; }
		public DbSet<PaymentType> PaymentTypes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<PaymentSetting>()
				.HasOne(ps => ps.Branch)
				.WithMany()
				.HasForeignKey(ps => ps.BranchId);

			modelBuilder.Entity<PaymentSetting>()
				.HasOne(ps => ps.EducationType)
				.WithMany()
				.HasForeignKey(ps => ps.EducationTypeId);

			modelBuilder.Entity<PaymentSetting>()
						.HasOne(ps => ps.PaymentType)
						.WithMany()
						.HasForeignKey(ps => ps.PaymentTypeId);

			modelBuilder.Entity<PaymentSetting>()
						.HasOne(ps => ps.EducationYear)
						.WithMany()
						.HasForeignKey(ps => ps.EducationYearId);

			// Indexes

			modelBuilder.Entity<PaymentSetting>()
				.HasIndex(ps => ps.BranchId)
				.HasDatabaseName("IX_PaymentSetting_BranchId");

			modelBuilder.Entity<PaymentSetting>()
				.HasIndex(ps => ps.EducationTypeId)
				.HasDatabaseName("IX_PaymentSetting_EducationTypeId");

			modelBuilder.Entity<PaymentSetting>()
				.HasIndex(ps => ps.EducationYearId)
				.HasDatabaseName("IX_PaymentSetting_EducationYearId");

			modelBuilder.Entity<PaymentSetting>()
				.HasIndex(ps => new { ps.EducationYearId, ps.BranchId, ps.EducationTypeId })
				.HasDatabaseName("IX_PaymentSetting_EducationYearId_BranchId_EducationTypeId");

		}
	}
}
