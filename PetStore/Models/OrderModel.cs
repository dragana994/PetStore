using System;
using System.Collections.Generic;

namespace PetStore.Models
{
	public class OrderModel
	{
		public int? OrderId { get; set; }

		public DateTime? Date { get; set; }

		public string CustomerFirstName { get; set; }
		public string CustomerLastName { get; set; }
		public string CustomerAddress { get; set; }
		public string CustomerCreditCardNumber { get; set; }

		public double? Amount { get; set; }

		public string OrderStatus { get; set; }

		public List<OrderItemModel> Items { get; set; }
	}
}