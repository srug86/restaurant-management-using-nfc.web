using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.objects
{
    class Room
    {
        private string name, xml;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Xml
        {
            get { return xml; }
            set { xml = value; }
        }

        private int width, height, tables, capacity;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Tables
        {
            get { return tables; }
            set { tables = value; }
        }

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public Room() { }

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