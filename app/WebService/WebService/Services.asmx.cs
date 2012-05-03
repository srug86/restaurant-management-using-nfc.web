﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using WebServices.objects;

namespace WebServices
{
    /// <summary>
    /// Descripción breve de Service1
    /// </summary>
    [WebService(Namespace = "http://localhost/MobiCarta", Name = "MobiCartaWebServices", 
        Description = "Servicio web que gestiona la información de la aplicación MobiCarta")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Services : System.Web.Services.WebService
    {
        /* Servicios comunes */
        [WebMethod(Description = "Devuelve true si se ha invocado al método satisfactoriamente")]
        public bool connect()
        {
            return true;
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de restaurantes almacenados en la BBDD")]
        public string getRooms()
        {
            return XmlProcessor.xmlRoomsBuilder(SqlProcessor.selectAllRooms());
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la especificación de los elementos del restaurante 'name'")]
        public string getRoom(string name)
        {
            return SqlProcessor.selectRoom(name).Xml;
        }

        [WebMethod(Description = "Guarda en la BD la especificación del restaurante (contenida en 'xml')")]
        public void saveRoom(string xml)
        {
            SqlProcessor.insertRoom(XmlProcessor.xmlAddRoomDecoder(xml));
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con el estado en el que se encuentran las mesas del restaurante")]
        public string getTablesStatus()
        {
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Resetea de la BBDD los datos de la jornada anterior (estado de las mesas, pedidos y clientes)")]
        public void resetJourney(string room)
        {
            SqlProcessor.truncateTables();  // se eliminan las mesas existentes en la BD
            List<Table> tables = XmlProcessor.xmlRoomTablesDecoder(SqlProcessor.selectRoom(room).Xml);  // se cargan las mesas del comedor elegido
            foreach (Table table in tables)  // se inicializan las mesas y se guardan en la BD
            {
                table.Status = -1;
                table.Client = "";
                table.Guests = 0;
                SqlProcessor.insertTable(table);
            }
            SqlProcessor.truncateOrders();  // se eliminan todos los pedidos existentes en la BD
            SqlProcessor.deleteAnonymousClients();  // se eliminan los clientes anónimos (no usan NFC)
        }

        /* Servicios propios del recibidor */
        [WebMethod(Description = "Devuelve el estado almacenado del cliente con id 'clientID' y lo actualiza")]
        public int getClientStatus(string dni, string name, string surname)
        {
            Client client = SqlProcessor.selectClient(dni);
            if (client.Name == null)
            {
                SqlProcessor.insertClient(new Client(dni, name, surname, 0, 0));
                return -1;  // indica primer acceso de un cliente
            }
            else return client.Status;
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con el estado de las mesas después de asignar un cliente a una mesa")]
        public string setAllocationTable(string dni, int tableID, int guests)
        {
            int appearances = SqlProcessor.selectClient(dni).Appearances;
            SqlProcessor.updateClient(dni, 1, appearances++); // status (cliente) == 1 (sentado)
            SqlProcessor.updateTable(tableID, 0, dni, guests); // status (mesa) == 0 (ocupada)
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con el estado de las mesas después de que un cliente abandone una mesa")]
        public string setDeallocationTable(int tableID)
        {
            string client = SqlProcessor.selectTable(tableID).Client;
            SqlProcessor.updateClient(client, 0, -3); // status (cliente) == 0 (no está); -3 en apariciones no hace nada
            SqlProcessor.updateTable(tableID, -1, "", 0); // status (mesa) == -1 (libre)
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con el estado de las mesas después de que un cliente abandone el restaurante")]
        public string setDeallocationClient(string dni)
        {
            int tableID = SqlProcessor.selectTable(dni).Id;
            SqlProcessor.updateClient(dni, 0, -3); // status (cliente) == 0 (no está); -3 en apariciones no hace nada
            SqlProcessor.updateTable(tableID, -1, "", 0); // status (mesa) == -1 (libre)
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        /* Servicios propios de la barra */
        [WebMethod(Description = "Genera una nueva lista de productos")]
        public void saveProductsList(string xml)
        {
            List<Product> products = XmlProcessor.xmlProductsDecoder(xml);
            if (products.Count > 0) SqlProcessor.truncateProducts();
            foreach (Product product in products)
                SqlProcessor.insertProduct(product);
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de productos almacenada en la BBDD")]
        public string getProducts()
        {
            return XmlProcessor.xmlProductsBuilder(SqlProcessor.selectAllProducts());
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de pedidos almacenados en la BBDD")]
        public string getOrdersStatus()
        {
            return XmlProcessor.xmlOrdersStatusBuilder(SqlProcessor.selectAllOrders());
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con los datos de la mesa, los datos del cliente y los datos de los pedidos para esa mesa")]
        public string getTableStatus(int tableID)
        {
            string dni = SqlProcessor.selectTable(tableID).Client;
            return XmlProcessor.xmlTableStatusBuilder(SqlProcessor.selectTable(tableID), SqlProcessor.selectClient(dni), SqlProcessor.selectTableOrders(tableID));
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con el estado de las mesas después de haber cobrado la factura 'bill' (también en formato XML)")]
        public string addBillPaid(string bill)
        {
            string xml = "";
            /* */
            return xml;
        }

        [WebMethod(Description = "Añade un pedido 'order' (en formato XML) a la lista de pedidos de la BBDD")]
        public string addNewOrder(string xml)
        {
            List<Order> orders = XmlProcessor.xmlOrdersDecoder(xml);
            foreach (Order order in orders)
                SqlProcessor.insertOrder(order);
            recalculateTablesStatus();
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Cambia el estado actual del pedido 'orderID' por el nuevo estado 'status'")]
        public string setOrderStatus(int orderID, int status)
        {
            SqlProcessor.updateOrder(orderID, -3, status, -3); // -3 no modifica el argumento
            recalculateTablesStatus();
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Cambia la cantidad actual del pedido 'orderID' por la cantidad 'amount'")]
        public void setProductAmount(int orderID, int amount)
        {
            SqlProcessor.updateOrder(orderID, amount, -3, -3); // -3 no modifica el argumento
        }

        [WebMethod(Description = "Cambia la mesa actual del pedido 'orderID' por la mesa con identificador 'tableID'")]
        public string setOrderTable(int orderID, int tableID)
        {
            SqlProcessor.updateOrder(orderID, -3, -3, tableID); // -3 no modifica el argumento
            recalculateTablesStatus();
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Devuelve el identificador (int) de la mesa que ocupa el cliente con identificador 'dni'")]
        public int getTableID(string dni)
        {
            return SqlProcessor.selectTable(dni).Id;
        }

        private void recalculateTablesStatus()
        {
            foreach (Table table in SqlProcessor.selectAllTables())
            {
                if (table.Status >= 0 && table.Status < 3)
                {
                    List<Order> orders = SqlProcessor.selectTableOrders(table.Id);
                    if (orders.Count > 0)
                    {
                        table.Status = 2;
                        foreach (Order order in orders)
                            if (order.Status == 0 || order.Status == 1)
                                table.Status = 1;
                    }
                    else table.Status = 0;
                }
                SqlProcessor.updateTable(table.Id, table.Status, "", -3);
            }
        }
    }
}