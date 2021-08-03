using Baking.Data.Entity;
using Baking.IRepositories;
using Baking.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Services
{
	public class UserService : IUserService
	{
		private readonly IGenericRepository<User> _userRepository;
		private readonly IGenericRepository<Role> _roleRepository;
		public UserService(IGenericRepository<User> userRepository,
			IGenericRepository<Role> roleRepository)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
		}

		public async Task<string> GetRoleByEmail(string email)
		{
			var user = await _userRepository.GetAsync(x => x.Email == email);
			var roles = await _roleRepository.GetAll();
			var test = from p in user
					   from role in roles
					   where p.RoleId == role.Id
					   select role.Name;
			return test.FirstOrDefault();
		}
	}
}
