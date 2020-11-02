using System.Collections.Generic;

namespace PetStore.Models
{
	public class ProductModel
	{
		public int? ProductId { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		public int? Review { get; set; }

		public double Price { get; set; }
		public int Amount { get; set; }

		public string ImagePath { get; set; }

		public int? BrandId { get; set; }
		public string BrandName { get; set; }

		public List<CategoryModel> Categories { get; set; }
	}
}