using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEB_voorbereiding.Models;

namespace WEB_voorbereiding.Tools
{
    public class CustomPasswordValidator : PasswordValidator<CustomIdentityUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<CustomIdentityUser> userManager, CustomIdentityUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();
            IdentityResult result = await base.ValidateAsync(userManager, user, password);
            var validation = result.Succeeded;
            if (!password.ToLower().Equals(password))
            {
                errors.Add(new IdentityError
                {
                    Description = "Paswoord mag geen hoofdletters bevatten!"
                });
                validation = false;
            }
            return validation ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
