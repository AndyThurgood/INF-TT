using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Configuration
{
    public class ApiConfigurationOption
    {
        public string ServiceAddress { get; set; }
        public string EstablishmentEndpoint { get; set; }
        public string AuthorityEndpoint { get; set; }
        public string CacheKey { get; set; }
    }
}
