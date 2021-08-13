using Baking.Data.Entities;
using Baking.IServices;
using Baking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Baking.Data;
using MediatR;
using Baking.Features;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace Baking.Controllers
{
	public class PieController : Controller
	{
		private readonly IMediator _mediator;

		public PieController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _mediator.Send(new GetAllPieQuery()));
		}


		[Authorize(Roles = Constatns.AdminRole)]
		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = Constatns.AdminRole)]
		public async Task<IActionResult> Create(AddPieCommand client)
		{
			Pie pie = await _mediator.Send(client);
			return RedirectToAction(nameof(Index));

		}

		[Authorize(Roles = Constatns.AdminRole)]
		public async Task<IActionResult> Edit(int id)
		{
			var pie = await _mediator.Send(new GetPieByIdQuery { Id = id });
			return pie != null ? View(pie) : NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = Constatns.AdminRole)]
		public async Task<IActionResult> Edit(int id, UpdatePieByIdCommand command)
		{
			if (id != command.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					await _mediator.Send(command);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (await _mediator.Send(new GetPieByIdQuery { Id = id }) is null)
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
			return View(command);
		}

		[Authorize(Roles = Constatns.AdminRole)]
		public async Task<IActionResult> Delete(int id)
		{
			var pie = await _mediator.Send(new GetPieByIdQuery { Id = id });
			return pie != null ? View(pie) : NotFound();
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = Constatns.AdminRole)]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var pie = await _mediator.Send(new DeletePieByIdCommand { Id = id });
			return RedirectToAction(nameof(Index));
		}
	}
}
