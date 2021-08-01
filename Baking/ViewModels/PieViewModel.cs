using Baking.Data.Entity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Baking.ViewModels
{
	public class PieViewModel
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public IFormFile Image { get; set; }

		public List<OrderPie> OrderPies { get; set; }
	}
}
