using Baking.Data.Entity;
using Baking.IRepositories;
using Baking.IServices;
using Baking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
			ViewBag.ErrorMessage = TempData["Message"];
			return View(await _orderService.GetOrders(User.Identity.Name));
		}

		[Authorize]
		[Route("Order/Index/{date:DateTime}")]
		public async Task<IActionResult> Index(DateTime date)
		{
			var result = (await _orderService.GetOrders(User.Identity.Name)).Where(x => x.ExecutionDate.Date == date);
			var resultwithwhere = result.Where(x => x.ExecutionDate.Date == date);
			return View(resultwithwhere);
		}

		[Route("Order/CancelOrder/{id:int}")]
		public async Task<IActionResult> CancelOrder(int id)
		{
			await _orderService.CancelOrder(id);
			return RedirectToAction("Index");
		}

		[Route("Order/ConfirmOrder/{id:int}")]
		public async Task<IActionResult> ConfirmOrder(int id)
		{
			var status = await _orderService.ConfirmOrder(id);
			if (!status)
			{
				TempData["Message"] = "You can't change this status.";
			}
			return RedirectToAction("Index");
		}

		[Route("Order/StartOrder/{id:int}")]
		public async Task<IActionResult> StartOrder(int id)
		{
			var status = await _orderService.StartOrder(id);
			if (!status)
			{
				TempData["Message"] = "You can't change this status.";
			}
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Details(int id)
		{
			var order = await _orderRepository.GetById(id);
			var orderPie = (await _orderPieRepository.GetAsync(x => x.OrderId == id)).FirstOrDefault();
			var pie = (await _pieRepository.GetAsync(x => x.Id == orderPie.PieId)).FirstOrDefault();

			var result = new OrderViewModel
			{
				Deposit = order.Deposit,
				CreationDate = order.CreationDate,
				ExecutionDate = order.ExecutionDate,
				Status = order.Status,
				Name = pie.Name,
				Price = pie.Price
			};

			return View(result);
		}

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
	}
}
