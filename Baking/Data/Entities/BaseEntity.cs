using System.ComponentModel.DataAnnotations;

namespace Baking.Data.Entities
{
	public class BaseEntity
	{
		[Key]
		public int Id { get; set; }
	}
}
