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
        private readonly ApplicationDbContext _context;
        private readonly GebruikersRepo _repo;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GebruikersController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _repo = new GebruikersRepo(_context);
            _roleManager = roleManager;
        }
        public SelectList GetAllRoles(string selectedRoleId = "1")
        {
            return new SelectList(_roleManager.Roles.ToList(), "Id", "Name", selectedRoleId);
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gebruikers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GebruikerId,Voornaam,Naam,Email,Functie")] Gebruiker gebruiker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gebruiker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gebruiker);
        }
        [Authorize(Roles = Roles.Admin)]
        // GET: Gebruikers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            List<IdentityRole> identityRoles = new List<IdentityRole>();
            var roles = await _roleManager.Roles.ToListAsync();
           // SelectList s = new SelectList(roles);

            identityRoles.AddRange(_roleManager.Roles);
            SelectList s = new SelectList(roles, "id", "roleName");
            var gebruiker = await _context.Gebruikers.FindAsync(id);
            //List<string> lstfuncties = _repo.GetGebruikers().Select(x => x.Functie).Distinct().ToList() ;

            ViewData["RoleId"] = new SelectList(_roleManager.Roles, "Id", "Name");
            if (id == null)
            {
                return NotFound();
            }

            
            if (gebruiker == null)
            {
                return NotFound();
            }
            return View(gebruiker);
        }

        // POST: Gebruikers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GebruikerId,Voornaam,Naam,Email,Functie")] Gebruiker gebruiker)
        {
            if (id != gebruiker.GebruikerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gebruiker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GebruikerExists(gebruiker.GebruikerId))
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
            return View(gebruiker);
        }

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
