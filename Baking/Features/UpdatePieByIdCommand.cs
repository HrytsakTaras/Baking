using Baking.Data.Entities;
using Baking.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Baking.Features
{
	public class UpdatePieByIdCommand : IRequest<Pie>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public byte[] Image { get; set; }
		public string Description { get; set; }

		public class UpdatePieByIdCommandHandle : IRequestHandler<UpdatePieByIdCommand, Pie>
		{
			private readonly IGenericRepository<Pie> _pieRepository;

			public UpdatePieByIdCommandHandle(IGenericRepository<Pie> pieRepository)
			{
				_pieRepository = pieRepository;
			}

			public async Task<Pie> Handle(UpdatePieByIdCommand command, CancellationToken cancellationToken)
			{
				var pie = new Pie
				{
					Id = command.Id,
					Name = command.Name,
					Price = command.Price,
					//Image = command.Image,
					Description = command.Description
				};
				await _pieRepository.Update(command.Id, pie);
				return pie;
			}
		}
	}
}
