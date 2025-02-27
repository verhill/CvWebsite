using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CV_Website.Models;
using System.Threading.Tasks;
using CV_Website.ViewModels;

namespace CV_Website.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

       
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.UserName, // ta ej bort även om det ser ut som dubble lagring för då kan vi inte logga in
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Name = model.Name,
                    Private = model.Private
                };

                
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    
                    return RedirectToAction("Index", "Home");
                }

                
                
            }

            return View(model);
        }

        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.FindByEmailAsync(model.UserName);

                if (user != null)
                {
                   
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        
                        return RedirectToAction("Index", "Home");
                    }

                    
                    ModelState.AddModelError(string.Empty, "Fel lösen eller mail.");
                }
                else
                {
                    
                    ModelState.AddModelError(string.Empty, "Användaren finns ej.");
                }
            }

          
            return View(model);
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
