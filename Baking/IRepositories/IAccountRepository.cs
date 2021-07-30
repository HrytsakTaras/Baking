using Baking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Interfaces
{
	public interface IAccountRepository
	{
		Task<List<User>> GetAll();
	}
}
