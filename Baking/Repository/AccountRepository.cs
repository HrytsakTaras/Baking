using Baking.Interfaces;
using Baking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Repository
{
	public class AccountRepository : IAccountRepository
	{
		private readonly ApplicationContext _context;
		public AccountRepository(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<List<User>> GetAll()
		{
			var result = await _context.Users.ToListAsync();
			return result;
		}
	}
}
