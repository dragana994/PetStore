using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Entities;

namespace PetStore.Configurations
{
	public class BrandConfiguration : IEntityTypeConfiguration<BrandEntity>
	{
		public void Configure(EntityTypeBuilder<BrandEntity> builder)
		{
			builder
				.HasKey(x => x.BrandId);
		}
	}
}