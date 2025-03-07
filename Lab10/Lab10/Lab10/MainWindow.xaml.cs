using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SubBindingList<Car> myCars;

        public MainWindow() {
            
            InitializeComponent();

            createCarGrid();
            initializeSortandFindComboBox();
            queries();
            queries2();
            find(); 
            sort();
        }

        private void initializeSortandFindComboBox()
        {
            List<string> options = new List<string>();
            options.Add("Model");
            options.Add("Year");
            options.Add("Engine Model");
            options.Add("Engine HorsePower");
            options.Add("Engine Displacement");

            FindComboBox.ItemsSource = options;
            FindComboBox.SelectedIndex = 0;
            SortComboBox.ItemsSource = options;
            SortComboBox.SelectedIndex = 0;
        }

        private void createCarGrid()
        {
            myCars = ListManager.LoadDataSub();

            carsGrid.ItemsSource = myCars;
            DeleteComboBox.ItemsSource = myCars;
            EditComboBox.ItemsSource = myCars;
            EditComboBox.SelectedIndex = 0;

            DataGridTextColumn modelColumn = new DataGridTextColumn();
            modelColumn.Header = "Model";
            modelColumn.Binding = new Binding("model");
            carsGrid.Columns.Add(modelColumn);
            

            DataGridTextColumn yearColumn = new DataGridTextColumn();
            yearColumn.Header = "Year";
            yearColumn.Binding = new Binding("year");
            carsGrid.Columns.Add(yearColumn);

            DataGridTextColumn engineModelColumn = new DataGridTextColumn();
            engineModelColumn.Header = "Engine Model";
            engineModelColumn.Binding = new Binding("motor.model");
            carsGrid.Columns.Add(engineModelColumn);

            DataGridTextColumn hpModelColumn = new DataGridTextColumn();
            hpModelColumn.Header = "Engine HorsePower";
            hpModelColumn.Binding = new Binding("motor.horsePower");
            carsGrid.Columns.Add(hpModelColumn);

            DataGridTextColumn displacementModelColumn = new DataGridTextColumn();
            displacementModelColumn.Header = "Engine Displacement";
            displacementModelColumn.Binding = new Binding("motor.displacement");
            carsGrid.Columns.Add(displacementModelColumn);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Car newCar = new Car();
            Engine newEngine = new Engine();

            int year = 0;
            double horsePower = 0, displacement = 0;

            if(CarModelTextBox.Text != "Car Model" 
                && EngineModelTextBox.Text != "Engine Model" 
                && int.TryParse(YearTextBox.Text, out year)
                && double.TryParse(DisplacementTextBox.Text, out displacement)
                && double.TryParse(HPTextBox.Text, out horsePower))
            {
                newCar.model = CarModelTextBox.Text;
                newCar.year = year;
                newEngine.model = EngineModelTextBox.Text;
                newEngine.displacement = displacement;
                newEngine.horsePower = horsePower;

                newCar.motor = newEngine;

                myCars.Add(newCar);

                carsGrid.ItemsSource = myCars;
                EditComboBox.ItemsSource = myCars;
                EditComboBox.SelectedIndex = 0;
                DeleteComboBox.ItemsSource = myCars;
            }
            else
            {
                MessageBox.Show("Please enter all data and in correct format!");
            }

            CarModelTextBox.Text = "Car Model";
            YearTextBox.Text = "Year Production";
            EngineModelTextBox.Text = "Engine Model";
            DisplacementTextBox.Text = "Engine Displacement";
            HPTextBox.Text = "Engine Horse Power";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            Car deleteCar = (Car)DeleteComboBox.SelectedItem;

            myCars.Remove(deleteCar);
            carsGrid.ItemsSource = myCars;
            EditComboBox.ItemsSource = myCars;
            DeleteComboBox.ItemsSource = myCars;
            EditComboBox.SelectedIndex = 0;
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)SortComboBox.SelectedValue == "Model")
            {
                myCars = new SubBindingList<Car>(myCars.OrderBy(x => x.model).ToList());
                carsGrid.ItemsSource = myCars;
            }
            if ((string)SortComboBox.SelectedValue == "Year")
            {
                myCars = new SubBindingList<Car>(myCars.OrderBy(x => x.year).ToList());
                carsGrid.ItemsSource = myCars;
            }
            if ((string)SortComboBox.SelectedValue == "Engine Model")
            {
                myCars = new SubBindingList<Car>(myCars.OrderBy(x => x.motor.model).ToList());
                carsGrid.ItemsSource = myCars;
            }
            if ((string)SortComboBox.SelectedValue == "Engine Displacement")
            {
                myCars = new SubBindingList<Car>(myCars.OrderBy(x => x.motor.displacement).ToList());
                carsGrid.ItemsSource = myCars;
            }
            if ((string)SortComboBox.SelectedValue == "Engine HorsePower")
            {
                myCars = new SubBindingList<Car>(myCars.OrderBy(x => x.motor.horsePower).ToList());
                carsGrid.ItemsSource = myCars;
            }
        }

        private void EditBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Car selectedCar = (Car)EditComboBox.SelectedItem;
            CarModelEditBox.Text = selectedCar.model;
            YearEditBox.Text = selectedCar.year.ToString();
            EngineModelEditBox.Text = selectedCar.motor.model;
            HPEditBox.Text = selectedCar.motor.horsePower.ToString();
            DisplacementEditBox.Text = selectedCar.motor.displacement.ToString();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Car selectedCar = (Car)EditComboBox.SelectedItem;
            int index = myCars.IndexOf(selectedCar);


            int year = 0;
            double horsePower = 0, displacement = 0;

            if (CarModelEditBox.Text != ""
                && EngineModelEditBox.Text != ""
                && int.TryParse(YearEditBox.Text, out year)
                && double.TryParse(DisplacementEditBox.Text, out displacement)
                && double.TryParse(HPEditBox.Text, out horsePower))
            {
                myCars[index].model = CarModelEditBox.Text;
                myCars[index].year = year;
                myCars[index].motor.model = EngineModelEditBox.Text;
                myCars[index].motor.displacement = displacement;
                myCars[index].motor.horsePower = horsePower;
            }
            else
            {
                MessageBox.Show("Please enter all data and in correct format!");
            }
            EditComboBox.SelectionChanged -= EditBox_SelectionChanged;
            EditComboBox.ItemsSource = null;
            EditComboBox.ItemsSource = myCars;
            DeleteComboBox.ItemsSource = null;
            DeleteComboBox.ItemsSource = myCars;
            carsGrid.ItemsSource = null;
            carsGrid.ItemsSource = myCars;
            EditComboBox.SelectionChanged += EditBox_SelectionChanged;
            EditComboBox.SelectedIndex = 0;
            
        }


        private void queries()
        {
            var query1 = myCars.Where(car => car.model == "A6")
                                .Select(car => new
                                {
                                    engineType = car.motor.model == "TDI" ? "diesel" : "petrol",
                                    HPPL = (double) car.motor.horsePower / car.motor.displacement
                                });

            var query2 = query1.GroupBy(c => c.engineType, c => c.HPPL, (key, value) => new
                                                                    {
                                                                        engineType = key,
                                                                        avgHPPL = value.Average()
                                                                    }).OrderByDescending(c => c.avgHPPL).ToList();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var q in query2)
            {
                stringBuilder.Append(q.engineType + " " + q.avgHPPL.ToString() +"\n");
            }

            MessageBox.Show(stringBuilder.ToString());
        }

        Comparison<Car> arg1 = delegate (Car car1, Car car2)
        {
            if (car1.motor.horsePower == car2.motor.horsePower) return 0;
            if (car1.motor.horsePower > car2.motor.horsePower) return -1;
            return 1;
        };

        Predicate<Car> arg2 = delegate (Car car1)
        {
            if (car1.motor.model == "TDI") return true;
            return false;
        };

        Action<Car> arg3 = delegate (Car car1)
        {
            MessageBox.Show(car1.ToString());
        };

        private void queries2()
        {
            List<Car> cars = ListManager.LoadDataList();
            cars.Sort(new Comparison<Car>(arg1));
            cars.FindAll(arg2).ForEach(arg3);
        }

        private void sort()
        {
            SubBindingList<Car> cars = ListManager.LoadDataSub();

            cars.Sort(nameof(Car.motor) + "." + nameof(Engine.model), ListSortDirection.Descending);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (Car car in cars)
            {
                stringBuilder.Append(car.ToString() + "\n");
            }

            MessageBox.Show(stringBuilder.ToString());

        }

        private void find()
        {
            SubBindingList<Car> cars = ListManager.LoadDataSub();

            List<int> indexes = cars.Find(nameof(Car.motor) + "." + nameof(Engine.model), "TDI");

            StringBuilder stringBuilder = new StringBuilder();

            foreach (int index in indexes)
            {
                stringBuilder.Append(cars.ElementAt(index).ToString() + "\n");
            }

            MessageBox.Show(stringBuilder.ToString());
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedItem = FindComboBox.SelectedItem.ToString();
            if (selectedItem == "Model") selectedItem = nameof(Car.model);
            else if (selectedItem == "Year") selectedItem = nameof(Car.year);
            else if (selectedItem == "Engine Model") selectedItem = nameof(Car.motor) + "." + nameof(Engine.model);
            else if (selectedItem == "Engine HorsePower") selectedItem = nameof(Car.motor) + "." + nameof(Engine.horsePower);
            else if (selectedItem == "Engine Displacement") selectedItem = nameof(Car.motor) + "." + nameof(Engine.displacement);

            List<int> indexes = new List<int>();

            if(selectedItem ==  nameof(Car.model) || selectedItem == nameof(Car.motor) + "." + nameof(Engine.model))
            {
                string value = FindTextBox.Text;
                indexes = myCars.Find(selectedItem, value); 
            }
            else if (selectedItem == nameof(Car.year))
            {
                int value = 0;
                if(int.TryParse(FindTextBox.Text, out value))
                {
                    indexes = myCars.Find(selectedItem, value);
                }
                else {
                    MessageBox.Show("Invalid value!");
                    return; 
                }
            }
            else
            {
                double value;
                if(double.TryParse(FindTextBox.Text, out value))
                {
                    indexes = myCars.Find(selectedItem, value);
                }
                else {
                    MessageBox.Show("Invalid value");
                    return; 
                }
            }
            
            List<Car> cars = new List<Car>();
            foreach(int index in indexes)
            {
                cars.Add(myCars.ElementAt(index));
            }
            ResultComboBox.ItemsSource = cars;
            ResultComboBox.SelectedIndex = 0;
            FindComboBox.SelectedIndex = 0;

            FindTextBox.Text = "Enter value";
            FindComboBox.SelectedIndex = 0;
        }
    }
}
