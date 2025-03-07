using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Lab9
{
    [XmlType("car")]
    public class Car
    {
        public string model { get; set; }
        public int year { get; set; }

        [XmlElement("engine")]
        public Engine motor { get; set; }

        public Car(string model, Engine motor, int year)
        {
            this.model = model;
            this.motor = motor;
            this.year = year;
        }

        public Car() { }
    }
}
