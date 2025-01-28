using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class FleetsDB : BaseDB
    {
        public FleetsDB() { }


        protected override BaseEntity CreatModel()
        {
            // Create a new Fleet object
            Fleet fleet = new Fleet();

            // Fill the information from the access to the object
            fleet.FleetID = Convert.ToInt32(reader["FleetID"]);
            fleet.CompanyName = reader["CompanyName"].ToString();
            fleet.Amount = Convert.ToDouble(reader["Amount"]);
            fleet.Credit = Convert.ToDouble(reader["Credit"]);
            fleet.Discount = Convert.ToInt32(reader["Discount"]);
            fleet.LogoPath = reader["LogoPath"].ToString();
            fleet.Active = Convert.ToBoolean(reader["Active"]);


            return fleet;
        }

       
        public override void init()
        {
            this.lst.ForEach(entity =>
            {
                Fleet fleet = entity as Fleet;
                fleet.Cars = GlobalDB.carsDB.GetCars().Where(x => x.FleetID == fleet.FleetID).ToList();

                if (fleet.LogoPath != "")
                    fleet.Logo = File.ReadAllBytes(@"../../../Images/" + fleet.LogoPath);

                if (fleet.FleetID != null)
                    fleet.Users = GlobalDB.usersDB.GetUsers().Where(x => x.FleetID == fleet.FleetID).ToList();

                //fleet.lstRules = GlobalDB.rulesDB.GetRules().Where(x => x.FleetID == fleet.FleetID).ToList();
            });
        }

        protected override string GetTableName()
        {
            return "Fleets";
        }

        public List<Fleet> GetFleets()
        {
            return lst.Cast<Fleet>().ToList();
        }

    }
}
