using System;
using Newtonsoft.Json;
using System.Net.Sockets;


namespace ClientSolution
{
    class Program
    {
        static public async Task Main(string[] args)
        {
            Person person = new Person();
            person.createPerson();
            Client client = new Client("127.0.0.1", 64332, person);
            await client.clientStart();
            
        }

    }

}
