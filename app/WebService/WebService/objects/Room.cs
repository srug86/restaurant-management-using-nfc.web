using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.objects
{
    // 'Room' contiene la información detallada de una plantilla del restaurante
    class Room
    {
        /* Atributos del objeto */
        private string name, xml;
        // Nombre
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        // Descripción (en XML)
        public string Xml
        {
            get { return xml; }
            set { xml = value; }
        }

        private int width, height, tables, capacity;
        // Anchura (número de columnas)
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        // Altura (número de filas)
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        // Número de mesas del salón
        public int Tables
        {
            get { return tables; }
            set { tables = value; }
        }
        // Capacidad total del salón
        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        /* Métodos constructores */
        public Room()
        {
            Name = "";
        }

        public Room(string name, int width, int height, int tables, int capacity, string xml)
        {
            Name = name;
            Width = width;
            Height = height;
            Tables = tables;
            Capacity = capacity;
            Xml = xml;
        }
    }
}