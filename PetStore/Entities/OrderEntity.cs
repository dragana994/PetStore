using System;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Entities
{
	public class OrderEntity
	{
		public int OrderId { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		[MaxLength(30)]
		public string CustomerFirstName { get; set; }
		[Required]
		[MaxLength(30)]
		public string CustomerLastName { get; set; }
		[Required]
		[MaxLength(30)]
		public string CustomerAddress { get; set; }
		[Required]
		[MaxLength(20)]
		public string CustomerCreditCardNumber { get; set; }

		[MaxLength(1)]
		public string Status { get; set; }
	}

	public static class OrderStatus
	{
		public static string Successful = "S";
		public static string Unsuccessful = "N";
	}
}