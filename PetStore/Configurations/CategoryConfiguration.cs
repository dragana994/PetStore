using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Entities;

namespace PetStore.Configurations
{
	public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
	{
		public void Configure(EntityTypeBuilder<CategoryEntity> builder)
		{
			builder
				.HasKey(x => x.CategoryId);
			
			builder
				.HasOne(x => x.SuperCategory)
				.WithMany()
				.HasForeignKey(x => x.SuperCategoryId);
		}
	}
}