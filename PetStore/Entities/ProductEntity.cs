using System.ComponentModel.DataAnnotations;

namespace PetStore.Entities
{
	public class ProductEntity
	{
		public int ProductId { get; set; }

		[Required]
		[MaxLength(30)]
		public string Name { get; set; }
		[MaxLength(250)]
		public string Description { get; set; }
		public int? Review { get; set; }

		[Required]
		public double Price { get; set; }
		[Required]
		public int Amount { get; set; }

		public string ImagePath { get; set; }

		public int? BrandId { get; set; }

		public BrandEntity Brand { get; set; }
	}
}