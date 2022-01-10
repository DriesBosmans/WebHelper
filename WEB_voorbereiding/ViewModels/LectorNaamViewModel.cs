using System.ComponentModel;
using WEB_voorbereiding.Data;
using WEB_voorbereiding.Models;

namespace WEB_voorbereiding.ViewModels
{
    public class LectorNaamViewModel
    {
        /// <summary>
        /// 
        /// Dit viewmodel wordt in de create en edit acties van de vaklectorscontroller gebruikt 
        /// om aan de volledige naam van de lector te komen ipv de id
        /// 
        /// </summary>
        /// <param name="l"></param>
        public LectorNaamViewModel(Lector l)
        {
            LectorId = l.LectorId;
            VolledigeNaam = l.Gebruiker.VolledigeNaam;
        }
  
        public int LectorId { get; set; }
       
        public string VolledigeNaam { get; set; }
        
    }
}
