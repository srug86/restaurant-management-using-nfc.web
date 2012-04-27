using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using WebServices.objects;

namespace WebServices
{
    class XmlProcessor
    {
        private const string header = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

        /* Métodos que construyen un XML a partir de objetos */
        public static string xmlTablesStatusBuilder(List<Table> tables)
        {
            string xml = header + "\n<Tables>\n";
            if (tables.Count > 0)
                foreach (Table table in tables)
                {
                    xml += "\t<Table id=\"" + table.Id + "\">\n";
                    xml += "\t\t<Status>" + table.Status + "</Status>\n";
                    xml += "\t\t<Client>" + table.Client + "</Client>\n";
                    xml += "\t\t<Guests>" + table.Guests + "</Guests>\n";
                    xml += "\t</Table>\n";
                }
            xml += "</Tables>";
            return xml;
        }

        public static string xmlOrdersStatusBuilder(List<Order> orders)
        {
            string xml = header + "\n<Orders>\n";
            if (orders.Count > 0)
            {
                int i = 1;
                foreach (Order order in orders)
                {
                    xml += "\t<Order id=\"" + i + "\">\n";
                    xml += "\t\t<Table>" + order.TableID + "</Table>\n";
                    xml += "\t\t<Product>" + order.Product + "</Product>\n";
                    xml += "\t\t<Amount>" + order.Amount + "</Amount>\n";
                    xml += "\t\t<Status>" + order.Status + "</Status>\n";
                    xml += "\t\t<Date>" + dateTimeToString(order.Date) + "</Date>\n";
                    xml += "\t</Order>\n";
                    i++;
                }
            }
            xml += "</Orders>";
            return xml;
        }

        public static string xmlProductsBuilder(List<Product> products)
        {
            string xml = header + "\n<Products>\n";
            if (products.Count > 0)
                foreach (Product product in products)
                {
                    xml += "\t<Product name=\"" + product.Name + "\">\n";
                    xml += "\t\t<Category>" + product.Category + "</Category>\n";
                    xml += "\t\t<Price>" + product.Price + "</Price>\n";
                    xml += "\t\t<Description>" + product.Description + "</Description>\n";
                    xml += "\t</Product>\n";
                }
            xml += "</Products>";
            return xml;
        }

        public static string xmlRoomsBuilder(List<Room> rooms)
        {
            string xml = header + "\n<Rooms>\n";
            if (rooms.Count > 0)
                foreach (Room room in rooms)
                {
                    xml += "\t<Room name=\"" + room.Name + "\">\n";
                    xml += "\t\t<Width>" + room.Width + "</Width>\n";
                    xml += "\t\t<Height>" + room.Height + "</Height>\n";
                    xml += "\t\t<Tables>" + room.Tables + "</Tables>\n";
                    xml += "\t\t<Capacity>" + room.Capacity + "</Capacity>\n";
                    xml += "\t</Room>\n";
                }
            xml += "</Rooms>";
            return xml;
        }

        public static string xmlTableStatusBuilder(Table table, Client client, List<Order> orders)
        {
            string xml = header + "\n<TableInf>\n";
            xml += "\t<Table>\n";
            xml += "\t\t<Id>" + table.Id + "</Id>\n";
            xml += "\t\t<Capacity>" + table.Capacity + "</Capacity>\n";
            xml += "\t\t<Status>" + table.Status + "</Status>\n";
            xml += "\t</Table>\n";
            xml += "\t<Client>\n";
            if (client != null)
            {
                xml += "\t\t<DNI>" + client.Dni + "</DNI>\n";
                xml += "\t\t<Name>" + client.Name + "</Name>\n";
                xml += "\t\t<Surname>" + client.Surname + "</Surname>\n";
                xml += "\t\t<Guests>" + table.Guests + "</Guests>\n";
                xml += "\t\t<Appearances>" + client.Appearances + "</Appearances>\n";
            }
            xml += "\t</Client>\n";
            xml += "\t<Orders>\n";
            if (orders.Count > 0)
            {
                int i = 1;
                foreach (Order order in orders)
                {
                    xml += "\t\t<Order id=\"" + i + "\">\n";
                    xml += "\t\t\t<Product>" + order.Product + "</Product>\n";
                    xml += "\t\t\t<Amount>" + order.Amount + "</Amount>\n";
                    xml += "\t\t\t<Status>" + order.Status + "</Status>\n";
                    xml += "\t\t\t<Date>" + dateTimeToString(order.Date) + "</Date>\n";
                    xml += "\t\t</Order>\n";
                }
            }
            xml += "\t</Orders>\n";
            xml += "</TableInf>";
            return xml;
        }

