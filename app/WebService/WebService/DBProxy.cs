using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebServices
{
    public sealed class DBProxy
    {
        static readonly DBProxy instance = new DBProxy();
        const string connectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\Sergio\Documents\pfc\web\app\WebService\WebService\App_Data\Database.mdf;Integrated Security=True;User Instance=True";
        //const string connectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Documents and Settings\Sergio\Mis documentos\pfc\web\app\WebService\WebService\App_Data\Database.mdf;Integrated Security=True;User Instance=True";

        private SqlConnection connection = null;
        private SqlDataReader data = null;

        static DBProxy() { }
        DBProxy() { }

        public static DBProxy Instance
        {
            get { return instance; }
        }

        public void connect()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void disconnect()
        {
            if (data != null) data.Close();
            connection.Close();
        }

        public void setData(string sql) // Sentencias de tipo INSERT o UPDATE
        {
            SqlCommand command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        public SqlDataReader getData(string sql)   // Sentencias de tipo SELECT
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Connection = connection;
            SqlDataReader data = command.ExecuteReader();
            return data;
        }
    }
}