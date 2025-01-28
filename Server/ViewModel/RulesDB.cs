using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class RulesDB : BaseDB
    {
        protected override BaseEntity CreatModel()
        {
            Rule rule = new Rule();

            rule.RuleID = Convert.ToInt32(reader["RuleID"]);
            rule.RuleName = reader["RuleName"].ToString();
            rule.FleetID = Convert.ToInt32(reader["FleetID"]);

            return rule;
        }

        public override void init()
        {
            // בעצם השליפה האחרי ההוספה לא מתבצעת אחרי ההוספה
            this.lst.ForEach(entity =>
            {
                Rule rule = entity as Rule;
                rule.LimitedProducts = GlobalDB.productsLimitDB.GetProductsLimits().Where(x=>x.RuleID == rule.RuleID).ToList();
                rule.Tags = GlobalDB.rulesTagsDB.GetRulesTags().Where(x => x.RuleID == rule.RuleID).Select(x=>x.Tag).ToList();
            });
        }

        public List<Rule> GetRules()
        {
            return lst.Cast<Rule>().ToList();   
        }

        protected override string GetTableName()
        {
            return "Rules";
        }
    }
}
