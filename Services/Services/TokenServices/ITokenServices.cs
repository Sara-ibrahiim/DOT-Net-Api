using Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TokenServices
{
    public interface ITokenServices
    {
        string CreateToken(AppUser appUser);
    }
}
