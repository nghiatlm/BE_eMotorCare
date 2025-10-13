using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMotoCare.Infrastructure.Sms
{
    public class VonageSettings
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string FromNumber { get; set; }
    }
}
