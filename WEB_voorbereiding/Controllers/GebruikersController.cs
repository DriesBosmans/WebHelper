using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_voorbereiding.Data;
using WEB_voorbereiding.Models;
using WEB_voorbereiding.Tools;
using WEB_voorbereiding.ViewModels;

namespace WEB_voorbereiding.Controllers
{
    public class GebruikersController : Controller
    {
        UserManager<CustomIdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _context;
        GebruikersRepo _repo;


        public GebruikersController(UserManager<CustomIdentityUser> userManager,
           
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
           
            _context = context;
            _roleManager = roleManager;
            _repo = new GebruikersRepo(_context);
        }

        // GET: Gebruikers
        public async Task<IActionResult> Index(string functie)
        {
            var gebruikersList = _repo.GetGebruikers(functie).OrderBy(x => x.Naam);
            string f = functie == null ? "Gebruikers" : $"{functie}en";
            ViewData["Titel"] = f;

            return View(gebruikersList);
        }

        // GET: Gebruikers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gebruiker = await _context.Gebruikers
                .FirstOrDefaultAsync(m => m.GebruikerId == id);
            if (gebruiker == null)
            {
                return NotFound();
            }
            return View(gebruiker);
        }

        // GET: Gebruikers/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Gebruikers/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("GebruikerId,Voornaam,Naam,Email,Functie")] Gebruiker gebruiker)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(gebruiker);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(gebruiker);
        //}
        [Authorize(Roles = Roles.Admin)]
        // GET: Gebruikers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gebruiker = await _context.Gebruikers.FindAsync(id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            // Selectlist doorsturen via ViewBag
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var item in _roleManager.Roles)
            {
                selectList.Add(new SelectListItem { Text = item.Name, Value = item.Name });
            }
            ViewBag.roles = selectList;



            return View(gebruiker);
        }

        // POST: Gebruikers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<object> Edit(int id, [Bind("GebruikerId,Voornaam,Naam,Email,Functie")] Gebruiker updatedGebruiker)
        {
            if (id != updatedGebruiker.GebruikerId)
            {
                return NotFound();
            }
            string oldRole;

            // We kijken eerst of het om een admin gaat
            if (_context.Admins.Any(x => x.GebruikerId == id))
            {
                // Vorige role = admin
                oldRole = Roles.Admin;

                Admin oldAdmin = _context.Admins.Where(x => x.GebruikerId == id).FirstOrDefault();
                // Als de role veranderd is
                if (oldRole != updatedGebruiker.Functie)
                {
                    // weg uit die tabel, en toevoegen aan de nieuwe tabel
                    _context.Admins.Remove(oldAdmin);
                    if (updatedGebruiker.Functie == Roles.Lector)
                        _context.Lectors.Add(new Lector { Gebruiker = updatedGebruiker });
                    else if (updatedGebruiker.Functie == Roles.Student)
                        _context.Students.Add(new Student { Gebruiker = updatedGebruiker });
                }
            }

            // indien niet kijken we of het een lector is
            else if (_context.Lectors.Any(x => x.GebruikerId == id))
            {
                oldRole = Roles.Lector;
                Lector oldLector = _context.Lectors.Where(x => x.GebruikerId == id).FirstOrDefault();
                if (oldRole != updatedGebruiker.Functie)
                {
                    _context.Lectors.Remove(oldLector);
                    if (updatedGebruiker.Functie == Roles.Admin)
                        _context.Admins.Add(new Admin { Gebruiker = updatedGebruiker });
                    else if(updatedGebruiker.Functie == Roles.Student)
                        _context.Students.Add(new Student { Gebruiker= updatedGebruiker });

                }
            }

            // indien niet is het een student
            else
            {
                oldRole = Roles.Student;
                Student oldStudent = _context.Students.Where(x => x.GebruikerId == id).FirstOrDefault();
                if (oldRole != updatedGebruiker.Functie)
                {
                    _context.Students.Remove(oldStudent);
                    if(updatedGebruiker.Functie == Roles.Admin)
                        _context.Admins.Add(new Admin { Gebruiker = updatedGebruiker});
                    else if (updatedGebruiker.Functie == Roles.Lector)
                        _context.Lectors.Add(new Lector { Gebruiker = updatedGebruiker });

                }
            }

            // we hebben customIdentityUser nodig voor de usermanager
            CustomIdentityUser user = _userManager.Users.Where(x => x.Gebruiker.GebruikerId == id).FirstOrDefault();

            // Alle roles
            List<string> roles = new List<string>();
            foreach (var item in _roleManager.Roles)
                roles.Add(item.Name);
            

            if (ModelState.IsValid)
            {
                try
                {
                    // context wordt geupdateted en gesaved
                    _context.Update(updatedGebruiker);
                    
                    await _context.SaveChangesAsync();

                    // Alle rollen worden verwijderd en een nieuwe wordt toegewezen
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    await _userManager.AddToRoleAsync(user, updatedGebruiker.Functie);
                    

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GebruikerExists(updatedGebruiker.GebruikerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedGebruiker);
        }

        
        [Authorize(Roles = Roles.Admin)]
        // GET: Gebruikers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gebruiker = await _context.Gebruikers
                .FirstOrDefaultAsync(m => m.GebruikerId == id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            return View(gebruiker);
        }

        // POST: Gebruikers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gebruiker = await _context.Gebruikers.FindAsync(id);
            _context.Gebruikers.Remove(gebruiker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GebruikerExists(int id)
        {
            return _context.Gebruikers.Any(e => e.GebruikerId == id);
        }
    }
}
