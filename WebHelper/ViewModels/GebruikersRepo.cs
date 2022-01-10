using System.Collections.Generic;
using System.Linq;
using WebHelper.Data;
using WebHelper.Models;

namespace WebHelper.ViewModels
{
    /// <summary>
    /// Wordt gebruikt in de gebruikersfilter en de gebruikerscontroller
    /// Geeft niets meer dan een lijst van gebruikers, al dan niet gefilterd
    /// </summary>
    public class GebruikersRepo
    {
        ApplicationDbContext _context;

        public GebruikersRepo()
        {
        }

        public GebruikersRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Gebruiker> GetGebruikers(string functie = null)
        {
            List<Gebruiker> gebruikers = _context.Gebruikers.ToList();
            if (functie != null)
            {
                return gebruikers.Where(x => x.Functie == functie).ToList();
            }
            else
            {
                return gebruikers; 
            }
        }
    }
}
