using Baking.Data.Entity;
using Baking.Data.Enums;
using Baking.IRepositories;
using Baking.IServices;
using Baking.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Services
{
	public class OrderService : IOrderService
	{
		private readonly IGenericRepository<Order> _orderRepository;
		private readonly IGenericRepository<Pie> _pieRepository;
		private readonly IGenericRepository<OrderPie> _orderPieRepository;
		private readonly IGenericRepository<User> _userRepository;
		private readonly IUserService _userService;

		public OrderService(IGenericRepository<Order> orderRepository,
			IGenericRepository<Pie> pieRepository,
			IGenericRepository<OrderPie> orderPieRepository,
			IGenericRepository<User> userRepository,
			IUserService userService)
		{
			_orderRepository = orderRepository;
			_pieRepository = pieRepository;
			_orderPieRepository = orderPieRepository;
			_userRepository = userRepository;
			_userService = userService;
		}

		public async Task Create(Order order, OrderPie orderPieParam, int pieId, string userEmail)
		{
			User user = await GetUserByEmail(userEmail);

			order.Status = OrderStatus.InProgress;
			order.User = user;
			await _orderRepository.Create(order);
			List<OrderPie> orderPie = new List<OrderPie>();

			orderPie.Add(new OrderPie
			{
				OrderId = order.Id,
				PieId = pieId,
				ExecutionDate = orderPieParam.ExecutionDate
			});
			await _orderPieRepository.Create(orderPie.FirstOrDefault());

			user.Orders.Add(order);

			order.OrderPies = orderPie;

			await _userRepository.Update(pieId, user);

			await _orderRepository.Update(order.Id, order);
		}

		private async Task<User> GetUserByEmail(string userEmail)
		{
			int userId = (await _userRepository.GetAsync(x => x.Email == userEmail)).FirstOrDefault().Id;
			return await _userRepository.GetById(userId);
		}

		public async Task<IEnumerable<Order>> GetOrders(string email)
		{
			if(await _userService.GetRoleByEmail(email) == "admin")
			{
				return await _orderRepository.GetAll();
			}

			var users = await _userRepository.GetAll();

			var orders = await _orderRepository.GetAll();

			var userId = (await GetUserByEmail(email)).Id;

			var result = orders.Join(users,
				p => p.UserId,
				t => t.Id,
				(p, t) => new Order
				{
					Status = p.Status,
					CreationDate = p.CreationDate,
					Deposit = p.Deposit,
					UserId = p.UserId
				}).Where(x => x.UserId == userId).ToList();

			return result;
		}
	}
}
