using Microsoft.EntityFrameworkCore;

namespace Baking.Models
{
	public class ApplicationContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Pie> Pies { get; set; }
		public DbSet<OrderPie> OrderPies { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrderPie>().HasKey(sc => new { sc.OrderId, sc.PieId });

			string adminRoleName = "admin";
			string userRoleName = "user";

			string adminEmail = "admin@test.com";
			string adminPassword = "123456";

			Role adminRole = new Role { Id = 1, Name = adminRoleName };
			Role userRole = new Role { Id = 2, Name = userRoleName };
			User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

			modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
			modelBuilder.Entity<User>().HasData(new User[] { adminUser });
			base.OnModelCreating(modelBuilder);
		}
	}
}
