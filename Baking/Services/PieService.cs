using Baking.Data.Entities;
using Baking.IRepositories;
using Baking.IServices;
using Baking.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Baking.Services
{
	public class PieService : IPieService
	{
		private readonly IWebHostEnvironment _webHostEnviroment;
		private readonly IGenericRepository<Pie> _pieRepository;

		public PieService(IWebHostEnvironment webHostEnvironment,
			IGenericRepository<Pie> pieRepository)
		{
			_webHostEnviroment = webHostEnvironment;
			_pieRepository = pieRepository;
		}

		public Pie CreateNew(PieViewModel pieViewModel)
		{
			//string stringFileName = UploadFile(pieViewModel);
			var pie = new Pie
			{
				Name = pieViewModel.Name,
				Price = pieViewModel.Price,
				OrderPies = pieViewModel.OrderPies,
				//Image = stringFileName
			};
			return pie;
		}

		/*private string UploadFile(PieViewModel pieViewModel)
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
		}*/

		public async Task<IEnumerable<Pie>> GetAll()
		{
			return await _pieRepository.GetAll();
		}

		public Task<Pie> GetById(int id)
		{
			return _pieRepository.GetById(id);
		}

		public async Task Create(PieViewModel pieViewModel)
		{
			await _pieRepository.Create(CreateNew(pieViewModel));
		}

		public async Task Update(int id, Pie pie)
		{
			await _pieRepository.Update(id, pie);
		}

		public async Task Delete(Pie pie)
		{
			await _pieRepository.Delete(pie);
		}

		public async Task<double> Get20PercentFromPrice(int pieId)
		{
			var pie = await _pieRepository.GetById(pieId);
			return Convert.ToDouble(pie.Price) * 0.2;
		}
	}
}