        /* Métodos que construyen objetos a partir de un XML */
        public static List<Order> xmlOrdersDecoder(string sXml)
        {
            List<Order> ordersL = new List<Order>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sXml);
            XmlNodeList orders = xml.GetElementsByTagName("Orders");
            XmlNodeList oList = ((XmlElement)orders[0]).GetElementsByTagName("Order");
            foreach (XmlElement order in oList)
            {
                Order orderO = new Order();
                orderO.TableID = Convert.ToInt16(order.GetAttribute("Table"));
                orderO.Product = Convert.ToString(order.GetAttribute("Product"));
                orderO.Amount = Convert.ToInt16(order.GetAttribute("Amount"));
                orderO.Status = Convert.ToInt16(order.GetAttribute("Status"));
                orderO.Date = stringToDateTime(Convert.ToString(order.GetAttribute("Date")));
                ordersL.Add(orderO);
            }
            return ordersL;
        }

        public static List<Product> xmlProductsDecoder(string sXml)
        {
            List<Product> productsL = new List<Product>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sXml);
            XmlNodeList products = xml.GetElementsByTagName("Products");
            XmlNodeList pList = ((XmlElement)products[0]).GetElementsByTagName("Product");
            foreach (XmlElement product in pList)
            {
                Product productO = new Product();
                productO.Name = Convert.ToString(product.GetAttribute("name"));
                productO.Category = Convert.ToString(product.GetAttribute("Category"));
                productO.Price = Convert.ToDouble(product.GetAttribute("Price"));
                productO.Description = Convert.ToString(product.GetAttribute("Description"));
                productsL.Add(productO);
            }
            return productsL;
        }

        public static Room xmlAddRoomDecoder(string sXml)
        {
            Room roomDef = new Room();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sXml);
            XmlNodeList room = xml.GetElementsByTagName("Room");
            foreach (XmlElement node in room)
                roomDef.Name = node.GetAttribute("name");
            XmlNodeList dimension = ((XmlElement)room[0]).GetElementsByTagName("Dimension");
            XmlNodeList width = ((XmlElement)dimension[0]).GetElementsByTagName("Width");
            roomDef.Width = Convert.ToInt16(width[0].InnerText);
            XmlNodeList height = ((XmlElement)dimension[0]).GetElementsByTagName("Height");
            roomDef.Height = Convert.ToInt16(height[0].InnerText);
            XmlNodeList nTables = ((XmlElement)dimension[0]).GetElementsByTagName("NTables");
            roomDef.Tables = Convert.ToInt16(nTables[0].InnerText);
            XmlNodeList tCapacity = ((XmlElement)dimension[0]).GetElementsByTagName("TCapacity");
            roomDef.Capacity = Convert.ToInt16(tCapacity[0].InnerText);
            roomDef.Xml = sXml;
            return roomDef;
        }

        public static List<Table> xmlRoomTablesDecoder(string sXml)
        {
            List<Table> tablesDef = new List<Table>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sXml);
            XmlNodeList room = xml.GetElementsByTagName("Room");
            XmlNodeList tables = ((XmlElement)room[0]).GetElementsByTagName("Tables");
            XmlNodeList tList = ((XmlElement)tables[0]).GetElementsByTagName("Table");
            foreach (XmlElement table in tList)
            {
                Table tableDef = new Table();
                tableDef.Id = Convert.ToInt16(table.GetAttribute("id"));
                tableDef.Capacity = Convert.ToInt16(table.GetAttribute("capacity"));
                tablesDef.Add(tableDef);
            }
            return tablesDef;
        }

        private static string dateTimeToString(DateTime date)
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

        private static DateTime stringToDateTime(string date)
        {
            return new DateTime(Convert.ToInt16(date.Substring(0, 4)),
                Convert.ToInt16(date.Substring(5, 2)), Convert.ToInt16(date.Substring(8, 2)),
                Convert.ToInt16(date.Substring(11, 2)), Convert.ToInt16(date.Substring(14, 2)),
                Convert.ToInt16(date.Substring(17, 2)));
        }
    }
}