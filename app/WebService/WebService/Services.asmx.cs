﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using WebServices.objects;

namespace WebServices
{
    [WebService(Namespace = "http://localhost/MobiCarta", Name = "MobiCartaWebServices", 
        Description = "Servicio web que gestiona la información de la aplicación MobiCarta")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Services : System.Web.Services.WebService
    {
        /* Servicios comunes */
        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de restaurantes almacenados en la BD")]
        public string getRooms()
        {
            return XmlProcessor.xmlRoomsBuilder(SqlProcessor.selectAllRooms());
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la especificación de la plantilla de la jornada actual")]
        public string getCurrentRoom()
        {
            string name = SqlProcessor.selectRestaurantRoom();
            if (!name.Equals("none") && !name.Equals(""))
            {
                Room room = SqlProcessor.selectRoom(name);
                List<Room> rooms = new List<Room>();
                if (!room.Name.Equals("")) rooms.Add(room);
                return XmlProcessor.xmlRoomsBuilder(rooms);
            }
            return "";
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la especificación de los elementos del restaurante 'name'")]
        public string getRoom(string name, bool save)
        {
            Room room = SqlProcessor.selectRoom(name);
            if (save) SqlProcessor.saveCurrentRoom(room.Name);
            return room.Xml;
        }

        [WebMethod(Description = "Guarda en la BD la especificación del restaurante (contenida en 'xml')")]
        public void saveRoom(string name, string xml)
        {
            Room room = SqlProcessor.selectRoom(name);
            if (!room.Name.Equals("")) SqlProcessor.deleteRoom(name);
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

        [WebMethod(Description = "Devuelve una cadena en formato XML con el histórico de pedidos de un cliente")]
        public string getClientHistory(string client)
        {
            return XmlProcessor.xmlHistoryBuilder(SqlProcessor.selectHistoricalOrders(client));
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con el histórico de pedidos del restaurante")]
        public string getClientsHistory()
        {
            return XmlProcessor.xmlHistoryBuilder(SqlProcessor.selectHistoricalOrders(""));
        }

        [WebMethod(Description = "'Resetea' de la BD los datos de la jornada anterior (estado de las mesas, pedidos y clientes)")]
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
            SqlProcessor.deleteUnpaidBills(); // se eliminan las facturas generadas pero no pagadas
            SqlProcessor.deleteAnonymousClients();  // se eliminan los clientes anónimos (no usan NFC)
            SqlProcessor.resetClientsStatus();  // se cambia el estado de los demás clientes a 0 ('no está en el restaurante')
        }

        [WebMethod(Description = "'Resetea' todas las tablas de la base de datos de la aplicación")]
        public void resetDB()
        {
            SqlProcessor.truncateDB();
        }

        [WebMethod(Description = "Marca la factura y todos sus pedidos como 'pagados' y devuelve el estado actual de las mesas")]
        public string payBill(int billID, int type)
        {
            int tableID = SqlProcessor.selectTableBill(billID);
            string client = SqlProcessor.selectTable(tableID).Client;
            foreach (Order o in SqlProcessor.selectTableOrders(tableID, false))
                SqlProcessor.updateOrder(o.Id, -3, 3, -3);
            SqlProcessor.updateClient(client, 2, -3);
            SqlProcessor.updateTable(tableID, 3, "", -3);
            SqlProcessor.updateBillStatus(billID, type);
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        /* Servicios propios del recibidor */
        [WebMethod(Description = "Devuelve una cadena en formato XML con la información del cliente 'dni'")]
        public string getClient(string dni)
        {
            return XmlProcessor.xmlClientDataBuilder(SqlProcessor.selectClient(dni), SqlProcessor.selectAddress(dni));
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de clientes del restaurante")]
        public string getClients()
        {
            return XmlProcessor.xmlClientsDataBuilder(SqlProcessor.selectClients());
        }

        [WebMethod(Description = "Devuelve el estado almacenado del cliente con id 'clientID' y lo actualiza")]
        public int getClientStatus(string xmlClient)
        {
            List<Object> clientData = XmlProcessor.xmlClientDecoder(xmlClient);
            Client client = SqlProcessor.selectClient(((Client)clientData[0]).Dni);
            if (client.Name == null)    // si el cliente no existe, se crea
            {
                SqlProcessor.insertClient(new Client(((Client)clientData[0]).Dni, 
                    ((Client)clientData[0]).Name, ((Client)clientData[0]).Surname, 0, 0));  // se almacenan los datos personales
                SqlProcessor.insertAddress(((Client)clientData[0]).Dni, (Address)clientData[1]);    // y se almacena la dirección
                return -1;  // indica primer acceso de un cliente
            }
            else return client.Status;
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con las recomendaciones calculadas por el módulo recomendador para un cliente")]
        public string getRecommendation(string client)
        {
            string restaurant = SqlProcessor.selectRestaurant().Name;
            List<HistoricalOrder> clientHistory = SqlProcessor.selectHistoricalOrders(client);  // selecciona el historial de pedidos del cliente
            List<Product> products = SqlProcessor.selectAllProducts(false); // selecciona sólo los productos visibles
            int appearances = SqlProcessor.selectClient(client).Appearances;
            double discount = SqlProcessor.selectDiscount();
            int discountedV = SqlProcessor.selectDiscountedVisit();
            Recommendation rec = Recommender.generateRecommendation(products, clientHistory, restaurant, appearances, discount, discountedV);
            return XmlProcessor.xmlRecommendationBuilder(rec);
        }

        [WebMethod(Description = "Devuelve el importe total de la factura para la mesa 'tableID'")]
        public double getBillAmount(int tableID)
        {
            getBill(tableID, false);    // Se genera la factura
            return SqlProcessor.selectBillAmount(tableID);
        }

        [WebMethod(Description = "Devuelve el identificador de la factura para la mesa 'tableID'")]
        public int getBillID(int tableID)
        {
            return SqlProcessor.selectBillID(tableID);
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con el estado de las mesas después de asignar un cliente a una mesa")]
        public string setAllocationTable(string dni, int tableID, int guests)
        {
            int appearances = SqlProcessor.selectClient(dni).Appearances;
            SqlProcessor.updateClient(dni, 1, ++appearances); // status (cliente) == 1 (sentado)
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
            List<Order> orders = SqlProcessor.selectTableOrders(tableID, true);
            foreach (Order o in orders)
                SqlProcessor.insertHistoricalOrder(client, o);  // los pedidos son añadidos al histórico
            SqlProcessor.updateClient(client, 0, -3); // status (cliente) == 0 (no está); -3 en apariciones no hace nada
            SqlProcessor.updateTable(tableID, -1, "", 0); // status (mesa) == -1 (libre)
            SqlProcessor.deleteTableOrders(tableID);
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con los datos corporativos del restaurante")]
        public string getRestaurant()
        {
            List<Object> loo = new List<Object>();
            Company company = SqlProcessor.selectRestaurant();
            loo.Add(company);
            loo.Add(SqlProcessor.selectAddress(company.NIF));
            loo.Add(SqlProcessor.selectIVA());
            loo.Add(SqlProcessor.selectDiscount());
            loo.Add(SqlProcessor.selectDiscountedVisit());
            return XmlProcessor.xmlRestaurantDataBuilder(loo);
        }

        [WebMethod(Description = "Inicializa los datos corporativos del restaurante")]
        public void saveRestaurant(string xml)
        {
            List<Object> info = XmlProcessor.xmlRestaurantDecoder(xml);
            Company c = (Company)info[0];
            Address a = (Address)info[1];
            double iva = (Double)info[2];
            double discount = (Double)info[3];
            int discountedVisit = (Int32)info[4];
            SqlProcessor.insertRestaurant(c, SqlProcessor.selectNBill(), iva, discount, discountedVisit, SqlProcessor.selectRestaurantRoom());
            SqlProcessor.insertAddress(c.NIF, a);
        }

        /* Servicios propios de la barra */
        [WebMethod(Description = "Genera una nueva lista de productos a partir de un XML")]
        public void saveProductsList(string xml)
        {
            List<Product> products = XmlProcessor.xmlProductsDecoder(xml);
            if (products.Count > 0) SqlProcessor.truncateProducts();
            foreach (Product product in products)
                SqlProcessor.insertProduct(product);
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de productos almacenada en la BD")]
        public string getProducts(bool nonVisible)
        {
            return XmlProcessor.xmlProductsBuilder(SqlProcessor.selectAllProducts(nonVisible));
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista de facturas almacenada en la BD")]
        public string getBills(int amount, bool ascending)
        {
            return XmlProcessor.xmlBillsBuilder(SqlProcessor.selectBills(amount, ascending));
        }

        [WebMethod(Description = "Devuelve una cadena en formato XML con la lista del historial de pedidos almacenada en la BD")]
        public string getHOrders(int amount, bool ascending)
        {
            return XmlProcessor.xmlHistoryBuilder(SqlProcessor.selectHOrders(amount, ascending));
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
            return XmlProcessor.xmlTableStatusBuilder(SqlProcessor.selectTable(tableID), SqlProcessor.selectClient(dni), SqlProcessor.selectTableOrders(tableID, true));
        }

        [WebMethod(Description = "Añade un pedido (en formato XML) a la lista de pedidos de la BD y devuelve una cadena XML con el estado actualizado de las mesas")]
        public string addNewOrder(string xml)
        {
            List<Order> orders = XmlProcessor.xmlOrdersDecoder(xml);
            foreach (Order order in orders)
                SqlProcessor.insertOrder(order);
            recalculateTablesStatus();  // recalcula el nuevo estado de las mesas del restaurante
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Cambia el estado actual del pedido 'orderID' por el nuevo estado 'status' y devuelve una cadena XML con el estado actualizado de las mesas")]
        public string setOrderStatus(int orderID, int status)
        {
            SqlProcessor.updateOrder(orderID, -3, status, -3); // -3 no modifica el argumento
            recalculateTablesStatus();  // recalcula el nuevo estado de las mesas del restaurante
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Cambia la cantidad actual del pedido 'orderID' por la cantidad 'amount'")]
        public void setProductAmount(int orderID, int amount)
        {
            SqlProcessor.updateOrder(orderID, amount, -3, -3); // -3 no modifica el argumento
        }

        [WebMethod(Description = "Cambia la mesa actual del pedido 'orderID' por la mesa con identificador 'tableID' y devuelve una cadena XML con el estado actualizado de las mesas")]
        public string setOrderTable(int orderID, int tableID)
        {
            SqlProcessor.updateOrder(orderID, -3, -3, tableID); // -3 no modifica el argumento
            recalculateTablesStatus();  // recalcula el nuevo estado de las mesas del restaurante
            return XmlProcessor.xmlTablesStatusBuilder(SqlProcessor.selectAllTables());
        }

        [WebMethod(Description = "Devuelve la factura con identificador 'billID' en formato XML")]
        public string getStaticBill(int billID)
        {
            return SqlProcessor.selectBill(billID);
        }

        [WebMethod(Description = "Calcula la factura de una mesa y la devuelve en formato XML")]
        public string getBill(int tableID, bool _short)
        {
            if (_short) return XmlProcessor.xmlShortBillBuilder(generateBill(tableID));
            string xmlBill = SqlProcessor.selectBillTable(tableID); // busca la factura no pagada de la mesa 'tableID'
            if (!xmlBill.Equals("")) return xmlBill;    // si la hay, la devuelve
            Bill bill = generateBill(tableID);  // si no, la calcula
            xmlBill = SqlProcessor.selectBill(bill.Id); // mira si está con las facturas pagadas
            if (!xmlBill.Equals("")) return xmlBill;    // si la encuentra, la devuelve
            xmlBill = XmlProcessor.xmlBillBuilder(bill);    // si no, genera el xml
            if (bill.Total > 0) // Para evitar almacenar basura
            {
                SqlProcessor.insertBill(bill, xmlBill);     // y lo almacena con el resto de facturas
                SqlProcessor.increaseNBill();   // incrementa el número de serie de las facturas
            }
            return xmlBill; // y devuelve el xml de la factura calculada
        }

        // Actualiza el estado de las mesas del restaurante
        private void recalculateTablesStatus()
        {
            foreach (Table table in SqlProcessor.selectAllTables())
            {
                if (table.Status >= 0)
                {
                    List<Order> orders = SqlProcessor.selectTableOrders(table.Id, true);
                    if (orders.Count > 0)
                    {
                        table.Status = 3;
                        foreach (Order order in orders)
                        {
                            if (order.Status == 0 || order.Status == 1)
                            {
                                table.Status = 1;
                                break;
                            }
                            else if (order.Status == 2)
                                table.Status = 2;
                        }
                    }
                    else table.Status = 0;
                }
                SqlProcessor.updateTable(table.Id, table.Status, "", -3);
            }
        }

        // Genera la factura de la mesa 'tableID'
        private Bill generateBill(int tableID)
        {
            Bill bill = new Bill();
            bill.CompanyInfo = SqlProcessor.selectRestaurant();
            bill.CompanyAddress = SqlProcessor.selectAddress(bill.CompanyInfo.NIF);
            Table table = SqlProcessor.selectTable(tableID);
            bill.ClientInfo = SqlProcessor.selectClient(table.Client);
            bill.ClientAddress = SqlProcessor.selectAddress(bill.ClientInfo.Dni);
            bill.Date = DateTime.Now;
            bill.TableID = tableID;
            bill.Iva = SqlProcessor.selectIVA();
            int dVisit = SqlProcessor.selectDiscountedVisit();
            if (bill.ClientInfo.Appearances % dVisit == 0 && bill.ClientInfo.Appearances > 0)   // descuento por fidelidad
                bill.Discount = SqlProcessor.selectDiscount();
            else bill.Discount = 0.0;
            bill.Id = SqlProcessor.selectNBill();
            bill.Serial = SqlProcessor.selectSerial();
            bill.Paid = 0;
            List<Order> orders = SqlProcessor.selectTableOrders(tableID, true);
            foreach (Order order in orders)
            {
                OrderPrice oPrice = new OrderPrice();
                Product product = SqlProcessor.selectProduct(order.Product);
                oPrice.Order = order;
                oPrice.Price = product.Price;
                if (product.DiscountedUnit > 0) // descuento por producto
                    oPrice.Discount = ((int)(order.Amount / product.DiscountedUnit)) * product.Discount * product.Price;
                else
                    oPrice.Discount = 0;
                oPrice.Total = (product.Price * order.Amount) - oPrice.Discount;
                bill.Orders.Add(oPrice);
                bill.Subtotal += oPrice.Total;
            }
            bill.TaxBase = bill.Subtotal - bill.Subtotal * (bill.Discount / 100);
            bill.Total = bill.TaxBase + bill.TaxBase * (bill.Iva / 100);
            bill.Quote = bill.Total - bill.TaxBase;
            return bill;
        }
    }
}