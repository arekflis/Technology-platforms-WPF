using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace ServerSolution
{
    class Program
    {
        static public async Task Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 64332);
            await server.serverStart();
        }

    }

}
