using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModel;

namespace RunGroupWebApp.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _signInManager;
        public readonly ApplicationDbContext _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public ActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel LoginViewModel)
        {
            if (!ModelState.IsValid) return View(LoginViewModel);
            var user = await _userManager.FindByEmailAsync(LoginViewModel.Email);
            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user,LoginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, LoginViewModel.Password,false,false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index","Race");
                    }
                }
            }
            TempData["Error"]="wrong credentials, please rty again.";
            return View(LoginViewModel);
        }

        public ActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid) return View(registerViewModel);
            var checkUser = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if(checkUser != null)
            {
                ModelState.AddModelError("Email", "This email already exists.");
                return View(registerViewModel);
            }
            var newUser = new AppUser()
            {
                Email= registerViewModel.Email,
                UserName = registerViewModel.Email
            };
            var newResponse  = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (newResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return RedirectToAction("Index", "Race");
            }
            foreach (var error in newResponse.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(registerViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Race");
        }
    }
}
