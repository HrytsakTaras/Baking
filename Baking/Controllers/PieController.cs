using Baking.Interfaces;
using Baking.Models;
using Baking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Controllers
{
	public class PieController : Controller
	{
		private readonly IPieRepository _pieRepository;

		public PieController(IPieRepository pieRepository)
		{
			_pieRepository = pieRepository;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _pieRepository.GetAll());
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var pie = await _pieRepository.GetById(id);
			if (pie == null)
			{
				return NotFound();
			}

			return View(pie);
		}

		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(PieViewModel pieViewModel)
		{
			await _pieRepository.Create(pieViewModel);
			return RedirectToAction(nameof(Index));

		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var pie = await _pieRepository.GetById(id);

			if (pie == null)
			{
				return NotFound();
			}
			return View(pie);
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
					await _pieRepository.Update(id, pie);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_pieRepository.PieExists(pie.Id))
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

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var pie = await _pieRepository.GetById(id);
			if (pie == null)
			{
				return NotFound();
			}

			return View(pie);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _pieRepository.Delete(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
