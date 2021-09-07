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
	public class GetPieByIdQuery : IRequest<Pie>
	{
		public int Id { get; set; }

		public class GetPieByIdHandler : IRequestHandler<GetPieByIdQuery, Pie>
		{
			private readonly IGenericRepository<Pie> _pieRepository;

			public GetPieByIdHandler(IGenericRepository<Pie> pieRepository)
			{
				_pieRepository = pieRepository;
			}

			public async Task<Pie> Handle(GetPieByIdQuery query, CancellationToken cancellationToken)
			{
				var pie = (await _pieRepository.GetAsync(x => x.Id == query.Id)).FirstOrDefault();
				return pie;
			}
		}
	}

	

}
