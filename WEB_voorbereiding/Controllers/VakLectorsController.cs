using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_voorbereiding.Data;
using WEB_voorbereiding.Models;
using WEB_voorbereiding.ViewModels;

namespace WEB_voorbereiding.Controllers
{
    public class VakLectorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VakLectorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VakLectors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context
                .VakLectors
                .Include(v => v.Lector)
                .ThenInclude(v => v.Gebruiker) // Gebruiker includen om aan de naam te kunnen in de view
                .Include(v => v.Vak);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VakLectors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vakLector = await _context.VakLectors
                .Include(v => v.Lector)
                .ThenInclude(v => v.Gebruiker)// Gebruiker includen om aan de naam te kunnen in de view
                .Include(v => v.Vak)
                .FirstOrDefaultAsync(m => m.VakLectorId == id);
            if (vakLector == null)
            {
                return NotFound();
            }

            return View(vakLector);
        }

        // GET: VakLectors/Create
        public IActionResult Create()
        {
            // Hier gebruiken we Lectornaamviewmodel omdat we niet aan de naam van de gebruiker kunnen
            var lectoren = _context.Lectors.Include(l => l.Gebruiker);
            List<LectorNaamViewModel> lectorlist = new List<LectorNaamViewModel>();
            foreach (var lector in lectoren)
            {
                LectorNaamViewModel vm = new LectorNaamViewModel(lector);
                lectorlist.Add(vm);

            }
            ViewData["LectorId"] = new SelectList(lectorlist, "LectorId", "VolledigeNaam");
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Cursus");
            return View();
        }

        // POST: VakLectors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VakLectorId,VakId,LectorId")] VakLector vakLector)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vakLector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LectorId"] = new SelectList(_context.Lectors, "LectorId", "LectorId", vakLector.LectorId);
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Cursus", vakLector.VakId);
            return View(vakLector);
        }

        // GET: VakLectors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vakLector = await _context.VakLectors.FindAsync(id);
            if (vakLector == null)
            {
                return NotFound();
            }
            var lectoren = _context.Lectors.Include(l => l.Gebruiker);
            List<LectorNaamViewModel> lectorlist = new List<LectorNaamViewModel>();
            foreach (var lector in lectoren)
            {
                LectorNaamViewModel vm = new LectorNaamViewModel(lector);
                lectorlist.Add(vm);

            }
            ViewData["LectorId"] = new SelectList(lectorlist, "LectorId", "VolledigeNaam", vakLector.LectorId);
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Cursus", vakLector.VakId);
            return View(vakLector);
        }

        // POST: VakLectors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VakLectorId,VakId,LectorId")] VakLector vakLector)
        {
            if (id != vakLector.VakLectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vakLector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VakLectorExists(vakLector.VakLectorId))
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
            ViewData["LectorId"] = new SelectList(_context.Lectors, "LectorId", "LectorId", vakLector.LectorId);
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Cursus", vakLector.VakId);
            return View(vakLector);
        }

        // GET: VakLectors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vakLector = await _context.VakLectors
                .Include(v => v.Lector)
                .ThenInclude(v => v.Gebruiker)
                .Include(v => v.Vak)
                .FirstOrDefaultAsync(m => m.VakLectorId == id);
            if (vakLector == null)
            {
                return NotFound();
            }

            return View(vakLector);
        }

        // POST: VakLectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vakLector = await _context.VakLectors.FindAsync(id);
            _context.VakLectors.Remove(vakLector);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VakLectorExists(int id)
        {
            return _context.VakLectors.Any(e => e.VakLectorId == id);
        }
    }
}
