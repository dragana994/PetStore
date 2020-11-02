using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Entities
{
	public class UserEntity : IdentityUser<int>
	{
		[Required]
		[MaxLength(30)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(30)]
		public string LastName { get; set; }
	}
}