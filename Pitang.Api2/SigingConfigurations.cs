using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Pitang.Api2
{
    public class SigingConfigurations
    {
        public SecurityKey _key { get; }
        public SigningCredentials _SigningCredentials { get; }

        public SigingConfigurations()
        {
            using(var provider = new RSACryptoServiceProvider(2048))
            {
                _key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            _SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256Signature);

           
        }

   
        
    }
}
