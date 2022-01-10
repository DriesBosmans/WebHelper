using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebHelper.TagHelpers
{
    /// <summary>
    /// Niet vergeten te registreren in _ViewImports.cshtml
    /// </summary>
    //target element, het attribuut dat we willen zetten
    [HtmlTargetElement("h4", Attributes = "text-color")]
    public class ChangeTextColorTagHelper : TagHelper
    { 
        // deze property moet overeenkomen met het attribuut
        public string TextColor { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // We zetten een bootstrap klasse bv text-primary
            output.Attributes.SetAttribute("class", $"text-{TextColor}");
        }
    }
}
