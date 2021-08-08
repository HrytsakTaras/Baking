using System.Collections.Generic;

namespace Baking.Data.Entity
{
	public class User : BaseEntity
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public bool IsRegularClient { get; set; }
		public decimal Balance { get; set; }

		public List<Order> Orders { get; set; } = new List<Order>();

		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}
