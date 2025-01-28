using Manager.FuelingManagmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public static class Global
    {
        public static FuelingManagmentServiceClient client = new FuelingManagmentServiceClient();

        public static string DefaultColorHex = "#FF00BCD4";

        
        
        
        public static User CurrentUser { get; set; }
        public static Fleet CurrentFleet { get; set; }
        public static List<Rule> fleetRules { get;set; }
        public static List<Product> lstProducts{ get;set; }




    }
}
