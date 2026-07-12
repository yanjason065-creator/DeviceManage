using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagement.Api.Tests.Helpers
{
    public static class JwtHelper
    {
        public static JwtSecurityToken Decode(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(jwtToken);  
        }
    }
}
