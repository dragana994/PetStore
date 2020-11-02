using System.ComponentModel.DataAnnotations;

namespace PetStore.Entities
{
	public class BrandEntity
	{
		public int BrandId { get; set; }

		[Required]
		[MaxLength(30)]
		public string Name { get; set; }
	}
}