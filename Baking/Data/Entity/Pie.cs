using System.Collections.Generic;

namespace Baking.Data.Entity
{
	public class Pie : BaseEntity
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }

		public List<OrderPie> OrderPies { get; set; }
	}
}
