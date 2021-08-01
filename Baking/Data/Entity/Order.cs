using System;
using System.Collections.Generic;

namespace Baking.Data.Entity
{
	public enum Statuses
	{
		Canceled,
		InProgress,
		Succeed
	}
	public class Order : BaseEntity
	{
		public Statuses Status { get; set; }
		public DateTime CreationDate { get; set; }
		public decimal Deposit { get; set; }

		public List<OrderPie> OrderPies { get; set; }
	}
}
