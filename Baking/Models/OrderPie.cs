using System;

namespace Baking.Models
{
	public class OrderPie : BaseEntity
	{
		public int OrderId { get; set; }
		public Order Order { get; set; }

		public int PieId { get; set; }
		public Pie Pie { get; set; }

		public DateTime ExecutionDate { get; set; }
	}
}
