using Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace FuelingServices
{
    [ServiceContract]
    public interface IFuelingManagmentService
    {
        // Login Page

        [OperationContract]
        List<User> GetUsers();

        [OperationContract]
        User GetUserByLoginName(string loginName);

        [OperationContract]
        void AddUser(User user);

        [OperationContract]
        void UpdateUser(User user);

        [OperationContract]
        void DeleteUser(User user);




        // Stations Page
        [OperationContract]
        List<Station> GetStations();

        [OperationContract]
        int GetStationsAmount();

        [OperationContract]
        List<Tag> GetTags();

        [OperationContract]
        List<City> GetCities();

        [OperationContract]
        void AddStation(Station station);

        [OperationContract]
        void AddStationsTags(int stationId, List<Tag> tags);

        [OperationContract]
        void UpdateStation(Station station);

        [OperationContract]
        void UpdateStationsTags(int stationID, List<Tag> tags);

        [OperationContract]
        void DeleteStation(Station station);




        // Fleets Page
        [OperationContract]
        List<Fleet> GetFleets();

        [OperationContract]
        int GetFleetsAmount();

        [OperationContract]
        void AddFleet(Fleet fleet);

        [OperationContract]
        void AddUsersToFleet(List<User> lstUsers);

        [OperationContract]
        void UpdateFleet(Fleet fleet);

        [OperationContract]
        void DeleteFleet(Fleet fleet);




        // Products Page
        [OperationContract]
        int GetProductsAmount();

        [OperationContract]
        List<Product> GetProducts();

        [OperationContract]
        void AddProduct(Product product);

        [OperationContract]
        void UpdateProduct(Product product);

        [OperationContract]
        void DeleteProduct(Product product);





        // Rules Page
        [OperationContract]
        List<Rule> GetFleetRules(int fleetId);

        [OperationContract]
        Rule GetRuleByID(int? ruleID);

        [OperationContract]
        int GetFleetRulesAmount(int fleetId);

        [OperationContract]
        void AddRule(Rule rule);

        [OperationContract]
        void UpdateRule(Rule rule);

        [OperationContract]
        void DeleteRule(Rule rule);





        // WorkerCar Page
        [OperationContract]
        List<Car> GetFleetCars(int fleetId);

        [OperationContract]
        int GetFleetCarsAmount(int fleetId);

        [OperationContract]
        void AddWorkerCar(Car car);

        [OperationContract]
        void UpdateWorkerCar(Car car);

        [OperationContract]
        void DeleteWorkerCar(Car car);




        // Transactions Page
        [OperationContract]
        List<Transaction> GetTransactions(int fleetID);
        [OperationContract]
        List<Transaction> GetTransactionsByCarID(int carID);




        // RejectedRequest Page
        [OperationContract]
        List<RejectedRequest> GetRejectedRequests(int fleetID);


    }
}
