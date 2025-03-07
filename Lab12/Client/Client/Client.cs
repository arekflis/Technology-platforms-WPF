using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ClientSolution
{
    public class Client
    {
        private string ipAddress { get; set; }
        private int portNumber { get; set; }
        private Person person { get; set; }
        private TcpClient tcpClient { get; set; }

        public Client(string ipAddress, int portNumber, Person person)
        {
            this.ipAddress = ipAddress;
            this.portNumber = portNumber;
            this.tcpClient = new TcpClient();
            this.person = person;
        }


        public void findChanges(Person p1, Person p2)
        {
            if (p1.name != p2.name) Console.WriteLine($"Names are different: {p1.name} != {p2.name}");
            else if (p1.surname != p2.surname) Console.WriteLine($"Surnames are different: {p1.surname} != {p2.surname}");
            else if (p1.age != p2.age) Console.WriteLine($"Ages are different: {p1.age} != {p2.age}");
            else if (p1.height != p2.height) Console.WriteLine($"Heights are different: {p1.height} != {p2.height}");
            else if (p1.weight != p2.weight) Console.WriteLine($"Weights are different: {p1.weight} != {p2.weight}");
            else if (p1.salary != p2.salary) Console.WriteLine($"Salaries are different: {p1.salary} != {p2.salary}");
        }

        public async Task clientStart()
        {
            this.tcpClient.Connect(IPAddress.Parse(ipAddress), portNumber);
            NetworkStream stream = this.tcpClient.GetStream();
            Console.WriteLine("My person:");
            this.person.printPerson();
            for (int i=0; i<4; i++)
            {
                string jsonString = JsonSerializer.Serialize(this.person);
                byte[] data = Encoding.UTF8.GetBytes(jsonString);
                stream.Write(data, 0, data.Length);

                Console.WriteLine("I sent!");

                byte[] buffer = new byte[4096];
                int received = await stream.ReadAsync(buffer, 0, buffer.Length);
                jsonString = Encoding.UTF8.GetString(buffer, 0, received);
                Person personReceived = JsonSerializer.Deserialize<Person>(jsonString);
                Console.WriteLine("I received!");
                personReceived.printPerson();

                Console.WriteLine("Changes:");
                this.findChanges(this.person, personReceived);
                Random random = new Random();
                await Task.Delay(random.Next(1000,5000));
            }
            stream.Close();
            this.tcpClient.Close();
        }

    }
}
