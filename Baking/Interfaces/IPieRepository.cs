using Baking.Models;
using Baking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Interfaces
{
	public interface IPieRepository
	{
		Task<List<Pie>> GetAll();
		Task<Pie> GetById(int? id);
		Task Create(PieViewModel pieViewModel);
		Task Update(int id, Pie pie);
		Task Delete(int id);
		bool PieExists(int id);
	}
}
