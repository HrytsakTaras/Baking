using Baking.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.IServices
{
	public interface IOrderService
	{
		Task<bool> Create(Order order, OrderPie orderPieParam, int pieId, string userEmail);
		Task<IEnumerable<Order>> GetOrders(string email);
		Task CancelOrder(int id);
		Task<bool> ConfirmOrder(int id);
		Task<bool> StartOrder(int id);
	}
}
