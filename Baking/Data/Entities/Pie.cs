using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baking.Data.Entities
{
	public class Pie : BaseEntity
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }
		[NotMapped]
		public IFormFile file { get; set; }
		public string Description { get; set; }

		public List<OrderPie> OrderPies { get; set; }
	}
}
