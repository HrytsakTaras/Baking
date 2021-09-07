using Baking.Data.Entities;
using Baking.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Baking.Features.Account
{
	public class GetUserQuery : IRequest<User>
	{
		public string Email { get; set; }
		public string Password { get; set; }

		public class GetUserByIdHandler : IRequestHandler<GetUserQuery, User>
		{
			private readonly IGenericRepository<User> _accountRepository;

			public GetUserByIdHandler(IGenericRepository<User> accountRepository)
			{
				_accountRepository = accountRepository;
			}

			public async Task<User> Handle(GetUserQuery query, CancellationToken cancellationToken)
			{
				var user = _accountRepository.Include(x => x.Role)
				.FirstOrDefault(u => u.Email == query.Email && u.Password == query.Password);
				return user;
			}
		}
	}
}
