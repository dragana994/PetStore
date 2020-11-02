using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PetStore.Configurations;

namespace PetStore.Entities
{
	public class PetStoreDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
	{
		public DbSet<BrandEntity> Brands { get; set; }
		public DbSet<CategoryEntity> Categories { get; set; }
		public DbSet<OrderEntity> Orders { get; set; }
		public DbSet<OrderItemEntity> OrderItems { get; set; }
		public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
		public DbSet<ProductEntity> Products { get; set; }

		public PetStoreDbContext()
		{ }

		public PetStoreDbContext(DbContextOptions<PetStoreDbContext> options) : base(options)
		{ }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "pet_store.db" };
			var connectionString = connectionStringBuilder.ToString();
			var connection = new SqliteConnection(connectionString);

			optionsBuilder.UseSqlite(connection);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfiguration(new BrandConfiguration());
			builder.ApplyConfiguration(new CategoryConfiguration());
			builder.ApplyConfiguration(new OrderConfiguration());
			builder.ApplyConfiguration(new OrderItemConfiguration());
			builder.ApplyConfiguration(new ProductCategoryConfiguration());
			builder.ApplyConfiguration(new ProductConfiguration());
		}
	}
}