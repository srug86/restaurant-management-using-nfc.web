using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebServices
{
    public sealed class MySQLProxy
    {
        //static readonly MySQLProxy instance = new MySQLProxy();

        private MySqlCommand command;
        private MySqlDataAdapter adapter;
        private DataSet dataSet;

        private string server, db, pwd;

        static MySqlProxy() { }
        MySqlProxy() { }

        public static MySqlProxy Instance
        {
            get { return instance; }
        }



    }
}