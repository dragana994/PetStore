using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Entities;

namespace PetStore.Configurations
{
	public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategoryEntity>
	{
		public void Configure(EntityTypeBuilder<ProductCategoryEntity> builder)
		{
			builder
			.HasKey(x => new { x.ProductId, x.CategoryId });

			builder
				.HasOne(x => x.Product)
				.WithMany()
				.HasForeignKey(x => x.ProductId);

			builder
				.HasOne(x => x.Category)
				.WithMany()
				.HasForeignKey(x => x.CategoryId);
		}
	}
}