using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ViewModel;
using Model;
using FuelingServices;
using System.ServiceModel.Description;

namespace Host
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the server side...");
            ServiceHost FuelingManagmentService = new ServiceHost(typeof(FuelingServices.FuelingManagmentService));
            ServiceHost AuthorizationService = new ServiceHost(typeof(FuelingServices.FuelingAuthorizationService));
            FuelingManagmentService.Open();
            AuthorizationService.Open();
            Console.ReadLine();



        }
    }
}
