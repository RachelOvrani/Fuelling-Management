using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class CitiesDB : BaseDB 
    {
        protected override BaseEntity CreatModel()
        {
            City city = new City();

            // Fill the information from the access to the object
            city.CityID = Convert.ToInt32(reader["CityID"]);
            city.CityName = reader["CityName"].ToString();

            return city;
        }
        public override void init() { }


        protected override string GetTableName()
        {
            return "Cities";
        }

        public List<City> GetCities()
        {
            return lst.Cast<City>().ToList();
        }


    }
}
