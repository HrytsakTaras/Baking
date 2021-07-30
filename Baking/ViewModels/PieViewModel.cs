using Baking.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
