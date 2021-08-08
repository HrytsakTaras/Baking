using Baking.Data.Entity;
using Baking.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baking.IServices
{
	public interface IPieService
	{
		Task<IEnumerable<Pie>> GetAll();
		Task<Pie> GetById(int id);
		Task Create(PieViewModel pieViewModel);
		Task Update(int id, Pie pie);
		Task Delete(Pie pie);
		Task<double> Get20PercentFromPrice(int pieId);
	}
}
