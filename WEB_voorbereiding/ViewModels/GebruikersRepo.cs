using System.Collections.Generic;
using System.Linq;
using WEB_voorbereiding.Data;
using WEB_voorbereiding.Models;

namespace WEB_voorbereiding.ViewModels
{
    public class GebruikersRepo
    {
        ApplicationDbContext _context;
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
