using System.ComponentModel.DataAnnotations;

namespace Baking.ViewModels
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Заповніть поле Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Введіть пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароль введений невірно")]
		public string ConfirmPassword { get; set; }
	}
}
