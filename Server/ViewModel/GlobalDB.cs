using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public static class GlobalDB
    {
        public static UsersDB usersDB { get; set; }
        public static FleetsDB fleetsDB { get; set; }
        public static CarsDB carsDB { get; set; }
        public static ProductsDB productsDB { get; set; }
        public static CitiesDB citiesDB { get; set; }
        public static TagsDB tagsDB { get; set; }
        public static StationsTagsDB stationsTagsDB { get; set; }
        public static StationsDB stationsDB { get; set; }
        public static ProductsLimitDB productsLimitDB { get; set; }
        public static RulesTagsDB rulesTagsDB { get; set; }
        public static RejectedRequestsDB rejectedRequestsDB { get; set; }
        public static TransactionsDB transactionsDB { get; set; }

        public static RulesDB rulesDB { get; set; }

        static GlobalDB()
        {
            usersDB = new UsersDB();
            fleetsDB = new FleetsDB();
            carsDB = new CarsDB();
            tagsDB = new TagsDB();
            productsDB = new ProductsDB();
            productsLimitDB = new ProductsLimitDB();
            rulesTagsDB = new RulesTagsDB();
            citiesDB = new CitiesDB();
            stationsTagsDB = new StationsTagsDB();
            stationsDB = new StationsDB();
            rejectedRequestsDB = new RejectedRequestsDB();
            transactionsDB = new TransactionsDB();
            rulesDB = new RulesDB();

            fleetsDB.init();
            carsDB.init();
            usersDB.init();
            productsDB.init();
            citiesDB.init();
            tagsDB.init();
            stationsTagsDB.init();
            stationsDB.init();
            rejectedRequestsDB.init();
            transactionsDB.init();
            productsLimitDB.init();
            rulesTagsDB.init();
            rulesDB.init(); 

        }
    }
}
