using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServices.objects;
using System.Data.SqlClient;

namespace WebServices
{
    class SqlProcessor
    {
        private static DBProxy db = DBProxy.Instance;

        /* Acceso a la tabla de comedores 'Rooms' */
        public static void insertRoom(Room room)
        {
            db.connect();
            db.setData("INSERT INTO Rooms (name, width, height, tables, capacity, xml) VALUES ('" +
                room.Name + "', " + room.Width + ", " + room.Height + ", " + room.Tables + ", " +
                room.Capacity + ", '" + room.Xml + "')");
            db.disconnect();
        }

        public static Room selectRoom(string name)
        {
            Room room = new Room();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Rooms WHERE name = '" + name + "'");
            while (data.Read())
            {
                room.Name = data.GetString(0).Trim();
                room.Width = data.GetInt16(1);
                room.Height = data.GetInt16(2);
                room.Tables = data.GetInt16(3);
                room.Capacity = data.GetInt16(4);
                room.Xml = data.GetString(5).Trim();
            }
            db.disconnect();
            return room;
        }

        public static List<Room> selectAllRooms()
        {
            List<Room> rooms = new List<Room>();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Rooms");
            while (data.Read())
            {
                Room room = new Room();
                room.Name = data.GetString(0);
                room.Width = data.GetInt16(1);
                room.Height = data.GetInt16(2);
                room.Tables = data.GetInt16(3);
                room.Capacity = data.GetInt16(4);
                room.Xml = data.GetString(5);
                rooms.Add(room);
            }
            db.disconnect();
            return rooms;
        }

        /* Acceso a la tabla de productos 'Products' */
        public static void insertProduct(Product product)
        {
            db.connect();
            db.setData("INSERT INTO Products (name, category, price, description, visible, discount, discountedUnit) VALUES ('" +
                product.Name + "', '" + product.Category + "', '" + product.Price + "', '" + 
                product.Description + "', " + Convert.ToInt16(product.Visible) + ", '" +
                product.Discount + "', " + product.DiscountedUnit + ")");
            db.disconnect();
        }

        public static Product selectProduct(string name)
        {
            Product product = new Product();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Products WHERE name = '" + name + "'");
            while (data.Read())
            {
                product.Name = data.GetString(0).Trim();
                product.Category = data.GetString(1).Trim();
                product.Price = Convert.ToDouble(data.GetString(2));
                product.Description = data.GetString(3).Trim();
                product.Visible = data.GetBoolean(4);
                product.Discount = Convert.ToDouble(data.GetString(5));
                product.DiscountedUnit = data.GetInt16(6);
            }
            db.disconnect();
            return product;
        }

        public static List<Product> selectAllProducts(bool nonVisible)
        {
            List<Product> products = new List<Product>();
            db.connect();
            SqlDataReader data;
            if (nonVisible)
                data = db.getData("SELECT * FROM Products");
            else
                data = db.getData("SELECT * FROM Products WHERE visible = 1");
            while (data.Read())
            {
                Product product = new Product();
                product.Name = data.GetString(0).Trim();
                product.Category = data.GetString(1).Trim();
                product.Price = Convert.ToDouble(data.GetString(2));
                product.Description = data.GetString(3).Trim();
                product.Visible = data.GetBoolean(4);
                product.Discount = Convert.ToDouble(data.GetString(5));
                product.DiscountedUnit = data.GetInt16(6);
                products.Add(product);
            }
            db.disconnect();
            return products;
        }

        public static void truncateProducts()
        {
            db.connect();
            db.setData("TRUNCATE TABLE Products");
            db.disconnect();
        }

        /* Acceso a la tabla de pedidos 'Orders' */
        public static void insertOrder(Order order)
        {
            db.connect();
            db.setData("INSERT INTO Orders (Id, tableID, product, amount, status, date) VALUES (" +
                order.Id + "," + order.TableID + ", '" + order.Product + "', " + order.Amount + ", " + 
                order.Status + ", '" + order.Date.ToString() + "')");
            db.disconnect();
        }

        public static void updateOrder(int orderID, int amount, int status, int tableID)
        {
            db.connect();
            if (amount != -3)   // hay actualización de cantidad
                db.setData("UPDATE Orders SET amount = " + amount + " WHERE Id = " + orderID);
            if (status != -3)   // hay actualización de estado
            {
                if (status == -2)
                    db.setData("DELETE FROM Orders WHERE Id = " + orderID);
                else
                    db.setData("UPDATE Orders SET status = " + status + " WHERE Id = " + orderID);
            }
            if (tableID != -3)
                db.setData("UPDATE Orders SET tableID = " + tableID + " WHERE Id = " + orderID);
            db.disconnect();
        }

