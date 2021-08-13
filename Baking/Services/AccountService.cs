using Baking.Data.Entities;
using Baking.IRepositories;
using Baking.IServices;
using Baking.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Services
{
	public class AccountService : IAccountService
	{
		private readonly IGenericRepository<User> _userRepository;
		private readonly IGenericRepository<Role> _roleRepository;
		public AccountService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
		}

		public async Task<IEnumerable<User>> GetAll()
		{
			return await _userRepository.GetAll();
		}

		public async Task<User> GetUserAsync(string email)
		{
			return (await _userRepository.GetAsync(x => x.Email == email)).FirstOrDefault();
		}

		public async Task<Role> GetRoleAsync(string name)
		{
			return (await _roleRepository.GetAsync(x => x.Name == name)).FirstOrDefault();
		}

		public async Task<User> Create(RegisterModel model)
		{
			User user = new User {
				Email = model.Email,
				Password = model.Password,
				Balance = model.Balance
			};
			Role userRole = await GetRoleAsync("user");
			if (userRole != null)
				user.Role = userRole;

			await _userRepository.Create(user);
			return user;
		}

		public User Include(LoginModel model)
		{
			return _userRepository.Include(x => x.Role).FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
		}

	}
}
