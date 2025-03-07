using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Lab10
{
    internal class ListManager
    {
        public static BindingList<Car> LoadData()
        {
            BindingList<Car> myCars = new SubBindingList<Car>(){
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

            return myCars;
        }

        public static List<Car> LoadDataList()
        {
            List<Car> myCars = new List<Car>(){
                 new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                 new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                 new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                 new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                 new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                 new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                 new Car("A6", new Engine(3.0, 309, "TDI"), 2010),
                 new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                 new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            };

            return myCars;
        }

        public static SubBindingList<Car> LoadDataSub()
        {
            SubBindingList<Car> myCars = new SubBindingList<Car>(){
                 new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                 new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                 new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                 new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                 new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                 new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                 new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                 new Car("S6", new Engine(4.0, 414, "TFSI"), 2008),
                 new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            };

            return myCars;
        }

    }
}
