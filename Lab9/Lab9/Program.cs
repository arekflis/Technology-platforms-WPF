using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Lab9
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Car> myCars = new List<Car>(){
                 new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                 new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                 new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                 new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                 new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                 new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                 new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                 new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                 new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            };
            
            queries(myCars);
            serialize(myCars);
            deserialize();
            xPaths();
            createXML(myCars);
            createTableOnPage(myCars);
            changeXML();
        }


        static void changeXML()
        {
            XElement xmlFile = XElement.Load("cars.xml");

            foreach (var element in xmlFile.Elements())
            {
                foreach (var subElement in element.Elements())
                {
                    if (subElement.Name.LocalName == "engine")
                    {
                        foreach (var subSubElement in subElement.Elements())
                        {
                            if (subSubElement.Name.LocalName == "horsePower")
                            {
                                subSubElement.Name = "hp";
                            }
                        }
                    }
                }
            }

            foreach (var element in xmlFile.Elements())
            {
                string year = "";
                foreach (var subElement in element.Elements())
                {
                    if (subElement.Name.LocalName == "year")
                    {
                        year = subElement.Value;
                        subElement.Remove();
                    }
                }
                element.Element("model").SetAttributeValue("year", year);
            }


            xmlFile.Save("carsNew.xml");
            Console.WriteLine("XML file was changed\n");
        }

        static void createTableOnPage(List<Car> myCars)
        {
            XElement template = XElement.Load("template.html");
            XElement table = new XElement("table",
                                 new XAttribute("border", 5),
                                 new XElement("tr",
                                      new XElement("th", "Model"),
                                      new XElement("th", "Year"),
                                      new XElement("th", "EngineModel"),
                                      new XElement("th", "EngineHorsePower"),
                                      new XElement("th", "EngineDisplacement")
                                 )
                 );

            IEnumerable<XElement> rows = from car in myCars
                                         select new XElement("tr",
                                                    new XElement("td", car.model),
                                                    new XElement("td", car.year),
                                                    new XElement("td", car.motor.model),
                                                    new XElement("td", car.motor.horsePower),
                                                    new XElement("td", car.motor.displacement)
                                        );

            table.Add(rows);
            template.Add(table);
            template.Save("template.html");
            Console.WriteLine("Table on the page was created\n");
        }
        static void createXML(List<Car> myCars)
        {
            IEnumerable<XElement> nodes = from car in myCars
                                          select new XElement("car",
                                                     new XElement("model", car.model),
                                                     new XElement("year", car.year),
                                                     new XElement("engine",
                                                         new XAttribute("model", car.motor.model),
                                                         new XElement("displacement", car.motor.displacement),
                                                         new XElement("horsePower", car.motor.horsePower)
                                                         )
                                                     );
            XElement rootNode = new XElement("cars", nodes);
            rootNode.Save("CarsFromLinq.xml");
            Console.WriteLine("New XML file was created\n");
        }

        static void xPaths()
        {
            XElement rootNode = XElement.Load("cars.xml");
            string path = "sum(/car/engine[@model != 'TDI']/horsePower) div count(/car/engine[@model != 'TDI'])";
            
            double avgHP = (double)rootNode.XPathEvaluate(path);
            Console.WriteLine("Average horsePower: " + avgHP + "\n");
            
            string path2 = "/car[not(model = preceding-sibling::car/model)]";
            IEnumerable<XElement> models = rootNode.XPathSelectElements(path2);
            
            Console.WriteLine("Unique car names:");
            foreach (XElement model in models)
            {
                Console.WriteLine(model.Element("model").Value);
            }
            Console.WriteLine("\n");
        }

        static void serialize(List<Car> myCars)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            using (TextWriter writer = new StreamWriter("cars.xml"))
            {
                serializer.Serialize(writer, myCars);
            }
            Console.WriteLine("Serialized list myCars");
            Console.WriteLine("\n");
        }
        
        static void deserialize()
        {
            List<Car> newCars;
            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            using(TextReader reader = new StreamReader("cars.xml"))
            {
                newCars = (List<Car>)serializer.Deserialize(reader);
            }
            Console.WriteLine("Deserialized list myCars\n");
            Console.WriteLine("Cars:");
            foreach (var car in newCars)
            {
                Console.WriteLine(car.model + " " + car.year + " " + car.motor.displacement + " " + car.motor.horsePower + " " + car.motor.model);
            }
            Console.WriteLine("\n");
        }


        static void queries(List<Car> myCars)
        {
            var query1 = myCars.Where(car => car.model == "A6")
                                .Select(car => new
                                {
                                    engineType = car.motor.model == "TDI" ? "diesel" : "petrol",
                                    hppl = (double)(car.motor.horsePower / car.motor.displacement)
                                });

            var query2 = query1.GroupBy(eType => eType.engineType,
                                        eType => eType.hppl,
                                        (key, title) => new
                                        {
                                            type = key,
                                            avgHPPL = title.Average()
                                        });

            Console.WriteLine("Average hppl in groups:");
            foreach (var item in query2)
            {
                Console.WriteLine(item.type + ": " + item.avgHPPL);
            }
            Console.WriteLine("\n");
        }

    }
}
