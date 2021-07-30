﻿namespace Baking.Models
{
	public class User : BaseEntity
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public bool IsRegularClient { get; set; }

		public int? RoleId { get; set; }
		public Role Role { get; set; }
	}
}