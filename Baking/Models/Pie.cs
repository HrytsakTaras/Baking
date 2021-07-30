using System.Collections.Generic;

namespace Baking.Models
{
	public class Pie
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }

		public List<OrderPie> OrderPies { get; set; }
	}
}
