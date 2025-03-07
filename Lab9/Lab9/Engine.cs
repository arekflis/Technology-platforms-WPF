using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab9
{
    public class Engine
    {
        public double displacement {  get; set; }
        public double horsePower { get; set; }
        
        [XmlAttribute("model")]
        public string model {  get; set; }

        public Engine(double displacement, double horsePower, string model)
        {
            this.model = model;
            this.displacement = displacement;
            this.horsePower = horsePower;
        }
        
        public Engine()
        {

        }

    }

}
