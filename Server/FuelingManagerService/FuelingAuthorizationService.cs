using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel.PeerResolvers;
using ViewModel;

namespace FuelingServices
{
    public class FuelingAuthorizationService : IFuelingAuthorizationService
    {

        // FuellingSimulator
        
        public bool CheckExistingCarByPlateNumber(string plateNumber)
        {
            return GlobalDB.carsDB.GetCars().Exists(x => x.PlateNumber == plateNumber);
        }
        public FuellingRespond FuellingRequest(FuellingRequest request)
        {
            FuellingRespond respond = new FuellingRespond();

            Car currentCar = GlobalDB.carsDB.GetCars().FirstOrDefault(x => x.PlateNumber == request.PlateNumber);
            currentCar.Rule = GlobalDB.rulesDB.GetRules().FirstOrDefault(x => x.RuleID == currentCar.RuleID);

            SetRespond(currentCar, request, respond);


            // דווח על בקשה שנדחתה
            if (respond.Amount == -1)
            {
                RejectedRequest rejected = new RejectedRequest();
                rejected.RejectedRequestDate = DateTime.Now;
                rejected.CarID = currentCar.CarID;
                rejected.StationID = request.GasStation.StationID;
                //rejected.FleetID = GlobalDB.fleetsDB.GetFleets().FirstOrDefault(x => x.FleetID == currentCar.FleetID).FleetID;
                rejected.ProductID = request.ChosenProduct.ProductID;
                rejected.Reason = respond.Explination;

                GlobalDB.rejectedRequestsDB.Insert(rejected);
            }

            return respond;
        }

        private void SetRespond(Car currentCar, FuellingRequest request, FuellingRespond respond)
        {
            // אם התחנה לא קיימת בחוק
            foreach (var tag in currentCar.Rule.Tags)
            {
                if (!request.GasStation.Tags.Exists(x => x.TagID == tag.TagID))
                {
                    respond.IsAllowed = false;
                    respond.Explination = "תחנה זו לא קיימת בחוק שלך.";
                    respond.Amount = -1;
                    return;
                }
            }

            // אם המוצר לא קיים בחוק
            ProductLimit p = currentCar.Rule.LimitedProducts.FirstOrDefault(x => x.ProductID == request.ChosenProduct.ProductID);
            if (p == null)
            {
                respond.IsAllowed = false;
                respond.Explination = "מוצר זה לא קיים בחוק שלך.";
                respond.Amount = -1;
                return;
            }


            List<Transaction> lst = GetTransactionsByCarAndProductID(currentCar.CarID, request.ChosenProduct.ProductID);

            double dailyVolumeUsed = 0;
            double monthlyVolumeUsed = 0;

            // בודק שימוש
            foreach (var tran in lst)
            {
                // בחודש הנוכחי
                if (tran.TransactionDate.Year == DateTime.Now.Year &&
                    tran.TransactionDate.Month == DateTime.Now.Month)
                {
                    // ביום הנוכחי
                    if (tran.TransactionDate.Day == DateTime.Now.Day)
                    {
                        dailyVolumeUsed += tran.Volume;
                    }

                    monthlyVolumeUsed += tran.Volume;
                }
            }


            // חריגה מהכמות המותרת
            if (monthlyVolumeUsed >= p.MonthlyVolume ||
                dailyVolumeUsed >= p.DailyVolume)
            {
                respond.IsAllowed = false;
                respond.Explination = "נגמר לך המכסה למוצר זה, נסה מועד מאוחר יותר.";
                respond.Amount = -1;
                return;
            }


            var amount = (p.DailyVolume - dailyVolumeUsed <= p.MonthlyVolume - monthlyVolumeUsed) ?
                          p.DailyVolume - dailyVolumeUsed : p.MonthlyVolume - monthlyVolumeUsed;


            respond.IsAllowed = true;
            respond.Amount = amount;
            return;
        }

        private List<Transaction> GetTransactionsByCarAndProductID(int carID, int productID)
        {
            List<Transaction> lst = new List<Transaction>();

            foreach (var tran in GlobalDB.transactionsDB.GetTransactions())
            {
                if (tran.CarID == carID && tran.ProductID == productID)
                {
                    lst.Add(tran);
                }
            }
            return lst;
        }

        public void FuellingReport(FuellingRequest request, double amount)
        {
            Transaction t = new Transaction();
            Car currentCar = GlobalDB.carsDB.GetCars().FirstOrDefault(x => x.PlateNumber == request.PlateNumber);
            
            t.TransactionDate = DateTime.Now;
            t.CarID = currentCar.CarID;
            //t.FleetID = currentCar.FleetID;
            t.StationID = request.GasStation.StationID;
            t.ProductID = request.ChosenProduct.ProductID;
            t.Price = request.ChosenProduct.Price;
            t.Volume = amount;
            t.TotalPayment = Convert.ToDecimal(Convert.ToDouble(t.Price) * t.Volume);

            GlobalDB.transactionsDB.Insert(t);
        }            



        



        // Stations Page
        public List<Station> GetStations()
        {
            return GlobalDB.stationsDB.GetStations();
        }
        public int GetStationsAmount()
        {
            return GlobalDB.stationsDB.GetStations().Count();
        }



        // Products Page
        public int GetProductsAmount()
        {
            return GlobalDB.productsDB.GetProducts().Count();
        }
        public List<Product> GetProducts()
        {
            return GlobalDB.productsDB.GetProducts();
        }

    }
}
