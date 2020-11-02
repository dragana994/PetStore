using System.ComponentModel.DataAnnotations;

namespace PetStore.Entities
{
	public class OrderItemEntity
	{
		public short OrderItemId { get; set; }
		public int OrderId { get; set; }

		public int ProductId { get; set; }

		[Required]
		public int Amount { get; set; }
		[Required]
		public double UnitPrice { get; set; }

		public OrderEntity Order { get; set; }
		public ProductEntity Product { get; set; }
	}
}