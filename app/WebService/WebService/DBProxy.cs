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

        // Cadena de conexión a la BD MySQL
        const string connectionString = "server=161.67.140.37;database=mobicarta;uid=sergio.rubia;pwd=sergiodlr2012";

        private MySqlConnection connection = null;
        private MySqlDataReader data = null;

        /* Implementación de un 'Singleton' para esta clase */
        static DBProxy() { }

        DBProxy() { }

        public static DBProxy Instance
        {
            get { return instance; }
        }

        // Abre una nueva conexión con la BD
        public void connect()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        // Cierra una conexión existente
        public void disconnect()
        {
            if (data != null) data.Close();
            connection.Close();
        }

        // Realiza una consulta que modifica la BD
        public void setData(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        // Realiza una consulta que devuelve resultados
        public MySqlDataReader getData(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader data = command.ExecuteReader();
            return data;
        }
    }
}