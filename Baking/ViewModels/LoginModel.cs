using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.ViewModels
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Email address not specified")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password not specified")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
