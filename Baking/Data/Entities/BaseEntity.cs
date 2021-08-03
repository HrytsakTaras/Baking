using System.ComponentModel.DataAnnotations;

namespace Baking.Data.Entity
{
	public class BaseEntity
	{
		[Key]
		public int Id { get; set; }
	}
}
