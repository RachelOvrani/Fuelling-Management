using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class TagsDB : BaseDB
    {
        protected override BaseEntity CreatModel()
        {
            // Create a new tag object
            Tag tag = new Tag();

            // Fill the information from the access to the object
            tag.TagID = Convert.ToInt32(reader["TagID"]);
            tag.TagName = reader["TagName"].ToString();

            return tag;
        }

        protected override string GetTableName()
        {
            return "Tags";
        }

        public List<Tag> GetTags()
        {
            return lst.Cast<Tag>().ToList();
        }

        public override void init() { }
    }
}
