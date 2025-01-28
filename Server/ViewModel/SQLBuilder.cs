using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public static class SQLBuilder
    {
        public static string InsertEntity(BaseEntity entity)
        {
            Type type = entity.GetType();
            string command = "INSERT INTO " + entity.GetTableName() + " (";
            string values = " VALUES (";

            foreach (var property in type.GetProperties())
            {
                // שדה שאינו שייך ל-DB
                // נשתמש בזה גם עבור שדה מזהה שאין צורך להקצות לו ערך
                if (property.GetCustomAttributes(typeof(NoDBAttribute), false).Length > 0)
                    continue;

                object value = property.GetValue(entity);
                command += property.Name + " ,";
                values += PutValue(value) + " ,";
            }
            command = command.Substring(0, command.Length - 2) + ")";
            values = values.Substring(0, values.Length - 2) + ")";

            return command + values;
        }
        public static string UpdateEntity(BaseEntity entity)
        {
            Type type = entity.GetType();
            string command = "UPDATE " + entity.GetTableName() + " SET ";

            foreach (var property in type.GetProperties())
            {
                // שדה שאינו שייך ל-DB
                // נשתמש בזה גם עבור שדה מזהה שאין צורך להקצות לו ערך
                if (property.GetCustomAttributes(typeof(NoDBAttribute), false).Length > 0)
                    continue;

                object value = property.GetValue(entity);
                command += property.Name + " = " + PutValue(value) + " ,";
            }

            string where = "";
            foreach (var keyField in entity.GetKeyFields())
            {
                if (where != "")
                    where += " AND ";

                object value = entity.GetType().GetProperty(keyField).GetValue(entity);
                where += keyField + " = " + PutValue(value);
            }

            command = command.Substring(0, command.Length - 2) + " WHERE " + where;

            return command;
        }
        public static string DeleteEntity(BaseEntity entity)
        {
            Type type = entity.GetType();
            string command = "DELETE * FROM " + entity.GetTableName() + " WHERE ";

            string where = "";
            foreach (var keyField in entity.GetKeyFields())
            {
                if (where != "")
                    where += " AND ";

                object value = entity.GetType().GetProperty(keyField).GetValue(entity);
                where += keyField + " = " + PutValue(value);
            }

            command += where;

            return command;
        }

        private static string PutValue(object value)
        {
            if (value == null)
                return "null";
           
            if (value is string || value is DateTime)
                return "'" + value + "'";

            if(value is Color)
            {
                Color c = (Color)value;
                
                return "'" + ColorTranslator.ToHtml(c) + "'";
            }



            if (value is BaseEntity)
            {
                BaseEntity e = value as BaseEntity;
                string k = e.GetKeyFields()[0];
                value = value.GetType().GetProperty(k).GetValue(value);
            } 

            return value.ToString();
        }
    }
}
