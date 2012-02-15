using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Receiver.domain;
using System.Data.SqlClient;

namespace WebService
{
    /// <summary>
    /// Descripción breve de Service1
    /// </summary>
    [WebService(Namespace = "http://localhost/MobiCarta")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {
        private DBProxy db = DBProxy.Instance;

        [WebMethod(Description = "Devuelve el estado almacenado del cliente con id idClient y lo actualiza")]
        public int clientStatus (string idClient)
        {
            int status = -1;
            string sentence = "";
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Clients WHERE idClient = '" + idClient + "'");
            if (data == null)   // Si no existe el cliente se inserta en la base de datos
                sentence = "INSERT INTO Clients (idClient, status, appearances) VALUES('" + idClient + "',1,1)";
            else
            {
                while (data.Read())
                {
                    switch (data.GetOrdinal("status"))
                    {
                        case 0: // El cliente acaba de entrar al restaurante
                            int appearances = data.GetOrdinal("appearances");
                            sentence = "UPDATE Clients SET status = 1, appearances = '" + (appearances++) + "' WHERE idClient = '" + idClient + "'";
                            status = 0;
                            break;
                        case 1: // El cliente está en el restaurante y no ha pagado
                            status = 1;
                            break;
                        case 2: // El cliente ha pagado y se marcha
                            status = 2;
                            break;
                        default: break;
                    }
                }
            }
            if (status != 1) db.setData(sentence);
            db.disconnect();
            return status;
        }

        /*[WebService]
        public Client getClient(string idClient)
        {
            Client client = new Client("null", null);
            SqlDataReader data = db.getData("SELECT * FROM Clients WHERE idClient = '" + idClient + "'");
            if (data != null)
                while (data.Read())
                {
                    client.Id = idClient;
                    client.Paid = true;
                    client.Total = data.GetOrdinal("appearances");
                }
            return client;
        }*/
    }
}