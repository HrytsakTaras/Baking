using Baking.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.IServices
{
	public interface IOrderService
	{
		Task Create(Order order, OrderPie orderPieParam, int pieId, string userEmail);
		Task<IEnumerable<Order>> GetOrders(string email);
		Task<bool> CancelOrder(int id);
		Task<bool> ConfirmOrder(int id);
	}
}
