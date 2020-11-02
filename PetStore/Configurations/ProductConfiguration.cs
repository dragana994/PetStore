using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Entities;

namespace PetStore.Configurations
{
	public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
	{
		public void Configure(EntityTypeBuilder<ProductEntity> builder)
		{
			builder
				.HasKey(x => x.ProductId);

			builder
				.HasOne(x => x.Brand)
				.WithMany()
				.HasForeignKey(x => x.BrandId);

			builder
				.HasIndex(x => x.Name);
		}
	}
}