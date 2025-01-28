using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class UsersDB : BaseDB
    {
        public UsersDB() { }


        protected override BaseEntity CreatModel()
        {
            // Create a new User object
            User user = new User();

            // Fill the information from the access to the object
            user.UserID = Convert.ToInt32(reader["UserID"]);
            user.FirstName = reader["FirstName"].ToString();
            user.LastName = reader["LastName"].ToString();
            user.ID = reader["ID"].ToString();
            user.PhoneNumber = reader["PhoneNumber"].ToString();
            user.LoginName = reader["LoginName"].ToString();
            user.LoginPassword = reader["LoginPassword"].ToString();
            user.Role = Convert.ToInt32(reader["Role"]);

            var fleetID = reader["FleetID"].ToString();
            if (fleetID is "")
                user.FleetID = null;
            else
                user.FleetID = Convert.ToInt32(fleetID);

            user.PicturePath = reader["PicturePath"].ToString();

            // Convert hex to Color
            string colorFromDB = reader["WorkspaceColor"].ToString();
            user.WorkspaceColor = System.Drawing.ColorTranslator.FromHtml(colorFromDB);

            user.IsActive = Convert.ToBoolean(reader["IsActive"]);

            return user;
        }


        public override void init()
        {
            this.lst.ForEach((Action<BaseEntity>)(entity =>
            {
                User user = entity as User;
                if (user.PicturePath != "")
                    user.Picture = File.ReadAllBytes(@"../../../Images/" + user.PicturePath);

                //if (user.FleetID != null && user.FleetID != 0)
                //   user.Fleet = GlobalDB.fleetsDB.GetFleets().FirstOrDefault(x => x.FleetID == user.FleetID);
            }));


        }

        protected override string GetTableName()
        {
            return "Users";
        }

        public List<User> GetUsers()
        {
            return lst.Cast<User>().ToList();
        }


    }
}