        public static List<Order> selectTableOrders(int tableID)
        {
            List<Order> orders = new List<Order>();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Orders WHERE tableID = " + tableID);
            while (data.Read())
            {
                Order order = new Order();
                order.Id = data.GetInt16(0);
                order.TableID = data.GetInt16(1);
                order.Product = data.GetString(2).Trim();
                order.Amount = data.GetInt16(3);
                order.Status = data.GetInt16(4);
                order.Date = Convert.ToDateTime(data.GetSqlDateTime(5).ToString());
                orders.Add(order);
            }
            db.disconnect();
            return orders;
        }

        public static List<Order> selectAllOrders()
        {
            List<Order> orders = new List<Order>();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Orders");
            while (data.Read())
            {
                Order order = new Order();
                order.Id = data.GetInt16(0);
                order.TableID = data.GetInt16(1);
                order.Product = data.GetString(2).Trim();
                order.Amount = data.GetInt16(3);
                order.Status = data.GetInt16(4);
                order.Date = Convert.ToDateTime(data.GetSqlDateTime(5).ToString());
                orders.Add(order);
            }
            db.disconnect();
            return orders;
        }

        public static void deleteTableOrders(int tableID)
        {
            db.connect();
            db.setData("DELETE FROM Orders WHERE tableID = " + tableID);
            db.disconnect();
        }

        public static void truncateOrders()
        {
            db.connect();
            db.setData("TRUNCATE TABLE Orders");
            db.disconnect();
        }

        /* Acceso a la tabla de clientes 'Clients' */
        public static void insertClient(Client client)
        {
            db.connect();
            db.setData("INSERT INTO Clients (dni, name, surname, status, appearances) VALUES ('" +
                client.Dni + "', '" + client.Name + "', '" + client.Surname + "', " + client.Status + 
                ", " + client.Appearances + ")");
            db.disconnect();
        }

        public static void updateClient(string dni, int status, int appearances)
        {
            db.connect();
            if (status != -3)
                db.setData("UPDATE Clients SET status = " + status + " WHERE dni = '" + dni + "'");
            if (appearances != -3)
                db.setData("UPDATE Clients SET appearances = " + appearances + " WHERE dni = '" + dni + "'");
            db.disconnect();
        }

        public static Client selectClient(string dni)
        {
            Client client = new Client();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Clients WHERE dni = '" + dni + "'");
            while (data.Read())
            {
                client.Dni = data.GetString(0).Trim();
                client.Name = data.GetString(1).Trim();
                client.Surname = data.GetString(2).Trim();
                client.Status = data.GetInt16(3);
                client.Appearances = data.GetInt16(4);
            }
            db.disconnect();
            return client;
        }

        public static void deleteAnonymousClients()
        {
            db.connect();
            db.setData("DELETE FROM Clients WHERE name = ''");
            db.disconnect();
        }

        public static void resetClientsStatus()
        {
            db.connect();
            db.setData("UPDATE Clients SET status = 0");
            db.disconnect();
        }

        /* Acceso a la tabla de mesas 'Tables' */
        public static void insertTable(Table table)
        {
            db.connect();
            db.setData("INSERT INTO Tables (Id, status, capacity, client, guests) VALUES (" +
                table.Id + ", " + table.Status + ", " + table.Capacity + ", '" + table.Client + 
                "', " + table.Guests + ")");
            db.disconnect();
        }

        public static void updateTable(int tableID, int status, string client, int guests)
        {
            db.connect();
            if (status != -3)
                db.setData("UPDATE Tables SET status = " + status + " WHERE Id = " + tableID);
            if (client != "")
                db.setData("UPDATE Tables SET client = '" + client + "' WHERE Id = " + tableID);
            if (guests != -3)
                db.setData("UPDATE Tables SET guests = " + guests + " WHERE Id = " + tableID);
            db.disconnect();
        }

        public static Table selectTable(int tableID)
        {
            db.connect();
            Table table = selectTableAux("SELECT * FROM Tables WHERE Id = " + tableID);
            db.disconnect();
            return table;
        }

