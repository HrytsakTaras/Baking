using Baking.Data.Entities;
using System.Threading.Tasks;

namespace Baking.IServices
{
	public interface IUserService
	{
		Task<string> GetRoleByEmail(string email);
		Task<User> getUserByEmail(string email);
		Task<int> GetIdByEmail(string email);
		Task ChangeToRegularClient(Order order);
		Task ChangeDeposit(string email, User newUser);
	}
}
