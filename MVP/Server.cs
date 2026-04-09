using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MVP
{
    public class Server
    {
        private TcpListener listener;

        public async Task Start(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                var handler = new ClientHandler(client);
                _ = handler.Handle();
            }
        }
    }
}
