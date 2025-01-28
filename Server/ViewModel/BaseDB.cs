using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewModel
{
    public abstract class BaseDB
    {
        public string connectionString { get; set; }
        public OleDbConnection connection { get; set; }
        public OleDbCommand command { get; set; }
        public OleDbDataReader reader { get; set; }

        public List<BaseEntity> lst;

        public BaseDB()
        {
            var dbFile = new FileInfo(@"..\..\..\..\DB\FulingManager.mdb");
            connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + dbFile.FullName + @"'";
            connection = new OleDbConnection(connectionString);
            command = new OleDbCommand();
            Select();
        }


        public void Select()
        {
            Open();
            Read();
            Close();
        }

        private void Open()
        {
            command.Connection = connection;

            // Open the connection to the access
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }
        private void Read()
        {
            // Write a SQL command
            command.CommandText = "select * from " + GetTableName();

            // Premorm the command - the information return in an object
            reader = command.ExecuteReader();

            lst = new List<BaseEntity>();

            while (reader.Read())
            {
                lst.Add(CreatModel());
            }


            if (reader != null)
                reader.Close();
        }
        private void Close()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }



        protected abstract BaseEntity CreatModel();
        protected abstract string GetTableName();

        public abstract void init();

        public bool SaveEntity()
        {
            Open();
            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                Read();
                init();
                Close();
                return true;
            }
            else
            {
                Close();
                return false;
            }
        }

        public bool Insert(BaseEntity entity)
        {
            command.CommandText = SQLBuilder.InsertEntity(entity);
            return SaveEntity();
        }

        public bool Update(BaseEntity entity)
        {
            command.CommandText = SQLBuilder.UpdateEntity(entity);
            return SaveEntity();
        }

        public bool Delete(BaseEntity entity)
        {
            command.CommandText = SQLBuilder.DeleteEntity(entity);
            return SaveEntity();
        }
        
    }

}

