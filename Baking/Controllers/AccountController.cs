using Baking.Interfaces;
using Baking.IRepositories;
using Baking.Models;
using Baking.Repository;
using Baking.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Baking.Controllers
{
	public class AccountController : Controller
	{
		private readonly ApplicationContext _context;
		private readonly IAccountRepository _accountRepository;
		private readonly IGenericRepository<User> _userRepository;
		private readonly IGenericRepository<Role> _roleRepository;

		public AccountController(ApplicationContext context, IAccountRepository accountRepository, IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository)
		{
			_context = context;
			_accountRepository = accountRepository;
			_userRepository = userRepository;
			_roleRepository = roleRepository;
		}

		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Index()
		{
			var users = await _userRepository.GetAll();
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
				User user = (await _userRepository.GetAsync(x => x.Email==model.Email)).FirstOrDefault();
				if (user == null)
				{
					user = new User { Email = model.Email, Password = model.Password };
					Role userRole = (await _roleRepository.GetAsync(x => x.Name == "user")).FirstOrDefault();
					if (userRole != null)
						user.Role = userRole;

					await _userRepository.Create(user);

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
				
				User user = _userRepository.Include(x => x.Role).FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

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

