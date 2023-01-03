using PhoneAuthentication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAuthentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<bool> AuthenticateMobile(string mobile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateOTP(string code)
        {
            throw new NotImplementedException();
        }
    }
}
