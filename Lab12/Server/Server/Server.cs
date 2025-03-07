using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ServerSolution
{
    public class Server
    {

        private string ipAddress { get; set; }
        private int portNumber {  get; set; }
        
        private TcpListener listener { get; set; }

        public Server(string ipAddress, int portNumber)
        {
            this.ipAddress = ipAddress;
            this.portNumber = portNumber;
            this.listener = new TcpListener(IPAddress.Parse(ipAddress), portNumber);
        }

        public async Task serverStart()
        {
            try
            {
                this.listener.Start();
                Console.WriteLine("Server started");
                List<Task> taski = new List<Task>();
                for (int i=0;i<3; i++)
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    Task clientRun = Task.Run(() => this.doWithClient(tcpClient, i));
                    Console.WriteLine("Client joined the server");
                    taski.Add(clientRun);
                }
                await Task.WhenAll(taski);
            }
            finally
            {
                this.listener.Stop();
            }
        }

        public Person doChange(Person p1)
        {
            Random random = new Random();
            int index = random.Next(0, 6);
            if (index == 0) p1.name = "Trent";
            else if (index == 1) p1.surname = "Arnold";
            else if (index == 2) p1.weight = 200;
            else if (index == 3) p1.height = 250;
            else if (index == 4) p1.age = 100;
            else p1.salary = 12000;
            return p1;
        }


        public async Task doWithClient(TcpClient tcpClient, int id)
        {
            NetworkStream stream = tcpClient.GetStream();
            for (int i = 0; i<4; i++)
            {
                byte[] buffer = new byte[4096];
                int received = await stream.ReadAsync(buffer, 0, buffer.Length);
                string jsonString = Encoding.UTF8.GetString(buffer, 0, received);
                Person personReceived = JsonSerializer.Deserialize<Person>(jsonString);
                Console.WriteLine("I received!");
                personReceived.printPerson();

                personReceived = this.doChange(personReceived);

                Random random = new Random();
                await Task.Delay(random.Next(1000, 5000));


                jsonString = JsonSerializer.Serialize(personReceived);
                byte[] data = Encoding.UTF8.GetBytes(jsonString);
                stream.Write(data, 0, data.Length);

                Console.WriteLine("I sent!");
                personReceived.printPerson();
            }
            stream.Close();
            tcpClient.Close();
        }

    }
}