        public static Table selectTable(string client)
        {
            db.connect();
            Table table = selectTableAux("SELECT * FROM Tables WHERE client = '" + client + "'");
            db.disconnect();
            return table;
        }

        private static Table selectTableAux(string sql)
        {
            Table table = new Table();
            db.connect();
            SqlDataReader data = db.getData(sql);
            while (data.Read())
            {
                table.Id = data.GetInt16(0);
                table.Status = data.GetInt16(1);
                table.Capacity = data.GetInt16(2);
                table.Client = data.GetString(3).Trim();
                table.Guests = data.GetInt16(4);
            }
            db.disconnect();
            return table;
        }

        public static List<Table> selectAllTables()
        {
            List<Table> tables = new List<Table>();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Tables");
            while (data.Read())
            {
                Table table = new Table();
                table.Id = data.GetInt16(0);
                table.Status = data.GetInt16(1);
                table.Capacity = data.GetInt16(2);
                table.Client = data.GetString(3).Trim();
                table.Guests = data.GetInt16(4);
                tables.Add(table);
            }
            db.disconnect();
            return tables;
        }

        public static void truncateTables()
        {
            db.connect();
            db.setData("TRUNCATE TABLE Tables");
            db.disconnect();
        }

        /* Acceso a la tabla de facturas 'Bills' */
        public static int selectTableBill(int billID)
        {
            int tableID = 0;
            db.connect();
            SqlDataReader data = db.getData("SELECT tableID FROM Bills WHERE Id = " + billID);
            while (data.Read())
                tableID = data.GetInt16(0);
            db.disconnect();
            return tableID;
        }

        public static string selectBillTable(int tableID)
        {
            string xml = "";
            db.connect();
            SqlDataReader data = db.getData("SELECT xml FROM Bills WHERE tableID = " + tableID + " AND paid = 0");
            while (data.Read())
                xml = data.GetString(0).Trim();
            db.disconnect();
            return xml;
        }

        public static double selectBillAmount(int tableID)
        {
            double amount = 0.0;
            db.connect();
            SqlDataReader data = db.getData("SELECT total FROM Bills WHERE tableID = " + tableID);
            while (data.Read())
                amount = Convert.ToDouble(data.GetString(0).Trim());
            return amount;
        }

        public static void insertBill(Bill bill, string xml)
        {
            db.connect();
            db.setData("INSERT INTO Bills (Id, tableID, client, date, total, paid, xml) VALUES (" + 
                bill.Id + ", " + bill.TableID + ", '" + bill.ClientInfo.Dni + "', '" + bill.Date.ToString() + 
                "', '" + bill.Total + "', " + bill.Paid + ", '" + xml + "')");
            db.disconnect();
        }

        public static void updateBillStatus(int billID, int type)
        {
            db.connect();
            db.setData("UPDATE Bills SET paid = " + type + " WHERE Id = " + billID);
            db.disconnect();
        }

        /* Acceso a la tabla de direcciones 'Address' */
        public static void insertAddress(string nif, Address address)
        {
            db.connect();
            db.setData("INSERT INTO Address (nif, street, house, zipCode, town, state) VALUES ('" +
                nif + "', '" + address.Street + "', '" + address.Number + "', " + address.ZipCode +
                ", '" + address.Town + "', '" + address.Street + "')");
            db.disconnect();
        }

        public static Address selectAddress(string nif)
        {
            Address address = new Address();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Address WHERE nif = '" + nif + "'");
            while (data.Read())
            {
                address.Street = data.GetString(1).Trim();
                address.Number = data.GetString(2).Trim();
                address.ZipCode = data.GetInt32(3);
                address.Town = data.GetString(4).Trim();
                address.State = data.GetString(5).Trim();
            }
            db.disconnect();
            return address;
        }

        /* Acceso a la tabla de restaurantes 'Restaurants' */
        public static void insertRestaurant(Company company, double iva, double discount, int discountedVisit)
        {
            db.connect();
            db.setData("TRUNCATE TABLE Restaurants");
            db.setData("DELETE FROM Address WHERE nif = '" + company.NIF + "'");
            db.setData("INSERT INTO Restaurants (nif, name, phone, fax, mail, iva, discount, discountedVisit, nBill, serial, currentRoom) VALUES ('" +
                company.NIF + "', '" + company.Name + "', " + company.Phone + ", " + company.Fax +
                ", '" + company.Email + "', '" + iva + "', '" + discount + "', " + discountedVisit + ", 1, 1, 'none')");
            db.disconnect();
        }

