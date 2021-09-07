using Baking.Data.Entities;
using Baking.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Baking.Features
{
	public class AddPieCommand : IRequest<Pie>
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public byte[] Image { get; set; }
		public IFormFile file { get; set; }
		public string Description { get; set; }

		public class AddPieCommandHandler : IRequestHandler<AddPieCommand, Pie>
		{
			private readonly IGenericRepository<Pie> _pieRepository;
			private readonly IWebHostEnvironment _webHostEnviroment;

			public AddPieCommandHandler(IGenericRepository<Pie> pieRepository, IWebHostEnvironment webHostEnvironmen)
			{
				_pieRepository = pieRepository;
				_webHostEnviroment = webHostEnvironmen;
			}

			public async Task<Pie> Handle(AddPieCommand command, CancellationToken cancellationToken)
			{
				string stringFileName = UploadFile(command.file);
				Pie pie = new Pie
				{
					Name = command.Name,
					Price = command.Price,
					Image = stringFileName,
					Description = command.Description
				};
			await _pieRepository.Create(pie);

				return pie;
			}

			private string UploadFile(IFormFile pieViewModel)
			{
				string fileName;

				string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "images");
				fileName = Guid.NewGuid().ToString() + "-" + pieViewModel.FileName;
				string filePath = Path.Combine(uploadDir, fileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					pieViewModel.CopyTo(fileStream);
				}
				return fileName;
			}
		}
	}


}
