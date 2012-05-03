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
                room.Name = data.GetString(0);
                room.Width = data.GetInt16(1);
                room.Height = data.GetInt16(2);
                room.Tables = data.GetInt16(3);
                room.Capacity = data.GetInt16(4);
                room.Xml = data.GetString(5);
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
            db.setData("INSERT INTO Products (name, category, price, description) VALUES ('" +
                product.Name + "', '" + product.Category + "', " + product.Price + ", '" + 
                product.Description + "')");
            db.disconnect();
        }

        public static List<Product> selectAllProducts()
        {
            List<Product> products = new List<Product>();
            db.connect();
            SqlDataReader data = db.getData("SELECT * FROM Products");
            while (data.Read())
            {
                Product product = new Product();
                product.Name = data.GetString(0);
                product.Category = data.GetString(1);
                product.Price = data.GetSqlMoney(2).ToDouble();
                product.Description = data.GetString(3);
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
                order.Status + ", '" + toSqlDateTime(order.Date) + "')");
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
                order.Product = data.GetString(2);
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
                order.Product = data.GetString(2);
                order.Amount = data.GetInt16(3);
                order.Status = data.GetInt16(4);
                order.Date = Convert.ToDateTime(data.GetSqlDateTime(5).ToString());
                orders.Add(order);
            }
            db.disconnect();
            return orders;
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
                client.Dni = data.GetString(0);
                client.Name = data.GetString(1);
                client.Surname = data.GetString(2);
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
                table.Client = data.GetString(3);
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
                table.Client = data.GetString(3);
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

        /* Operaciones auxiliares */
        private static string toSqlDateTime(DateTime date)
        {
            string month = date.Month.ToString(), day = date.Day.ToString(), hour = date.Hour.ToString(),
                minute = date.Minute.ToString(), second = date.Second.ToString();
            if (date.Month < 10) month = "0" + date.Month.ToString();
            if (date.Day < 10) day = "0" + date.Day.ToString();
            if (date.Hour < 10) hour = "0" + date.Hour.ToString();
            if (date.Minute < 10) minute = "0" + date.Minute.ToString();
            if (date.Second < 10) second = "0" + date.Second.ToString();
            return date.Year.ToString() + "-" + month + "-" + day + " " +
                hour + ":" + minute + ":" + second;
        }

    }
}