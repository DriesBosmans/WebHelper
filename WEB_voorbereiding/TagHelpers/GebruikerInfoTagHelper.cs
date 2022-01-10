using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Text;
using WEB_voorbereiding.Data;

namespace WEB_voorbereiding.TagHelpers
{
    /// <summary>
    /// Niet vergeten te registreren in _ViewImports.cshtml
    /// </summary>
    [HtmlTargetElement("gebruiker-info")]
    public class GebruikerInfoTagHelper : TagHelper
    {
        ApplicationDbContext _context;
        public GebruikerInfoTagHelper(ApplicationDbContext context)
        {
            _context = context;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<div class='border border-primary'>");
            html.Append($"<h4>Aantal gebruikers: {GetGebruikerCount()}</h4>");
            html.Append("<p>Dit is de gebruiker-info taghelper.</p>");
            html.Append("</div>");
            output.Content.SetHtmlContent(html.ToString());
        }
        private int GetGebruikerCount()
        {
            int count = _context.Gebruikers.Count();
            return count;
        }
    }
}
