using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WEB_voorbereiding.Data;
using WEB_voorbereiding.Models;
using WEB_voorbereiding.ViewModels;

namespace WEB_voorbereiding.Components.GebruikersFilter
{
    public class GebruikersFilterViewComponent : ViewComponent
    {
        private GebruikersRepo _repo;

        public GebruikersFilterViewComponent(ApplicationDbContext context)
        {
            _repo = new GebruikersRepo(context);
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["functie"];
            List<Gebruiker> gebruikersList = _repo.GetGebruikers();
            List<string> functieList = gebruikersList
                .Select(x => x.Functie)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            return View(functieList);
        }
    }
}
