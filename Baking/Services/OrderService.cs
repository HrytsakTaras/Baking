using Baking.Data.Entity;
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

		public OrderService(IGenericRepository<Order> orderRepository,
			IGenericRepository<Pie> pieRepository,
			IGenericRepository<OrderPie> orderPieRepository,
			IGenericRepository<User> userRepository)
		{
			_orderRepository = orderRepository;
			_pieRepository = pieRepository;
			_orderPieRepository = orderPieRepository;
			_userRepository = userRepository;
		}

		public async Task Create(Order order, OrderPie orderPieParam, int pieId, string userEmail)
		{
			order.Status = Statuses.InProgress;
			await _orderRepository.Create(order);
			List<OrderPie> orderPie = new List<OrderPie>();

			orderPie.Add(new OrderPie
			{
				OrderId = order.Id,
				PieId = pieId,
				ExecutionDate = orderPieParam.ExecutionDate
			});
			await _orderPieRepository.Create(orderPie.FirstOrDefault());

			User user = await GetUserByEmail(userEmail);


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
	}
}
