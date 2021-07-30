using Baking.Interfaces;
using Baking.Models;
using Baking.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Repository
{
	public class PieRepository : IPieRepository
	{
		private readonly ApplicationContext _context;
		private readonly IWebHostEnvironment _webHostEnviroment;

		public PieRepository(IWebHostEnvironment webHostEnvironment, ApplicationContext context)
		{
			_context = context;
			_webHostEnviroment = webHostEnvironment;
		}

		public async Task Create(PieViewModel pieViewModel)
		{
			string stringFileName = UploadFile(pieViewModel);
			var pie = new Pie
			{
				Name = pieViewModel.Name,
				Price = pieViewModel.Price,
				OrderPies = pieViewModel.OrderPies,
				Image = stringFileName
			};

			_context.Add(pie);
			await _context.SaveChangesAsync();
		}

		private string UploadFile(PieViewModel pieViewModel)
		{
			string fileName = null;
			if (pieViewModel.Image != null)
			{
				string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "images");
				fileName = Guid.NewGuid().ToString() + "-" + pieViewModel.Image.FileName;
				string filePath = Path.Combine(uploadDir, fileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					pieViewModel.Image.CopyTo(fileStream);
				}
			}
			return fileName;
		}

		public async Task Delete(int id)
		{
			var pie = await _context.Pies.FindAsync(id);
			_context.Pies.Remove(pie);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Pie>> GetAll()
		{
			var result =  await _context.Pies.ToListAsync();
			return result;
		}

		public async Task<Pie> GetById(int? id)
		{
			var result = await _context.Pies
				.FirstOrDefaultAsync(m => m.Id == id);
			return result;
		}

		public async Task Update(int id, Pie pie)
		{
			_context.Update(pie);
			await _context.SaveChangesAsync();
		}

		public bool PieExists(int id)
		{
			return _context.Pies.Any(e => e.Id == id);
		}
	}
}
