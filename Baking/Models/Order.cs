using System;
using System.Collections.Generic;

namespace Baking.Models
{
	public enum Statuses
	{
		Canceled,
		Succeed
	}
	public class Order
	{
		public int Id { get; set; }
		public Statuses Status { get; set; }
		public DateTime CreationDate { get; set; }
		public decimal deposit { get; set; }

		public List<OrderPie> OrderPies { get; set; }
	}
}
