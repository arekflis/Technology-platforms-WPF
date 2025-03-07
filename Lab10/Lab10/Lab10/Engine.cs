using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab10
{
    internal class Engine : IComparable
    {
        public double displacement { get; set; }
        public double horsePower { get; set; }

        public string model { get; set; }

        public Engine(double displacement, double horsePower, string model)
        {
            this.model = model;
            this.displacement = displacement;
            this.horsePower = horsePower;
        }

        public Engine() {}

        public int CompareTo(object other)
        {
            if(other is Engine otherEngine)
            {
                if (this.horsePower == otherEngine.horsePower) return 0;
                if (this.horsePower < otherEngine.horsePower) return -1;
                return 1;
            }
            Environment.Exit(-1);
            return -1;
        }


    }
}
