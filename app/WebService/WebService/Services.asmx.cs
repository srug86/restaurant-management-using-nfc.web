using System;
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
        [WebMethod(Description = "Devuelve 'true' si se ha invocado al método satisfactoriamente")]
        public bool connect()
        {
            return true;
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de restaurantes almacenados en la BD")]
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

        [WebMethod(Description = "Devuelve el identificador (int) de la mesa que ocupa el cliente con identificador 'dni'")]
        public int getTableID(string dni)
        {
            return SqlProcessor.selectTable(dni).Id;
        }

        [WebMethod(Description = "Devuelve el histórico de pedidos de un cliente")]
        public string getClientHistory(string client)
        {
            return XmlProcessor.xmlHistoryBuilder(SqlProcessor.selectHistoricalOrders(client));
        }

        [WebMethod(Description = "Devuelve el histórico de pedidos del restaurante")]
        public string getClientsHistory()
        {
            return XmlProcessor.xmlHistoryBuilder(SqlProcessor.selectHistoricalOrders(""));
        }

        [WebMethod(Description = "Resetea de la BD los datos de la jornada anterior (estado de las mesas, pedidos y clientes)")]
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

        [WebMethod(Description = "Resetea todas las tablas de la base de datos de la aplicación")]
        public void resetDB()
        {
            SqlProcessor.truncateDB();
        }

        /* Servicios propios del recibidor */
        [WebMethod(Description = "Devuelve el estado almacenado del cliente con id 'clientID' y lo actualiza")]
        public int getClientStatus(string xmlClient)
        {
            List<Object> clientData = XmlProcessor.xmlClientDecoder(xmlClient);
            Client client = SqlProcessor.selectClient(((Client)clientData[0]).Dni);
            if (client.Name == null)
            {
                SqlProcessor.insertClient(new Client(((Client)clientData[0]).Dni, 
                    ((Client)clientData[0]).Name, ((Client)clientData[0]).Surname, 0, 0));  // se almacenan los datos personales
                SqlProcessor.insertAddress(((Client)clientData[0]).Dni, (Address)clientData[1]);    // y se almacena la dirección
                return -1;  // indica primer acceso de un cliente
            }
            else return client.Status;
        }

        [WebMethod(Description = "Devuelve el total de la factura para la mesa 'tableID'")]
        public double getBillAmount(int tableID)
        {
            return SqlProcessor.selectBillAmount(tableID);
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
            setDeallocation(client, tableID);
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con el estado de las mesas después de que un cliente abandone el restaurante")]
        public string setDeallocationClient(string dni)
        {
            int tableID = SqlProcessor.selectTable(dni).Id;
            setDeallocation(dni, tableID);
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        private void setDeallocation(string client, int tableID)
        {
            SqlProcessor.updateClient(client, 0, -3); // status (cliente) == 0 (no está); -3 en apariciones no hace nada
            SqlProcessor.updateTable(tableID, -1, "", 0); // status (mesa) == -1 (libre)
            SqlProcessor.deleteTableOrders(tableID);
        }

        [WebMethod(Description = "Inicializa los datos corporativos del restaurante")]
        public void setRestaurantInfo(string xml)
        {
            List<Object> info = XmlProcessor.xmlRestaurantDecoder(xml);
            Company c = (Company)info[0];
            Address a = (Address)info[1];
            double iva = (Double)info[2];
            double discount = (Double)info[3];
            SqlProcessor.insertRestaurant(c, iva, discount);
            SqlProcessor.insertAddress(c.NIF, a);
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

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de productos almacenada en la BD")]
        public string getProducts()
        {
            return XmlProcessor.xmlProductsBuilder(SqlProcessor.selectAllProducts());
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de pedidos almacenados en la BD")]
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

        [WebMethod(Description = "Añade un pedido (en formato XML) a la lista de pedidos de la BD")]
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

        [WebMethod(Description = "Calcula la factura de una mesa y la devuelve en formato XML")]
        public string getBill(int tableID)
        {
            string xmlBill = SqlProcessor.selectBillTable(tableID);
            if (xmlBill == "")
            {
                Bill bill = generateBill(tableID);
                SqlProcessor.increaseNBill();
                xmlBill = XmlProcessor.xmlBillBuilder(bill);
                SqlProcessor.insertBill(bill, xmlBill);
            }
            return xmlBill;
        }

        [WebMethod(Description = "Marca la factura y todos sus pedidos como 'pagados' y devuelve el estado actual de las mesas")]
        public string payBill(int billID, int type)
        {
            SqlProcessor.updateBillStatus(billID, type);
            int tableID = SqlProcessor.selectTableBill(billID);
            string client = SqlProcessor.selectTable(tableID).Client;
            SqlProcessor.updateClient(client, 2, -3);
            foreach (Order o in SqlProcessor.selectTableOrders(tableID))
            {
                SqlProcessor.updateOrder(o.Id, -3, 3, -3);
                SqlProcessor.insertHistoricalOrder(client, o);  // los pedidos son añadidos al histórico
            }
            SqlProcessor.updateTable(tableID, 3, "", -3);
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
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

        private Bill generateBill(int tableID)
        {
            Bill bill = new Bill();
            bill.CompanyInfo = SqlProcessor.selectRestaurant();
            bill.CompanyAddress = SqlProcessor.selectAddress(bill.CompanyInfo.NIF);
            Table table = SqlProcessor.selectTable(tableID);
            bill.ClientInfo = SqlProcessor.selectClient(table.Client);
            bill.ClientAddress = SqlProcessor.selectAddress(bill.ClientInfo.Dni);
            bill.Date = DateTime.Today;
            bill.TableID = tableID;
            bill.Iva = SqlProcessor.selectIVA();
            bill.Discount = SqlProcessor.selectDiscount();
            bill.Id = SqlProcessor.selectNBill();
            bill.Serial = SqlProcessor.selectSerial();
            List<Order> orders = SqlProcessor.selectTableOrders(tableID);
            foreach (Order order in orders)
            {
                OrderPrice oPrice = new OrderPrice();
                Product product = SqlProcessor.selectProduct(order.Product);
                oPrice.Order = order;
                oPrice.Price = product.Price;
                oPrice.Iva = bill.Iva;
                oPrice.Discount = ((int)(order.Amount / product.DiscountedUnit)) * product.Discount * product.Price;
                oPrice.Total = ((product.Price * order.Amount) * (1 - oPrice.Discount));// * (1 + (oPrice.Iva / 100));
                bill.Orders.Add(oPrice);
                bill.Total += oPrice.Total;
                bill.TaxBase += oPrice.Price * order.Amount - oPrice.Discount;
            }
            bill.Quote = bill.Total - bill.TaxBase;
            return bill;
        }
    }
}