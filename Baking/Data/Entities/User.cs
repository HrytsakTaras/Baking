using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baking.Data.Entities
{
	public class User : BaseEntity
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public bool IsRegularClient { get; set; }
		public decimal Balance { get; set; }
		
		[NotMapped]
		public string ConfirmPassword { get;  set; }

		public List<Order> Orders { get; set; } = new List<Order>();

		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}
