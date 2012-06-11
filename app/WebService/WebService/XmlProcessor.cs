using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using WebServices.objects;
using System.Globalization;

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
                foreach (Order order in orders)
                {
                    xml += "\t<Order id=\"" + order.Id + "\">\n";
                    xml += "\t\t<Table>" + order.TableID + "</Table>\n";
                    xml += "\t\t<Product>" + order.Product + "</Product>\n";
                    xml += "\t\t<Amount>" + order.Amount + "</Amount>\n";
                    xml += "\t\t<Status>" + order.Status + "</Status>\n";
                    xml += "\t\t<Date>" + order.Date.ToString() + "</Date>\n";
                    xml += "\t</Order>\n";
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
                    xml += "\t\t<Visible>" + product.Visible + "</Visible>\n";
                    xml += "\t\t<Discount>" + product.Discount + "</Discount>\n";
                    xml += "\t\t<DiscountedUnit>" + product.DiscountedUnit + "</DiscountedUnit>\n";
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
                foreach (Order order in orders)
                {
                    xml += "\t\t<Order id=\"" + order.Id + "\">\n";
                    xml += "\t\t\t<Product>" + order.Product + "</Product>\n";
                    xml += "\t\t\t<Amount>" + order.Amount + "</Amount>\n";
                    xml += "\t\t\t<Status>" + order.Status + "</Status>\n";
                    xml += "\t\t\t<Date>" + order.Date.ToString() + "</Date>\n";
                    xml += "\t\t</Order>\n";
                }
            xml += "\t</Orders>\n";
            xml += "</TableInf>";
            return xml;
        }

        public static string xmlHistoryBuilder(List<HistoricalOrder> orders)
        {
            string xml = header + "\n<Historical>\n";
            foreach (HistoricalOrder order in orders)
            {
                xml += "\t<Order>\n";
                xml += "\t\t<Client>" + order.Client + "</Client>\n";
                xml += "\t\t<Product>" + order.Product + "</Product>\n";
                xml += "\t\t<Amount>" + order.Amount + "</Amount>\n";
                xml += "\t\t<Date>" + order.Date.ToString() + "</Date>\n";
                xml += "\t</Order>\n";
            }
            xml += "</Historical>";
            return xml;
        }

        public static string xmlBillBuilder(Bill bill)
        {
            string xml = header + "\n<Bill>\n";
            xml += "\t<Company name=\"" + bill.CompanyInfo.Name + "\">\n";
            xml += "\t\t<NIF>" + bill.CompanyInfo.NIF + "</NIF>\n";
            xml += "\t\t<Address>\n";
            xml += "\t\t\t<Street>" + bill.CompanyAddress.Street + "</Street>\n";
            xml += "\t\t\t<Number>" + bill.CompanyAddress.Number + "</Number>\n";
            xml += "\t\t\t<ZipCode>" + bill.CompanyAddress.ZipCode + "</ZipCode>\n";
            xml += "\t\t\t<Town>" + bill.CompanyAddress.Town + "</Town>\n";
            xml += "\t\t\t<State>" + bill.CompanyAddress.State + "</State>\n";
            xml += "\t\t</Address>\n";
            xml += "\t\t<Contact>\n";
            xml += "\t\t\t<Phone>" + bill.CompanyInfo.Phone + "</Phone>\n";
            xml += "\t\t\t<Fax>" + bill.CompanyInfo.Fax + "</Fax>\n";
            xml += "\t\t\t<Email>" + bill.CompanyInfo.Email + "</Email>\n";
            xml += "\t\t</Contact>\n";
            xml += "\t</Company>\n";
            xml += "\t<Client>\n";
            xml += "\t\t<DNI>" + bill.ClientInfo.Dni + "</DNI>\n";
            xml += "\t\t<Name>" + bill.ClientInfo.Name + "</Name>\n";
            xml += "\t\t<Surname>" + bill.ClientInfo.Surname + "</Surname>\n";
            xml += "\t\t<Address>\n";
            xml += "\t\t\t<Street>" + bill.ClientAddress.Street + "</Street>\n";
            xml += "\t\t\t<Number>" + bill.ClientAddress.Number + "</Number>\n";
            xml += "\t\t\t<ZipCode>" + bill.ClientAddress.ZipCode + "</ZipCode>\n";
            xml += "\t\t\t<Town>" + bill.ClientAddress.Town + "</Town>\n";
            xml += "\t\t\t<State>" + bill.ClientAddress.State + "</State>\n";
            xml += "\t\t</Address>\n";
            xml += "\t</Client>\n";
            xml += "\t<Info>\n";
            xml += "\t\t<Number>" + bill.Id + "</Number>\n";
            xml += "\t\t<Serial>" + bill.Serial + "</Serial>\n";
            xml += "\t\t<Date>" + bill.Date.ToString() + "</Date>\n";
            xml += "\t\t<Table>" + bill.TableID + "</Table>\n";
            xml += "\t</Info>\n";
            xml += "\t<Orders>\n";
            foreach (OrderPrice oPrice in bill.Orders)
            {
                xml += "\t\t<Order id=\"" + oPrice.Order.Id + "\">\n";
                xml += "\t\t\t<Product>" + oPrice.Order.Product + "</Product>\n";
                xml += "\t\t\t<Amount>" + oPrice.Order.Amount + "</Amount>\n";
                xml += "\t\t\t<Price>" + oPrice.Price + "</Price>\n";
                xml += "\t\t\t<Discount>" + oPrice.Discount + "</Discount>\n";
                xml += "\t\t\t<Total>" + oPrice.Total + "</Total>\n";
                xml += "\t\t</Order>\n";
            }
            xml += "\t</Orders>\n";
            xml += "\t<PriceSummary>\n";
            xml += "\t\t<Subtotal>" + bill.Subtotal + "</Subtotal>\n";
            xml += "\t\t<Discount>" + bill.Discount + "</Discount>\n";
            xml += "\t\t<TaxBase>" + bill.TaxBase + "</TaxBase>\n";
            xml += "\t\t<IVA>" + bill.Iva + "</IVA>\n";
            xml += "\t\t<Quote>" + bill.Quote + "</Quote>\n";
            xml += "\t\t<Total>" + bill.Total + "</Total>\n";
            xml += "\t</PriceSummary>\n";
            xml += "\t<Paid>" + bill.Paid + "</Paid>\n";
            xml += "</Bill>";
            return xml;
        }

        /*public static string xmlShortBillBuilder(Bill bill)
        {
            string xml = header + "<Bill>";
            xml += "<Company>" + bill.CompanyInfo.Name + "</Company>";
            xml += "<Date>" + bill.Date.ToString() + "</Date>";
            xml += "<Table>" + bill.TableID + "</Table>";
            xml += "<Orders>";
            foreach (OrderPrice oPrice in bill.Orders)
                xml += "<Order name=\"" + oPrice.Order.Product + "\" amount=\"" + 
                    oPrice.Order.Amount + "\" price=\"" + oPrice.Price + "\" discount=\"" +
                    oPrice.Discount + "\" total=\"" + oPrice.Total + "\"/>";
            xml += "</Orders>";
            xml += "<PriceSummary subtotal=\"" + bill.Subtotal + "\" discount=\"" + bill.Discount +
                "\" taxBase=\"" + bill.TaxBase + "\" IVA=\"" + bill.Iva + "\" quote=\"" + bill.Quote +
                "\" total=\"" + bill.Total + "\"/>";
            xml += "</Bill>";
            return xml;
        }*/

        public static string xmlShortBillBuilder(Bill bill)
        {
            string xml = header + "<Bill>";
            xml += "<Company>" + bill.CompanyInfo.Name + "</Company>";
            xml += "<Date>" + bill.Date.ToString() + "</Date>";
            xml += "<Table>" + bill.TableID + "</Table>";
            xml += "<Orders>";
            foreach (OrderPrice oPrice in bill.Orders)
            {
                xml += "<Order id=\"" + oPrice.Order.Id + "\">";
                xml += "<Name>" + oPrice.Order.Product + "</Name>";
                xml += "<Amount>" + oPrice.Order.Amount + "</Amount>";
                xml += "<Price>" + oPrice.Price + "</Price>";
                xml += "<Discount>" + oPrice.Discount + "</Discount>";
                xml += "<Total>" + oPrice.Total + "</Total>";
                xml += "</Order>";
            }
            xml += "</Orders>";
            xml += "<PriceSummary>";
            xml += "<Subtotal>" + bill.Subtotal + "</Subtotal>";
            xml += "<Discount>" + bill.Discount + "</Discount>";
            xml += "<TaxBase>" + bill.TaxBase + "</TaxBase>";
            xml += "<IVA>" + bill.Iva + "</IVA>";
            xml += "<Quote>" + bill.Quote + "</Quote>";
            xml += "<Total>" + bill.Total + "</Total>";
            xml += "</PriceSummary>";
            xml += "</Bill>";
            return xml;
        }

        public static string xmlRecommendationBuilder(Recommendation rec)
        {
            string xml = header + "\n<Recommendations>\n";
            xml += "\t<Opening>" + rec.Opening + "</Opening>\n";
            xml += "\t<Products>\n";
            xml += "\t\t<Usually>\n";
            foreach (RecProduct product in rec.Usually)
            {
                xml += "\t\t\t<Category name=\"" + product.Category + "\">\n";
                xml += "\t\t\t\t<Product>" + product.Name + "</Product>\n";
                xml += "\t\t\t\t<Times>" + product.Times + "</Times>\n";
                xml += "\t\t\t</Category>\n";
            }
            xml += "\t\t</Usually>\n";
            xml += "\t\t<Promotional>\n";
            foreach (RecProduct product in rec.Promotional)
            {
                xml += "\t\t\t<Product name=\"" + product.Name + "\">\n";
                xml += "\t\t\t\t<Discount>" + product.Discount + "</Discount>\n";
                xml += "\t\t\t\t<Units>" + product.DiscountedUnit + "</Units>\n";
                xml += "\t\t\t</Product>\n";
            }
            xml += "\t\t</Promotional>\n";
            xml += "\t\t<Recommended>\n";
            foreach (RecProduct product in rec.Recommended)
                xml += "\t\t\t<Product name=\"" + product.Name + "\" category=\"" +
                    product.Category + "\">\n";
            xml += "\t\t</Recommended>\n";
            xml += "\t</Products>\n";
            xml += "</Recommendations>";
            return xml;
        }

        /* Métodos que construyen objetos a partir de un XML */
        public static List<Object> xmlRestaurantDecoder(string sXml)
        {
            List<Object> objects = new List<Object>();
            if (sXml != "")
            {
                Company company = new Company();
                Address address = new Address();
                double iva, discount = 0.0;
                int discountedVisit = 0;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sXml);
                XmlNodeList restaurant = xml.GetElementsByTagName("Restaurant");
                XmlNodeList companyD = ((XmlElement)restaurant[0]).GetElementsByTagName("Company");
                XmlNodeList nif = ((XmlElement)companyD[0]).GetElementsByTagName("NIF");
                company.NIF = Convert.ToString(nif[0].InnerText).Trim();
                XmlNodeList name = ((XmlElement)companyD[0]).GetElementsByTagName("Name");
                company.Name = Convert.ToString(name[0].InnerText).Trim();
                XmlNodeList phone = ((XmlElement)companyD[0]).GetElementsByTagName("Phone");
                company.Phone = Convert.ToInt32(phone[0].InnerText);
                XmlNodeList fax = ((XmlElement)companyD[0]).GetElementsByTagName("Fax");
                company.Fax = Convert.ToInt32(phone[0].InnerText);
                XmlNodeList email = ((XmlElement)companyD[0]).GetElementsByTagName("Email");
                company.Email = Convert.ToString(email[0].InnerText).Trim();
                objects.Add(company);
                XmlNodeList addressD = ((XmlElement)restaurant[0]).GetElementsByTagName("Address");
                XmlNodeList street = ((XmlElement)addressD[0]).GetElementsByTagName("Street");
                address.Street = Convert.ToString(street[0].InnerText).Trim();
                XmlNodeList number = ((XmlElement)addressD[0]).GetElementsByTagName("Number");
                address.Number = Convert.ToString(number[0].InnerText).Trim();
                XmlNodeList zip = ((XmlElement)addressD[0]).GetElementsByTagName("ZipCode");
                address.ZipCode = Convert.ToInt32(zip[0].InnerText);
                XmlNodeList town = ((XmlElement)addressD[0]).GetElementsByTagName("Town");
                address.Town = Convert.ToString(town[0].InnerText).Trim();
                XmlNodeList state = ((XmlElement)addressD[0]).GetElementsByTagName("State");
                address.State = Convert.ToString(state[0].InnerText).Trim();
                objects.Add(address);
                XmlNodeList rate = ((XmlElement)restaurant[0]).GetElementsByTagName("Rate");
                iva = Convert.ToDouble(((XmlElement)rate[0]).GetAttribute("iva"));
                objects.Add(iva);
                discount = Convert.ToDouble(((XmlElement)rate[0]).GetAttribute("discount"));
                objects.Add(discount);
                discountedVisit = Convert.ToInt32(((XmlElement)rate[0]).GetAttribute("discountedVisit"));
                objects.Add(discountedVisit);
            }
            return objects;
        }

        public static List<Object> xmlClientDecoder(string sXml)
        {
            List<Object> clientData = new List<Object>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sXml);
            XmlNodeList cl = xml.GetElementsByTagName("Profile");
            Client client = new Client();
            XmlNodeList dni = ((XmlElement)cl[0]).GetElementsByTagName("DNI");
            client.Dni = Convert.ToString(dni[0].InnerText).Trim();
            XmlNodeList name = ((XmlElement)cl[0]).GetElementsByTagName("Name");
            client.Name = Convert.ToString(name[0].InnerText).Trim();
            XmlNodeList surname = ((XmlElement)cl[0]).GetElementsByTagName("Surname");
            client.Surname = Convert.ToString(surname[0].InnerText).Trim();
            clientData.Add(client);
            Address address = new Address();
            XmlNodeList addr = ((XmlElement)cl[0]).GetElementsByTagName("Address");
            XmlNodeList street = ((XmlElement)addr[0]).GetElementsByTagName("Street");
            address.Street = Convert.ToString(street[0].InnerText).Trim();
            XmlNodeList number = ((XmlElement)addr[0]).GetElementsByTagName("Number");
            address.Number = Convert.ToString(number[0].InnerText).Trim();
            XmlNodeList zip = ((XmlElement)addr[0]).GetElementsByTagName("ZipCode");
            address.ZipCode = Convert.ToInt32(zip[0].InnerText);
            XmlNodeList town = ((XmlElement)addr[0]).GetElementsByTagName("Town");
            address.Town = Convert.ToString(town[0].InnerText).Trim();
            XmlNodeList state = ((XmlElement)addr[0]).GetElementsByTagName("State");
            address.State = Convert.ToString(state[0].InnerText).Trim();
            clientData.Add(address);
            return clientData;
        }

        public static List<Order> xmlOrdersDecoder(string sXml)
        {
            List<Order> loo = new List<Order>();
            if (sXml != "")
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sXml);
                XmlNodeList oders = xml.GetElementsByTagName("Orders");
                XmlNodeList oList = ((XmlElement)oders[0]).GetElementsByTagName("Order");
                foreach (XmlElement order in oList)
                {
                    Order o = new Order();
                    o.Id = Convert.ToInt16(order.GetAttribute("id"));
                    XmlNodeList table = ((XmlElement)order).GetElementsByTagName("Table");
                    o.TableID = Convert.ToInt16(table[0].InnerText);
                    XmlNodeList product = ((XmlElement)order).GetElementsByTagName("Product");
                    o.Product = Convert.ToString(product[0].InnerText);
                    XmlNodeList amount = ((XmlElement)order).GetElementsByTagName("Amount");
                    o.Amount = Convert.ToInt16(amount[0].InnerText);
                    XmlNodeList status = ((XmlElement)order).GetElementsByTagName("Status");
                    o.Status = Convert.ToInt16(status[0].InnerText);
                    XmlNodeList date = ((XmlElement)order).GetElementsByTagName("Date");
                    o.Date = Convert.ToDateTime(date[0].InnerText);
                    loo.Add(o);
                }
            }
            return loo;
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
                productO.Name = Convert.ToString(product.GetAttribute("name")).Trim();
                XmlNodeList category = ((XmlElement)product).GetElementsByTagName("Category");
                productO.Category = Convert.ToString(category[0].InnerText).Trim();
                XmlNodeList price = ((XmlElement)product).GetElementsByTagName("Price");
                productO.Price = Convert.ToDouble(price[0].InnerText);
                XmlNodeList description = ((XmlElement)product).GetElementsByTagName("Description");
                productO.Description = Convert.ToString(description[0].InnerText);
                XmlNodeList visible = ((XmlElement)product).GetElementsByTagName("Visible");
                productO.Visible = Convert.ToBoolean(visible[0].InnerText);
                XmlNodeList discount = ((XmlElement)product).GetElementsByTagName("Discount");
                productO.Discount = Convert.ToDouble(discount[0].InnerText);
                XmlNodeList discountedUnit = ((XmlElement)product).GetElementsByTagName("DiscountedUnit");
                productO.DiscountedUnit = Convert.ToInt32(discountedUnit[0].InnerText);
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
    }
}