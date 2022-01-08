using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WEB_voorbereiding.Data;
using WEB_voorbereiding.Models;
using WEB_voorbereiding.Tools;
using WEB_voorbereiding.ViewModels;

namespace WEB_voorbereiding.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        UserManager<CustomIdentityUser> _userManager;
        SignInManager<CustomIdentityUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _context;

        public AccountController(UserManager<CustomIdentityUser> userManager,
            SignInManager<CustomIdentityUser> signInManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }

        #region register

        // Deze actie laadt de registratiepagina
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Deze actie is de POSTrequest
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (await UserExistAsync(user))
                {
                    ModelState.AddModelError("", "Email adres is al geregistreerd!");
                    return View(user);
                }

                if (!user.Wachtwoord.Equals(user.Herhaalwachtwoord))
                {
                    ModelState.AddModelError("", "Wachtwoord moet identiek zijn!");
                    return View(user);
                }
                else
                {
                    RegisterResultViewModel result = await RegisterUserAsync(user);
                    if (result.Succeeded)
                    {
                        // Na registratie sturen we de gebruiker door naar het loginscherm
                        return View("Login");
                    }
                    else
                    {
                        foreach (string error in result.Errors)
                            ModelState.AddModelError("", error);
                        return View(user);
                    }

                }
            }
            return View(user);

        }

        // De "backend" van het registratieproces, wordt opgeropen in de Postrequest
        private async Task<RegisterResultViewModel> RegisterUserAsync(RegisterViewModel user)
        {
            var registerResult = new RegisterResultViewModel();
            Gebruiker g = new Gebruiker()
            {
                Naam = user.Naam,
                Voornaam = user.Voornaam,
                Email = user.Email,
                Functie = Roles.Student
            };
            var identityUser = new CustomIdentityUser
            {
                UserName = user.Email,
                Email = user.Email,
                Gebruiker = g
            };
            _context.Add(g);
            _context.SaveChanges();
            var result = await _userManager.CreateAsync(identityUser, user.Wachtwoord);
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync(g.Functie) ;
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(identityUser, role.Name);
                    
                    registerResult.Succeeded = true;
                }
            }
            else
            {
                if (result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        registerResult.Errors.Add(error.Description);
                    }
                }
                else
                {
                    registerResult.Errors.Add("Er is een probleem om de gebruiker te registreren!");
                }
            }
            return registerResult;
        }

        private async Task<bool> UserExistAsync(RegisterViewModel user)
        {
            bool userExist = false; // FindByUsername bestaat ook.
            var result = await _userManager.FindByEmailAsync(user.Email);
            if (result != null)
                userExist = true;
            return userExist;
        }
        #endregion register
        #region login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager
                    .PasswordSignInAsync(user.Email, user.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    CustomIdentityUser identityUser = await _userManager.FindByEmailAsync(user.Email);
                    if (identityUser != null)
                    {
                        await _signInManager.SignInAsync(identityUser, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Foutieve email of wachtwoord.");
                    }
                }

                else
                {
                    ModelState.AddModelError("", "Foutieve email of wachtwoord.");
                }
            }
            return View(user);


        }
        #endregion
        #region Logout
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
        #endregion Logout
        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
