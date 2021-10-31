using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Businesss.Options
{
    public class AccountOptions
    {
        public string Secret { get; set; }
        public int Expires { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
