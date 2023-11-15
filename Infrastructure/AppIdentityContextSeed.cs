using Core.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppIdentityContextSeed
    {

        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Any()) {

                var user = new AppUser
                {
                    DisplayName = "Sara",
                    Email = "sou.cute94@gmail.com",
                    UserName = "SaraIbrahim",
                    Address = new Address
                    {
                        FirstName="Sara", 
                        LastName="Ibrahim",
                        Street="77",
                        State="Cairo",
                        City="cairo",
                        ZipCode="1568"

                    }
                };

                await userManager.CreateAsync(user,"Password1!");

            
            
            
            
            }





        }
    }
}
