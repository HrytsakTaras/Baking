using Baking.Data.Entity;
using Baking.Data.Enums;
using Microsoft.AspNetCore.Http;
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

		public string Name { get; set; }
		public decimal Price { get; set; }
	}
}
