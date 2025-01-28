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
    public class StationsDB : BaseDB
    {      
        public StationsDB() { }
        

        protected override BaseEntity CreatModel()
        {
            // Create a new Fleet object
            Station station = new Station();

            // Fill the information from the access to the object
            station.StationID = Convert.ToInt32(reader["StationID"]);
            station.StationName = reader["StationName"].ToString();
            station.Address = reader["Address"].ToString();
            station.CityID = Convert.ToInt32(reader["CityID"]);
            
            return station;
        }
        public override void init()
        { 
            this.lst.ForEach((Action<BaseEntity>)(entity =>
            {
                Station station = entity as Station;
                City city = GlobalDB.citiesDB.GetCities().FirstOrDefault(x=> x.CityID == station.CityID);
                station.City = city;

               
                List<Tag> tags = GlobalDB.stationsTagsDB.GetStationsTags().Where(x => x.StationID == station.StationID).Select(x=>x.Tag).ToList();
                station.Tags = tags;
            }));
            
        }

        protected override string GetTableName()
        {
            return "Stations";
        }

        public List<Station> GetStations()
        {
            return lst.Cast<Station>().ToList();
        }


       
    }
}
