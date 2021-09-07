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
	public class DeletePieByIdCommand : IRequest<Pie>
	{
		public int Id { get; set; }

		public class DeletePieByIdCommandHandler : IRequestHandler<DeletePieByIdCommand, Pie>
		{
			private readonly IGenericRepository<Pie> _pieRepository;

			public DeletePieByIdCommandHandler(IGenericRepository<Pie> pieRepository)
			{
				_pieRepository = pieRepository;
			}

			public async Task<Pie> Handle(DeletePieByIdCommand command, CancellationToken cancellationToken)
			{
				var pie = (await _pieRepository.GetAsync(x => x.Id == command.Id)).FirstOrDefault();
				await _pieRepository.Delete(pie);
				return pie;
			}
		}
	}
}
