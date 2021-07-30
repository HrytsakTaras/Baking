using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.ViewModels
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Не вказаний Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Не вказаний пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
