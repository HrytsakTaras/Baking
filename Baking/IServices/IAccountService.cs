using Baking.Data.Entities;
using Baking.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baking.IServices
{
	public interface IAccountService
	{
		Task<IEnumerable<User>> GetAll();
		Task<User> GetUserAsync(string email);
		Task<Role> GetRoleAsync(string name);
		Task<User> Create(RegisterModel model);
		User Include(LoginModel model);
	}
}
