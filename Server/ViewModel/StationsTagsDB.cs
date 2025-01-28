using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class StationsTagsDB : BaseDB
    {
        public StationsTagsDB() { }


        protected override BaseEntity CreatModel()
        {
            // Create a new station tag object
            StationTag stationTag = new StationTag();

            // Fill the information from the access to the object
            stationTag.StationID = Convert.ToInt32(reader["StationID"]);
            stationTag.TagID = Convert.ToInt32(reader["TagID"]);

            return stationTag;
        }

        protected override string GetTableName()
        {
            return "StationsTags";
        }


        public List<StationTag> GetStationsTags()
        {
            return lst.Cast<StationTag>().ToList();
        }


        public override void init()
        {
            this.lst.ForEach(entity =>
            {
                StationTag stationTag = entity as StationTag;
                Station station = GlobalDB.stationsDB.GetStations().FirstOrDefault(x => x.StationID == stationTag.StationID);
                stationTag.Station = station;


                Tag tag = GlobalDB.tagsDB.GetTags().FirstOrDefault(x => x.TagID == stationTag.TagID);
                stationTag.Tag = tag;
            });
        }
    }
}
