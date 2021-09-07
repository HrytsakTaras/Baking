using Baking.Features.Account;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baking.Validations
{
	public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
	{
		public AddAccountCommandValidator()
		{
			RuleFor(x => x.Email).NotEmpty();
			RuleFor(x => x.Password).NotEmpty();
		}
	}
}
