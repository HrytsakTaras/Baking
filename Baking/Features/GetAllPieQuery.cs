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
	public class GetAllPieQuery : IRequest<IEnumerable<Pie>>
	{
		public class GetAllPieQueryHandler : IRequestHandler<GetAllPieQuery, IEnumerable<Pie>>
		{
			private readonly IGenericRepository<Pie> _pieRepository;

			public GetAllPieQueryHandler(IGenericRepository<Pie> pieRepository)
			{
				_pieRepository = pieRepository;
			}

			public async Task<IEnumerable<Pie>> Handle(GetAllPieQuery query, CancellationToken cancellationToken)
			{
				var pies = await _pieRepository.GetAll();
				return pies;
			}
		}
	}


}
