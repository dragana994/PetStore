namespace PetStore.Models
{
	public class OrderItemModel
	{
		public short OrderItemId { get; set; }
		public int OrderId { get; set; }

		public int ProductId { get; set; }
		public string ProductName { get; set; }

		public int Amount { get; set; }
		public double? UnitPrice { get; set; }
	}
}