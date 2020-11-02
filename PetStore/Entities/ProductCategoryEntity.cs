namespace PetStore.Entities
{
	public class ProductCategoryEntity
	{
		public int ProductId { get; set; }
		public int CategoryId { get; set; }

		public ProductEntity Product { get; set; }

		public CategoryEntity Category { get; set; }
	}
}