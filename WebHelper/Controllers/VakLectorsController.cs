using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHelper.Data;
using WebHelper.Models;
using WebHelper.ViewModels;

namespace WebHelper.Controllers
{
    public class VakLectorsController : Controller
    {
        /// <summary>
        /// Deze klasse heb ik er in gestoken voor de foreign keys
        /// De enige moeilijkheid is de id's vervangen door namen in de views
        /// </summary>
        private readonly ApplicationDbContext _context;

        public VakLectorsController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Enkel .ThenInclude toevoegen voor gebruiker
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// // Hier gebruiken we LectorNaamViewModel omdat we niet aan de naam van de gebruiker kunnen
        /// </summary>
        /// <returns></returns>
        // GET: VakLectors/Create
        public IActionResult Create()
        {
            
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vakLector"></param>
        /// <returns></returns>
        // POST: VakLectors/Create
        
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

            // wordt enkel uitgevoerd bij een invalid postrequest
            var lectoren = _context.Lectors.Include(l => l.Gebruiker);
            List<LectorNaamViewModel> lectorlist = new List<LectorNaamViewModel>();
            foreach (var lector in lectoren)
            {
                LectorNaamViewModel vm = new LectorNaamViewModel(lector);
                lectorlist.Add(vm);

            }
            ViewData["LectorId"] = new SelectList(lectorlist, "LectorId", "VolledigeNaam");
            ViewData["VakId"] = new SelectList(_context.Vakken, "VakId", "Cursus");
            return View(vakLector);
        }
        /// <summary>
        /// Hier gebruiken we LectorNaamViewModel voor aan de naam te kunnen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// .ThenInclude voor aan de gebruiker te kunnen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
