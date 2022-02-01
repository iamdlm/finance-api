using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinApi.Helpers
{
    public class AppSettings
    {
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
        public string TokenSecret { get; set; }
    }
}
