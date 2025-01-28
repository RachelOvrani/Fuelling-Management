using FuelingSimulatorClient.FuelingAuthorizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public static class Global
    {
        public static FuelingAuthorizationServiceClient client = new FuelingAuthorizationServiceClient();

        public static string DefaultColorHex = "#FF00BCD4";

    }
}
