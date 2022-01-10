using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebHelper.Data;
using WebHelper.Models;
using WebHelper.ViewModels;

namespace WebHelper.Components.GebruikersFilter
{
    /// <summary>
    /// De filter bovenaan de gebruikerspagina
    /// </summary>
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
