using System.ComponentModel.DataAnnotations;

namespace Baking.ViewModels
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Enter email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Enter password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password is incorrect")]
		public string ConfirmPassword { get; set; }

		[Required]
		public decimal Balance { get; set; }
	}
}
