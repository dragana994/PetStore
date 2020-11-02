using System.ComponentModel.DataAnnotations;

namespace PetStore.Entities
{
	public class CategoryEntity
	{
		public int CategoryId { get; set; }

		[Required]
		[MaxLength(30)]
		public string Name { get; set; }
		[MaxLength(250)]
		public string Description { get; set; }

		public int? SuperCategoryId { get; set; }

		public CategoryEntity SuperCategory { get; set; }
	}
}