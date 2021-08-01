using Baking.Data.Entity;
using Baking.IServices;
using Baking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Baking.Controllers
{
	public class PieController : Controller
	{
		private readonly IPieService _pieService;

		public PieController(IPieService pieService)
		{
			_pieService = pieService;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _pieService.GetAll());
		}

		public async Task<IActionResult> Details(int id)
		{
			var pie = await _pieService.GetById(id);

			return pie != null ? View(pie) : NotFound();
		}

		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(PieViewModel pieViewModel)
		{
			await _pieService.Create(pieViewModel);
			return RedirectToAction(nameof(Index));

		}

		public async Task<IActionResult> Edit(int id)
		{
			var pie = await _pieService.GetById(id);
			return pie != null ? View(pie) : NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Pie pie)
		{
			if (id != pie.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					await _pieService.Update(id, pie);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (await _pieService.GetById(pie.Id) is null)
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
			return View(pie);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var pie = await _pieService.GetById(id);

			return pie != null ? View(pie) : NotFound();
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var result = await _pieService.GetById(id);
			await _pieService.Delete(result);
			return RedirectToAction(nameof(Index));
		}
	}
}
