using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class RulesTagsDB : BaseDB
    {
        protected override BaseEntity CreatModel()
        {
            RuleTag ruleTag = new RuleTag();

            ruleTag.TagID = Convert.ToInt32(reader["TagID"]);
            ruleTag.RuleID = Convert.ToInt32(reader["RuleID"]);

            return ruleTag;
        }

        public override void init()
        {
            this.lst.ForEach(entity =>
            {
                RuleTag ruleTag = entity as RuleTag;

                ruleTag.Tag = GlobalDB.tagsDB.GetTags().FirstOrDefault(x => x.TagID == ruleTag.TagID);

                // מחפש אותם ולא הם אותו Rule ה
                //ruleTag.Rule  = GlobalDB.rulesDB.GetRules().FirstOrDefault(x=>x.RuleID == ruleTag.RuleID);

            });
        }

        public List<RuleTag> GetRulesTags()
        {
            return lst.Cast<RuleTag>().ToList();
        }


        protected override string GetTableName()
        {
            return "RulesTags";
        }
    }
}
