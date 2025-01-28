using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace FuelingServices
{
    public class FuelingManagmentService : IFuelingManagmentService
    {

        // Login Page
        public List<User> GetUsers()
        {
            return GlobalDB.usersDB.GetUsers();
        }
        public User GetUserByLoginName(string loginName)
        {
            return GlobalDB.usersDB.GetUsers().FirstOrDefault(x => x.LoginName == loginName);
        }
        public void AddUser(User user)
        {
            GlobalDB.usersDB.Insert(user);
        }
        public void UpdateUser(User user)
        {
            if (user.Picture != null)
                File.WriteAllBytes(@"../../../Images/" + user.PicturePath, user.Picture);

            GlobalDB.usersDB.Update(user);
        }
        public void DeleteUser(User user)
        {
            GlobalDB.usersDB.Delete(user);
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
        public List<Tag> GetTags()
        {
            return GlobalDB.tagsDB.GetTags();
        }
        public List<City> GetCities()
        {
            return GlobalDB.citiesDB.GetCities();
        }
        public void AddStation(Station station)
        {
            GlobalDB.stationsDB.Insert(station);
        }
        public void AddStationsTags(int stationID, List<Tag> tags)
        {
            StationTag st = new StationTag();
            st.StationID = stationID;

            foreach (var tag in tags as List<Tag>)
            {
                st.TagID = tag.TagID;
                GlobalDB.stationsTagsDB.Insert(st);
            }

            GlobalDB.stationsDB.GetStations().FirstOrDefault(x => x.StationID == stationID).Tags = tags;
        }
        public void UpdateStation(Station station)
        {
            GlobalDB.stationsDB.Update(station);

        }
        public void UpdateStationsTags(int stationID, List<Tag> tags)
        {
            List<Tag> existingTags = GlobalDB.stationsDB.GetStations().FirstOrDefault(x => x.StationID == stationID).Tags;

            StationTag st = new StationTag();
            st.StationID = stationID;

            // Add Tag to the StationsTagsList
            foreach (var tag in tags)
            {
                if (!existingTags.Exists(x => x.TagID == tag.TagID))
                {
                    st.TagID = tag.TagID;
                    GlobalDB.stationsTagsDB.Insert(st);
                }

            }
            // Delete Tag to the StationsTagsList
            foreach (var tag in existingTags)
            {
                if (!tags.Exists(x => x.TagID == tag.TagID))
                {
                    st.TagID = tag.TagID;
                    GlobalDB.stationsTagsDB.Delete(st);
                }
            }
        }
        public void DeleteStation(Station station)
        {
            GlobalDB.stationsDB.Delete(station);

        }




        // Fleets Page
        public List<Fleet> GetFleets()
        {
            return GlobalDB.fleetsDB.GetFleets();
        }
        public int GetFleetsAmount()
        {
            return GlobalDB.fleetsDB.GetFleets().Count();
        }
        public void AddFleet(Fleet fleet)
        {

            // save the logo in the local folder
            if (fleet.Logo != null)
                File.WriteAllBytes(@"../../../Images/" + fleet.LogoPath, fleet.Logo);

            GlobalDB.fleetsDB.Insert(fleet);
        }
        public void AddUsersToFleet(List<User> lstUsers)
        {
            foreach (var user in lstUsers)
            {
                GlobalDB.usersDB.Insert(user);
            }

            Fleet f = GlobalDB.fleetsDB.GetFleets().FirstOrDefault(x => x.FleetID == lstUsers[0].FleetID);
            if (f.Users == null)
                f.Users = new List<User>();
            f.Users = lstUsers;
        }
        public void UpdateFleet(Fleet fleet)
        {

            if (fleet.Logo != null)
                File.WriteAllBytes(@"../../../Images/" + fleet.LogoPath, fleet.Logo);

            GlobalDB.fleetsDB.Update(fleet);
        }
        public void DeleteFleet(Fleet fleet)
        {
            GlobalDB.fleetsDB.Delete(fleet);

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
        public void AddProduct(Product product)
        {
            GlobalDB.productsDB.Insert(product);

        }
        public void UpdateProduct(Product product)
        {
            GlobalDB.productsDB.Update(product);
        }
        public void DeleteProduct(Product product)
        {
            GlobalDB.productsDB.Delete(product);

        }




        // Rules Page
        public List<Rule> GetFleetRules(int fleetId)
        {
            return GlobalDB.rulesDB.GetRules().Where(x => x.FleetID == fleetId).ToList();
        }
        public int GetFleetRulesAmount(int fleetId)
        {
            return GlobalDB.rulesDB.GetRules().Where(x => x.FleetID == fleetId).Count();
        }
        public Rule GetRuleByID(int? ruleID)
        {
            if (ruleID == null)
                return new Rule();

            return GlobalDB.rulesDB.GetRules().FirstOrDefault(x => x.RuleID == ruleID);
        }
        public void AddRule(Rule rule)
        {
            GlobalDB.rulesDB.Insert(rule);

            int currentRuleID = GlobalDB.rulesDB.GetRules().Max(x => x.RuleID);

            //  הוספת תגים לחוק
            foreach (var tag in rule.Tags)
            {
                RuleTag rt = new RuleTag();
                rt.RuleID = currentRuleID;
                rt.TagID = tag.TagID;

                GlobalDB.rulesTagsDB.Insert(rt);
            }
            // הוספת מוצרים מוגבלים לחוק
            foreach (var productLimit in rule.LimitedProducts)
            {
                ProductLimit pl = productLimit;
                pl.RuleID = currentRuleID;
                //pl.ProductID = productLimit.Product.ProductID;

                GlobalDB.productsLimitDB.Insert(pl);
            }

            // רשימת התגים והמוצרים המוגבלים 
            // init נכנסים ב
            // ובגלל שההוספה של החוק הראשונה ורק לאחר מכן מתבצעת ההוספה של התגים והמוצרים
            // Init לכן יש לקרוא לפונקצית
            GlobalDB.rulesDB.init();
        }
        public void UpdateRule(Rule rule)
        {
            // מחיקת התגים הקיימים בחוק זה
            List<RuleTag> oldRuleTags = GlobalDB.rulesTagsDB.GetRulesTags().Where(x => x.RuleID == rule.RuleID).ToList();
            foreach (var ruleTag in oldRuleTags)
            {
                GlobalDB.rulesTagsDB.Delete(ruleTag);
            }

            // מחיקת מוצרים מוגבלים הקיימים בחוק זה
            List<ProductLimit> oldProductLimit = GlobalDB.productsLimitDB.GetProductsLimits().Where(x => x.RuleID == rule.RuleID).ToList();
            foreach (var productLimit in oldProductLimit)
            {
                GlobalDB.productsLimitDB.Delete(productLimit);
            }

            //  הוספת תגים לחוק
            foreach (var tag in rule.Tags)
            {
                RuleTag rt = new RuleTag();
                rt.RuleID = rule.RuleID;
                rt.TagID = tag.TagID;

                GlobalDB.rulesTagsDB.Insert(rt);
            }

            // הוספת מוצרים מוגבלים לחוק
            foreach (var productLimit in rule.LimitedProducts)
            {
                ProductLimit pl = productLimit;
                pl.RuleID = rule.RuleID;
                //pl.ProductID = productLimit.Product.ProductID;

                GlobalDB.productsLimitDB.Insert(pl);
            }

            // רשימת התגים והמוצרים המוגבלים 
            // init נכנסים ב
            // ובגלל שההוספה של החוק הראשונה ורק לאחר מכן מתבצעת ההוספה של התגים והמוצרים
            // Init לכן יש לקרוא לפונקצית
            GlobalDB.rulesDB.init();
        }
        public void DeleteRule(Rule rule)
        {
            // מחיקת התגים הקיימים בחוק זה
            if (rule.Tags != null)
            {
                List<RuleTag> oldRuleTags = GlobalDB.rulesTagsDB.GetRulesTags().Where(x => x.RuleID == rule.RuleID).ToList();
                foreach (var ruleTag in oldRuleTags)
                {
                    GlobalDB.rulesTagsDB.Delete(ruleTag);
                }
            }

            // מחיקת מוצרים מוגבלים הקיימים בחוק זה
            if (rule.LimitedProducts != null)
            {
                List<ProductLimit> oldProductLimit = GlobalDB.productsLimitDB.GetProductsLimits().Where(x => x.RuleID == rule.RuleID).ToList();
                foreach (var productLimit in oldProductLimit)
                {
                    GlobalDB.productsLimitDB.Delete(productLimit);
                }
            }

            // מחיקת החוק מכל הרכבים הקשורים לחוק זה
            List<Car> relatedCars = GetFleetCars(rule.FleetID);
            foreach (Car car in relatedCars)
            {
                if (car.RuleID == rule.RuleID)
                {
                    car.RuleID = null;
                    GlobalDB.carsDB.Update(car);
                }
            }


            GlobalDB.rulesDB.Delete(rule);
        }



        // WorkerCars Page
        public int GetFleetCarsAmount(int fleetId)
        {
            return GlobalDB.carsDB.GetCars().Where(x => x.FleetID == fleetId).Count();
        }
        public List<Car> GetFleetCars(int fleetId)
        {
            return GlobalDB.carsDB.GetCars().Where(x => x.FleetID == fleetId).ToList();
        }
        public void AddWorkerCar(Car car)
        {
            GlobalDB.carsDB.Insert(car);
        }
        public void UpdateWorkerCar(Car car)
        {
            GlobalDB.carsDB.Update(car);
        }
        public void DeleteWorkerCar(Car car)
        {
            GlobalDB.carsDB.Delete(car);
        }



        // Transactions Page
        public List<Transaction> GetTransactions(int fleetID)
        {
            return GlobalDB.transactionsDB.GetTransactions().Where(x=>x.Car.FleetID == fleetID).ToList();
        }

        public List<Transaction> GetTransactionsByCarID(int carID)
        {
            List<Transaction> lst = new List<Transaction>();

            foreach (var tran in GlobalDB.transactionsDB.GetTransactions())
            {
                if (tran.CarID == carID)
                {
                    lst.Add(tran);
                }
            }
            return lst;
        }



        // RejectedRequest Page
        public List<RejectedRequest> GetRejectedRequests(int fleetID)
        {
            return GlobalDB.rejectedRequestsDB.GetRejectedRequests().Where(x=>x.Car.FleetID == fleetID).ToList();
        }

    }
}
