using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class CarsDB : BaseDB
    {
        public CarsDB() { }

        protected override BaseEntity CreatModel()
        {
            // Create a new Fleet object
            Car car = new Car();

            // Fill the information from the access to the objectfleet.CompanyName = reader["CompanyName"].ToString();
            car.CarID = Convert.ToInt32(reader["CarID"]);
            car.PlateNumber = reader["PlateNumber"].ToString();
            car.Model = reader["Model"].ToString();
            car.Menufactor = reader["Menufactor"].ToString();
            car.OwnerID = reader["OwnerID"].ToString();
            car.OwnerName = reader["OwnerName"].ToString();
            car.OwnerPhoneNumber = reader["OwnerPhoneNumber"].ToString();
            car.FleetID = Convert.ToInt32(reader["FleetID"]);

            int tempVal;
            car.RuleID = Int32.TryParse(reader["RuleID"].ToString(), out tempVal) ? tempVal : (int?)null;


            return car;
        }

        protected override string GetTableName()
        {
            return "Cars";
        }

        public List<Car> GetCars()
        {
            return lst.Cast<Car>().ToList();
        }

        public override void init()
        {
            this.lst.ForEach((Action<BaseEntity>)(entity =>
            {
               // Car car = entity as Car;
                //Fleet fleet = Enumerable.FirstOrDefault<Fleet>(GlobalDB.fleetsDB.GetFleets(), (Func<Fleet, bool>)(x => x.FleetID == car.FleetId));
                //car.Fleet = fleet;
            }));
        }

    }
}
