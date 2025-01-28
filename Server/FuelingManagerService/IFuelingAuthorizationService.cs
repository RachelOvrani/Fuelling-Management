using Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace FuelingServices
{
    [ServiceContract]
    public interface IFuelingAuthorizationService
    {
        // FuellingSimulator
        [OperationContract]
        FuellingRespond FuellingRequest(FuellingRequest request);
        
        [OperationContract]
        void FuellingReport(FuellingRequest request, double amount);

        [OperationContract]
        bool CheckExistingCarByPlateNumber(string plateNumber);

        // Stations
        [OperationContract]
        List<Station> GetStations();

        [OperationContract]
        int GetStationsAmount();


        // Products
        [OperationContract]
        int GetProductsAmount();

        [OperationContract]
        List<Product> GetProducts();

    }
}
