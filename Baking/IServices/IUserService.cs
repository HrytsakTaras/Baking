using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.IServices
{
	public interface IUserService
	{
		Task<string> GetRoleByEmail(string email);
	}
}
