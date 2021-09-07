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
	public class AddAccountCommand : IRequest<User>
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public bool IsRegularClient { get; set; }
		public decimal Balance { get; set; }

		public List<Order> Orders { get; set; } = new List<Order>();

		public int RoleId { get; set; }
		public Role Role { get; set; }

		public class AddPieCommandHandler : IRequestHandler<AddAccountCommand, User>
		{
			private readonly IGenericRepository<User> _accountRepository;
			private readonly IGenericRepository<Role> _roleRepository;

			public AddPieCommandHandler(IGenericRepository<User> accountRepository,
				IGenericRepository<Role> roleRepository)
			{
				_accountRepository = accountRepository;
				_roleRepository = roleRepository;
			}

			public async Task<User> Handle(AddAccountCommand command, CancellationToken cancellationToken)
			{
				var user = await GetUserAsync(command.Email);
				if (user == null)
				{
					user = new User
					{
						Email = command.Email,
						Password = command.Password,
						Balance = command.Balance
					};

					Role userRole = await GetRoleAsync("user");
					if (userRole != null)
						user.Role = userRole;

					await _accountRepository.Create(user);
				}
				else
				{
					return null;
				}
				return user;
			}

			private async Task<Role> GetRoleAsync(string name)
			{
				return (await _roleRepository.GetAsync(x => x.Name == name)).FirstOrDefault();
			}

			private async Task<User> GetUserAsync(string email)
			{
				return (await _accountRepository.GetAsync(x => x.Email == email)).FirstOrDefault();
			}
		}
	}
}
