using Baking.Data.Enums;
using System.Collections.Generic;

namespace Baking.Data.Entities
{
	public class Role : BaseEntity
	{
		public string Name { get; set; }
		public List<User> Users { get; set; } = new List<User>();
	}
}
