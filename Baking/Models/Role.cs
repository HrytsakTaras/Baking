using System.Collections.Generic;

namespace Baking.Models
{
	public class Role : BaseEntity
	{
		public string Name { get; set; }
		public List<User> Users { get; set; } = new List<User>();
	}
}
