using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSolution
{
    public class Person
    {
        public string name { get; set; }
        public string surname { get; set; }
        public int age { get; set; }

        public double height { get; set; }

        public double weight { get; set; }

        public double salary { get; set; }

        public Person() { }

        public Person(string name, string surname, int age, double height, double weight, double salary)
        {
            this.name = name;
            this.surname = surname;
            this.age = age;
            this.height = height;
            this.weight = weight;
            this.salary = salary;
        }

        private int returnIndex(Random random)
        {
            return random.Next(0, 4);
        }

        public void createPerson()
        {
            Random random = new Random();
            string[] names = { "Alexis", "James", "George", "Ava" };
            this.name = names[this.returnIndex(random)];
            string[] surnames = { "Johnson", "Colwill", "Palmer", "Sterling" };
            this.surname = surnames[this.returnIndex(random)];
            this.age = random.Next(0, 100);
            this.height = random.Next(150, 250);
            this.weight = random.Next(50, 200);
            this.salary = random.Next(7500, 12000);
        }


        public void printPerson()
        {
            Console.WriteLine($"Name: {this.name} Surname: {this.surname} Age: {this.age} Height: {this.height} Weight: {this.weight} Salary: {this.salary}");
        }
    }
}
