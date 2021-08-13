using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baking.Data.Entities
{
	public class OrderPie : BaseEntity
	{
		//public DateTime ExecutionDate { get; set; }

		[ForeignKey("Order")]
		public int OrderId { get; set; }
		public Order Order { get; set; }

		[ForeignKey("Pie")]
		public int PieId { get; set; }
		public Pie Pie { get; set; }
	}
}
