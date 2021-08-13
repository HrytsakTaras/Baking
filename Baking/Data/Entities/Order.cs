using Baking.Data.Enums;
using System;
using System.Collections.Generic;

namespace Baking.Data.Entities
{
	public class Order : BaseEntity
	{
		public OrderStatus Status { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public decimal Deposit { get; set; }
		public DateTime ExecutionDate { get; set; }

		public User User { get; set; }
		public int UserId { get; set; }

		public List<OrderPie> OrderPies { get; set; }
	}
}
