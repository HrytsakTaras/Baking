using Baking.Data.Entity;
using Baking.Data.Enums;
using Baking.IRepositories;
using Baking.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Services
{
	public class UserService : IUserService
	{
		private readonly IGenericRepository<User> _userRepository;
		private readonly IGenericRepository<Order> _orderRepository;
		private readonly IGenericRepository<Role> _roleRepository;
		public UserService(IGenericRepository<User> userRepository,
			IGenericRepository<Role> roleRepository,
			IGenericRepository<Order> orderRepository)
		{
			_orderRepository = orderRepository;
			_userRepository = userRepository;
			_roleRepository = roleRepository;
		}

		public async Task<string> GetRoleByEmail(string email)
		{
			var user = await _userRepository.GetAsync(x => x.Email == email);
			var roles = await _roleRepository.GetAll();
			var test = from p in user
					   from role in roles
					   where p.RoleId == role.Id
					   select role.Name;
			return test.FirstOrDefault();
		}

		public async Task<User> getUserByEmail(string email)
		{
			return (await _userRepository.GetAsync(x => x.Email == email)).FirstOrDefault();
		}

		public async Task<int> GetIdByEmail(string email)
		{
			var user = (await _userRepository.GetAsync(x => x.Email == email)).FirstOrDefault();
			return user.Id;
		}

		public async Task ChangeToRegularClient(Order order)
		{
			var userId = (await _userRepository.GetAsync(x => x.Id == order.UserId)).Select(x => x.Id).FirstOrDefault();

			var users = await _userRepository.GetAll();

			var orders = await _orderRepository.GetAll();

			users.Join(orders,
			u => u.Id,
			o => o.UserId,
			(u, o) => new User
			{
				Id = u.Id,
				Email = u.Email,
				IsRegularClient = u.IsRegularClient,
				Orders = u.Orders,
				Password = u.Password,
				Role = u.Role,
				RoleId = u.RoleId
			});

			var user = users.Where(x => x.Id == userId).FirstOrDefault();

			if (user.Orders.Where(x => x.Status == OrderStatus.Succeed).Count() >= 3)
			{
				user.IsRegularClient = true;
			}

			await _userRepository.Update(userId, user);
		}

		public async Task ChangeDeposit(string email, User newUser)
		{
			var user = await getUserByEmail(email);
			user.Balance = newUser.Balance;
			await _userRepository.Update(user.Id, user);
		}
	}
}
