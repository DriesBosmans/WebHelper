using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using WEB_voorbereiding.Models;
using WEB_voorbereiding.Tools;

namespace WEB_voorbereiding.Data
{
    public static class SeedData
    {
        public async static Task EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            UserManager<CustomIdentityUser> userManager = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<UserManager<CustomIdentityUser>>();

            RoleManager<IdentityRole> roleManager = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            // 3 gebruikers=> Student Dries, Lector Kristof, Admin Natacha
            // 1 Vak=> C# Web
            if (!context.Gebruikers.Any())
            {
                // Roles aanmaken
                await CreateRolesAsync(context, roleManager);
                Gebruiker d = new Gebruiker { Voornaam = "Dries", Naam = "Bosmans", Email = "dries.bosmans@student.pxl.be", Functie = Roles.Student };
                Gebruiker k = new Gebruiker { Voornaam = "Kristof", Naam = "Palmaers", Email = "kristof.palmaers@pxl.be", Functie = Roles.Lector };
                Gebruiker n = new Gebruiker { Voornaam = "Natacha", Naam = "Bruggen", Email = "natacha.bruggen@pxl.be", Functie = Roles.Admin };
                context.AddRange(d, k, n);

                Student dries = new Student { Gebruiker = d };
                Lector kristof = new Lector { Gebruiker = k };
                Admin natacha = new Admin { Gebruiker = n };
                Vak csharp = new Vak { Cursus = "Csharp Web", Handboek = "Asp.net", StudiePunten = 6 };
                VakLector vl = new VakLector { Lector = kristof, Vak = csharp };
                context.Students.Add(dries);
                context.Lectors.Add(kristof);
                context.Admins.Add(natacha);
                context.Vakken.Add(csharp);
                context.VakLectors.Add(vl);

                // Belangrijk dat de gebruikers eerst opgeslagen worden voor identity wordt aangemaakt
                context.SaveChanges();

                await CreateIdentityRecordsAsync(d, userManager, roleManager);
                await CreateIdentityRecordsAsync(k, userManager, roleManager);
                await CreateIdentityRecordsAsync(n, userManager, roleManager);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

        }

        async static Task CreateRolesAsync(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            if (!context.Roles.Any())
            {
                IdentityRole role = new IdentityRole(Roles.Student);
                await roleManager.CreateAsync(role);
                role = new IdentityRole(Roles.Admin);
                await roleManager.CreateAsync(role);
                role = new IdentityRole(Roles.Lector);
                await roleManager.CreateAsync(role);
            }
        }
        async static Task CreateIdentityRecordsAsync(Gebruiker g, UserManager<CustomIdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await userManager.FindByEmailAsync(g.Email) == null)
            {
                string password = "qsdf1234";                                                          // gebruikerID doorgeven
                CustomIdentityUser user = new CustomIdentityUser { Email = g.Email, UserName = g.Email, GebruikerId = g.GebruikerId };

                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var role = await roleManager.FindByNameAsync(g.Functie);
                    if (role != null)
                    {
                        await userManager.AddToRoleAsync(user, role.Name);
                    }
                }


            }
        }
    }
}
