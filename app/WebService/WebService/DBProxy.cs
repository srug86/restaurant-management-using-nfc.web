using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebServices
{
    public sealed class DBProxy
    {
        static readonly DBProxy instance = new DBProxy();

        //const string connectionString = "server=161.67.140.37:3500;database=mobicarta;uid=sergio.rubia;pwd=sergiodlr2012";
        const string connectionString = "server=192.168.1.36;database=MobiCarta;uid=root;pwd=root";

        private MySqlConnection connection = null;
        private MySqlDataReader data = null;

        static DBProxy() { }
        DBProxy() { }

        public static DBProxy Instance
        {
            get { return instance; }
        }

        public void connect()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        public void disconnect()
        {
            if (data != null) data.Close();
            connection.Close();
        }

        public void setData(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        public MySqlDataReader getData(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader data = command.ExecuteReader();
            return data;
        }
    }
}