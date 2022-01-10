using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebHelper.Data;
using WebHelper.Models;
using WebHelper.Tools;
using WebHelper.ViewModels;

namespace WebHelper.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Register, Login, Logout, Access denied
        /// Deze controller kan bijna volledig gekopieerd worden, 
        /// registreren en inloggen is bij ieder project hetzelfde
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Deze actie laadt de registratiepagina
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Deze actie is de POSTrequest
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <summary>
        /// De "backend" van het registratieproces, wordt opgeropen in de Postrequest
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
            CustomIdentityUser identityUser = new CustomIdentityUser
            {
                UserName = user.Email,
                Email = user.Email,
                Gebruiker = g
            };
            Student s = new Student { Gebruiker = g };
            _context.Add(g);
            _context.Students.Add(s);
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

        /// <summary>
        /// Gebruiker bestaat al
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
