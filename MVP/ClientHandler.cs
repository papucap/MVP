using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MVP
{
    public class ClientHandler
    {
        private TcpClient client;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
        }

        public async Task Handle()
        {
            using var stream = client.GetStream();
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(stream) { AutoFlush = true };

            await writer.WriteLineAsync("Zadej jméno:");

            string name = await reader.ReadLineAsync();
            await writer.WriteLineAsync($"Vítej {name}");

            while (true)
            {
                var input = await reader.ReadLineAsync();
                if (input == null) break;

                await writer.WriteLineAsync($"Zadal jsi: {input}");
            }
        }
    }
}
