using Baking.Interfaces;
using Baking.Models;
using Baking.Repository;
using Baking.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Baking.Controllers
{
	public class AccountController : Controller
	{
		private readonly ApplicationContext _context;
		private readonly IAccountRepository _accountRepository;

		public AccountController(ApplicationContext context, IAccountRepository accountRepository)
		{
			_context = context;
			_accountRepository = accountRepository;
		}

		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Index()
		{
			var users = await _accountRepository.GetAll();
			return View(users);
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
				User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
				if (user == null)
				{
					user = new User { Email = model.Email, Password = model.Password };
					Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
					if (userRole != null)
						user.Role = userRole;

					_context.Users.Add(user);
					await _context.SaveChangesAsync();

					await Authenticate(user);

					return RedirectToAction("Index", "Account");
				}
				else
					ModelState.AddModelError("", "Невірний логін або пароль");
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
				User user = await _context.Users
					.Include(u => u.Role)
					.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
				if (user != null)
				{
					await Authenticate(user);

					return RedirectToAction("Index", "Account");
				}
				ModelState.AddModelError("", "Невірний логін або пароль");
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
	}
}

