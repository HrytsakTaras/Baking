using Baking.Data.Entity;
using Baking.Data.Enums;
using System;
using System.Collections.Generic;

namespace Baking.ViewModels
{
	public class OrderViewModel
	{
		public OrderStatus Status { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public decimal Deposit { get; set; }

		public DateTime ExecutionDate { get; set; }
	}
}
