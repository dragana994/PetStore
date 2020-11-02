namespace PetStore.Models
{
	public class CategoryModel
	{
		public int CategoryId { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public int? SuperCategoryId { get; set; }
		public string SuperCategoryName { get; set; }
	}
}