        public static Company selectRestaurant()
        {
            Company company = new Company();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Restaurants");
            while (data.Read())
            {
                company.NIF = data.GetString(0).Trim();
                company.Name = data.GetString(1).Trim();
                company.Phone = data.GetInt32(2);
                company.Fax = data.GetInt32(3);
                company.Email = data.GetString(4).Trim();
            }
            db.disconnect();
            return company;
        }

        public static double selectIVA()
        {
            double iva = 0.0;
            db.connect();
            SqlDataReader data = db.getData("SELECT iva FROM Restaurants");
            while (data.Read())
                iva = Convert.ToDouble(data.GetString(0));
            db.disconnect();
            return iva;
        }

        public static int selectDiscountedVisit()
        {
            int visits = 0;
            db.connect();
            SqlDataReader data = db.getData("SELECT discountedVisit FROM Restaurants");
            while (data.Read())
                visits = data.GetInt16(0);
            db.disconnect();
            return visits;
        }

        public static double selectDiscount()
        {
            double discount = 0.0;
            db.connect();
            SqlDataReader data = db.getData("SELECT discount FROM Restaurants");
            while (data.Read())
                discount = Convert.ToDouble(data.GetString(0));
            db.disconnect();
            return discount;
        }

        public static int selectNBill()
        {
            int nBill = 1;
            db.connect();
            SqlDataReader data = db.getData("SELECT nBill FROM Restaurants");
            while (data.Read())
                nBill = data.GetInt32(0);
            return nBill;
        }

        public static void increaseNBill()
        {
            db.connect();
            db.setData("UPDATE Restaurants SET nBill = nBill + 1");
            db.disconnect();
        }

        public static int selectSerial()
        {
            int serial = 1;
            db.connect();
            SqlDataReader data = db.getData("SELECT serial FROM Restaurants");
            while (data.Read())
                serial = data.GetInt32(0);
            return serial;
        }

        public static void increaseSerial()
        {
            db.connect();
            db.setData("UPDATE Restaurants SET serial = serial + 1");
            db.disconnect();
        }

        public static string selectRestaurantRoom()
        {
            string room = "none";
            db.connect();
            SqlDataReader data = db.getData("SELECT currentRoom FROM Restaurants");
            while (data.Read())
                room = data.GetString(0).Trim();
            db.disconnect();
            return room;
        }

        public static void saveCurrentRoom(string room)
        {
            db.connect();
            db.setData("UPDATE Restaurants SET currentRoom = '" + room + "'");
            db.disconnect();
        }

        /* Acceso a la tabla del historial de pedidos 'Historical' */
        public static void insertHistoricalOrder(string client, Order order)
        {
            db.connect();
            db.setData("INSERT INTO Historical (id, client, product, amount, date) VALUES (NEWID(), '" + client + 
                "', '" + order.Product + "', " + order.Amount + ", '" + order.Date.ToString() + "')");
            db.disconnect();
        }

        public static List<HistoricalOrder> selectHistoricalOrders(string client)
        {
            List<HistoricalOrder> orders = new List<HistoricalOrder>();
            db.connect();
            SqlDataReader data;
            if (client.Equals(""))
                data = db.getData("SELECT * FROM Historical");
            else
                data = db.getData("SELECT * FROM Historical WHERE client = '" + client + "'");
            while (data.Read())
            {
                HistoricalOrder o = new HistoricalOrder();
                o.Client = data.GetString(1).Trim();
                o.Product = data.GetString(2).Trim();
                o.Amount = data.GetInt32(3);
                o.Date = Convert.ToDateTime(data.GetSqlDateTime(4).ToString());
                orders.Add(o);
            }
            return orders;
        }

        /* Operaciones auxiliares */
        public static void truncateDB()
        {
            db.connect();
            db.setData("TRUNCATE TABLE Clients");
            db.setData("TRUNCATE TABLE Products");
            db.setData("TRUNCATE TABLE Orders");
            db.setData("TRUNCATE TABLE Tables");
            db.setData("TRUNCATE TABLE Rooms");
            db.setData("TRUNCATE TABLE Bills");
            db.setData("TRUNCATE TABLE Address");
            db.setData("TRUNCATE TABLE Restaurants");
            db.disconnect();
        }
    }
}