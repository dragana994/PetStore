using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Entities;

namespace PetStore.Configurations
{
	public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
	{
		public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
		{
			builder
			.HasKey(x => new { x.OrderItemId, x.OrderId });

			builder
				.Property(x => x.ProductId)
				.IsRequired();

			builder
				.HasOne(x => x.Order)
				.WithMany()
				.HasForeignKey(x => x.OrderId);

			builder
				.HasOne(x => x.Product)
				.WithMany()
				.HasForeignKey(x => x.ProductId);
		}
	}
}