using System;

namespace Baking.Models
{
	public class OrderPie
	{
		public int Id { get; set; }

		public int OrderId { get; set; }
		public Order Order { get; set; }

		public int PieId { get; set; }
		public Pie Pie { get; set; }

		public DateTime ExecutionDate { get; set; }
	}
}
