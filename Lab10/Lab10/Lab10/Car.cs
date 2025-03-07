using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab10
{
    internal class Car
    {
        public string model { get; set; }
        public int year { get; set; }

        public Engine motor { get; set; }

        public Car(string model, Engine motor, int year)
        {
            this.model = model;
            this.motor = motor;
            this.year = year;
        }

        public Car() { }

        public override string ToString()
        {
            return $"{model}, {year}";
        }
    }
}
