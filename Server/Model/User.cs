using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class User : BaseEntity
    {
        [NoDB]
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string LoginName { get; set; }
        [DataMember]
        public string LoginPassword { get; set; }
        [DataMember]
        public int Role { get; set; }
        [DataMember]
        public int? FleetID { get; set; }
        [DataMember]
        public string PicturePath { get; set; }
        [DataMember]
        public Color WorkspaceColor { get; set; }
        [DataMember]
        public bool IsActive { get; set; }



        [NoDB]
        [DataMember]
        public byte[] Picture { get; set; }


        public override string GetTableName()
        {
            return "Users";
        }

        public override string[] GetKeyFields()
        {
            return new string[] { "UserID" };
        }
    }
}
