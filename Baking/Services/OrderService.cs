using Baking.Data.Entities;
using Baking.Data.Enums;
using Baking.IRepositories;
using Baking.IServices;
using Baking.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Baking.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Baking.Services
{
	public class OrderService : IOrderService
	{
		private readonly IGenericRepository<Order> _orderRepository;
		private readonly IGenericRepository<Pie> _pieRepository;
		private readonly IGenericRepository<OrderPie> _orderPieRepository;
		private readonly IGenericRepository<User> _userRepository;
		private readonly IUserService _userService;
		private readonly IPieService _pieService;

		public OrderService(IGenericRepository<Order> orderRepository,
			IGenericRepository<Pie> pieRepository,
			IGenericRepository<OrderPie> orderPieRepository,
			IGenericRepository<User> userRepository,
			IUserService userService, 
			IPieService pieService)
		{
			_orderRepository = orderRepository;
			_pieRepository = pieRepository;
			_orderPieRepository = orderPieRepository;
			_userRepository = userRepository;
			_userService = userService;
			_pieService = pieService;
		}

		public async Task<bool> Create(Order order, OrderPie orderPieParam, int pieId, string userEmail)
		{
			var result = await _pieRepository.GetById(pieId);
			var percentFromPrice = Convert.ToDouble(result.Price) * 0.2;
			
			User user = await GetUserByEmail(userEmail);

			if (Convert.ToDouble(order.Deposit) > await _pieService.Get20PercentFromPrice(pieId) || user.IsRegularClient)
			{

				order.Status = OrderStatus.WaitingToStart;
				order.User = user;
				await _orderRepository.Create(order);
				List<OrderPie> orderPie = new List<OrderPie>();

				orderPie.Add(new OrderPie
				{
					OrderId = order.Id,
					PieId = pieId,
				});

				await _orderPieRepository.Create(orderPie.FirstOrDefault());

				user.Orders.Add(order);
				user.Balance -= order.Deposit;

				await _userRepository.Update(pieId, user);

				await _orderRepository.Update(order.Id, order);

				return true;
			}
			return false;
		}

		private async Task<User> GetUserByEmail(string userEmail)
		{
			int userId = (await _userRepository.GetAsync(x => x.Email == userEmail)).FirstOrDefault().Id;
			return await _userRepository.GetById(userId);
		}

		public async Task<IEnumerable<Order>> GetOrders(string email)
		{
			if(await _userService.GetRoleByEmail(email) == Constatns.AdminRole)
			{
				return await _orderRepository.GetAll();
			}

			var users = await _userRepository.GetAll();

			var orders = await _orderRepository.GetAll();
			
			var userId = await _userService.GetIdByEmail(email);

			var result = orders.Join(users,
				p => p.UserId,
				t => t.Id,
				(p, t) => new Order
				{
					Id = p.Id,
					Status = p.Status,
					CreationDate = p.CreationDate,
					Deposit = p.Deposit,
					UserId = p.UserId,
					ExecutionDate = p.ExecutionDate
				}).Where(x => x.UserId == userId).ToList();

			return result;
		}

		public async Task CancelOrder(int id)
		{
			
			var order = await _orderRepository.GetById(id);
			var user =  (await _userRepository.GetAsync(x => x.Id == order.UserId)).FirstOrDefault();
			if (order.Status == OrderStatus.Succeed || order.Status == OrderStatus.InProgress)
			{
				order.Status = OrderStatus.CanceledAfterStart;
				await _orderRepository.Update(id, order);

				user.Balance += order.Deposit;
				await _userRepository.Update(id, user);
				return;
			}
			order.Status = OrderStatus.Canceled;
			await _orderRepository.Update(id, order);

			user.Balance += order.Deposit;
			await _userRepository.Update(id, user);
		}

		public async Task<bool> ConfirmOrder(int id)
		{
			Order order = await _orderRepository.GetById(id);
			if(order.Status == OrderStatus.Canceled ||
				order.Status == OrderStatus.CanceledAfterStart)
			{
				return false;
			}
			order.Status = OrderStatus.Succeed;
			await _orderRepository.Update(id, order);
			await _userService.ChangeToRegularClient(order);

			return true;
		}

		public async Task<bool> StartOrder(int id)
		{
			var order = await _orderRepository.GetById(id);
			if (order.Status == OrderStatus.WaitingToStart)
			{
				order.Status = OrderStatus.InProgress;
				await _orderRepository.Update(id, order);
				return true;
			}
			return false;
		}
	}
}
