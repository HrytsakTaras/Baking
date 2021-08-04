using Baking.Data.Entity;
using System.Threading.Tasks;

namespace Baking.IServices
{
	public interface IUserService
	{
		Task<string> GetRoleByEmail(string email);
		Task<int> GetIdByEmail(string email);
		Task ChangeToRegularClient(Order order);
	}
}
