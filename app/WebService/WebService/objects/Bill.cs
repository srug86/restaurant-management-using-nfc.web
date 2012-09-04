using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServices.objects
{
    // 'Bill' describe la información de una factura
    public class Bill
    {
        /* Atributos del objeto */
        // Información de la compañía
        private Company companyInfo;
        public Company CompanyInfo
        {
            get { return companyInfo; }
            set { companyInfo = value; }
        }

        private Address companyAddress, clientAddress;
        // Dirección de la compañía
        public Address CompanyAddress
        {
            get { return companyAddress; }
            set { companyAddress = value; }
        }
        // Dirección del cliente
        public Address ClientAddress
        {
            get { return clientAddress; }
            set { clientAddress = value; }
        }

        // Información del cliente
        private Client clientInfo;
        internal Client ClientInfo
        {
            get { return clientInfo; }
            set { clientInfo = value; }
        }

        private int id, tableID, serial, paid;
        // Identificador de la factura
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        // Mesa facturada
        public int TableID
        {
            get { return tableID; }
            set { tableID = value; }
        }
        // Número de serie de la factura
        public int Serial
        {
            get { return serial; }
            set { serial = value; }
        }
        // Método de pago: (0) No cobrada, (1) Cobro normal, (2) Cobro NFC
        public int Paid
        {
            get { return paid; }
            set { paid = value; }
        }

        // Fecha de facturación
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private double iva, discount, taxBase, quote, subtotal, total;
        // IVA
        public double Iva
        {
            get { return iva; }
            set { iva = value; }
        }
        // Descuento en el precio acumulado
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }
        // Base imponible
        public double TaxBase
        {
            get { return taxBase; }
            set { taxBase = value; }
        }
        // Cuota
        public double Quote
        {
            get { return quote; }
            set { quote = value; }
        }
        // Subtotal
        public double Subtotal
        {
            get { return subtotal; }
            set { subtotal = value; }
        }
        // Total
        public double Total
        {
            get { return total; }
            set { total = value; }
        }

        // Lista de pedidos
        private List<OrderPrice> orders;
        public List<OrderPrice> Orders
        {
            get { return orders; }
            set { orders = value; }
        }

        // Método constructor
        public Bill() {
            Paid = 0;
            TaxBase = Total = Subtotal = Quote = 0;
            Orders = new List<OrderPrice>();
        }
    }
}