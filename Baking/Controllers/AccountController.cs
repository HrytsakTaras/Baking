using Baking.Data.Entity;
using Baking.IServices;
using Baking.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data;
using Baking.Data;

namespace Baking.Controllers
{
	public class AccountController : Controller
	{

		private readonly IAccountService _accountService;
		private readonly IUserService _userService;

		public AccountController(IAccountService accountService,
			IUserService userService)
		{
			_accountService = accountService;
			_userService = userService;
		}

		[Authorize(Roles = Constatns.AdminRole)]
		public async Task<IActionResult> Index()
		{
			return View(await _accountService.GetAll());
		}

		[HttpGet]
		public IActionResult Register()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Pie");
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				User user = await _accountService.GetUserAsync(model.Email);
				if (user == null)
				{
					user = await _accountService.Create(model);

					await Authenticate(user);

					return RedirectToAction("Index", "Account");
				}
				else
					ModelState.AddModelError("", "Invalid login or password");
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Pie");
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{

				User user = _accountService.Include(model);

				if (user != null)
				{
					await Authenticate(user);

					return RedirectToAction("Index", "Account");
				}
				ModelState.AddModelError("", "Invalid login or password");
			}
			return View(model);
		}

		private async Task Authenticate(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
			};

			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
		}

		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Account");
		}

		[HttpGet, ActionName("Edit")]
		public async Task<IActionResult> EditProfile()
		{
			var email = User.Identity.Name;
			var user = await _userService.getUserByEmail(email);
			return View(user);
		}

		[HttpPost, ActionName("Edit")]
		public async Task<IActionResult> EditProfile(User newUser)
		{
			var email = User.Identity.Name;
			await _userService.ChangeDeposit(email, newUser);
			return RedirectToAction("Index", "Pie");
		}
	}
}

