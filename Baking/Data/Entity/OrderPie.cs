using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baking.Data.Entity
{
	public class OrderPie : BaseEntity
	{
		[Key]
		[Column(Order=0)]
		[ForeignKey("Order")]
		public int OrderId { get; set; }
		public Order Order { get; set; }

		[Key]
		[Column(Order = 1)]
		[ForeignKey("Pie")]
		public int PieId { get; set; }
		public Pie Pie { get; set; }

		public DateTime ExecutionDate { get; set; }
	}
}
