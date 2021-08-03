using Baking.Data.Entity;
using Baking.IRepositories;
using Baking.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Baking.Controllers
{
	public class OrderController : Controller
	{
		private readonly IGenericRepository<Order> _orderRepository;
		private readonly IGenericRepository<Pie> _pieRepository;
		private readonly IGenericRepository<OrderPie> _orderPieRepository;
		private readonly IGenericRepository<User> _userRepository;
		private readonly IOrderService _orderService;

		public OrderController(IGenericRepository<Order> orderRepository,
			IGenericRepository<Pie> pieRepository,
			IGenericRepository<OrderPie> orderPieRepository,
			IGenericRepository<User> userRepository,
			IOrderService orderService)
		{
			_orderRepository = orderRepository;
			_pieRepository = pieRepository;
			_orderPieRepository = orderPieRepository;
			_userRepository = userRepository;
			_orderService = orderService;
		}

		[Authorize]
		public async Task<IActionResult> Index()
		{
			return View(await _orderService.GetOrders(User.Identity.Name));
		}


		/*[Authorize(Roles ="user")]
		[HttpPost, ActionName("Index")]
		public async Task<IActionResult> IndexForUser()
		{
			var users = await _userRepository.GetAll();
			
			var orders = await _orderRepository.GetAll();
			var userEmail = HttpContext.User.Identity.Name;
			int userId = (await _userRepository.GetAsync(x => x.Email == userEmail)).FirstOrDefault().Id;

			var result = orders.Join(users,
				p => p.UserId,
				t => t.Id,
				(p, t) => new Order
				{
					Status = p.Status,
					CreationDate = p.CreationDate,
					Deposit = p.Deposit,
					UserId = p.UserId
				}).Where(x=>x.UserId==userId).ToList();

			return View(result);
		}*/

		/*public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var order = await _context.Orders
				.FirstOrDefaultAsync(m => m.Id == id);
			if (order == null)
			{
				return NotFound();
			}

			return View(order);
		}*/

		[Authorize]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Order/Create/{pieId}")]
		public async Task<IActionResult> Create(Order order, OrderPie orderPieParam, int pieId)
		{
			var userEmail = HttpContext.User.Identity.Name;
			await _orderService.Create(order, orderPieParam, pieId, userEmail);

			return RedirectToAction(nameof(Index));
		}

		/*public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var order = await _context.Orders.FindAsync(id);
			if (order == null)
			{
				return NotFound();
			}
			return View(order);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Status,CreationDate,Deposit,Id")] Order order)
		{
			if (id != order.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(order);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!OrderExists(order.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(order);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var order = await _context.Orders
				.FirstOrDefaultAsync(m => m.Id == id);
			if (order == null)
			{
				return NotFound();
			}

			return View(order);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var order = await _context.Orders.FindAsync(id);
			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool OrderExists(int id)
		{
			return _context.Orders.Any(e => e.Id == id);
		}
	}*/
	}
}
