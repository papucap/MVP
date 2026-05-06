using System.Net.Sockets;
using System.Text;

namespace MVP.Client;
public class Client
{
    public async Task Start(string host, int port)
    {
        var client = new TcpClient();
        await client.ConnectAsync(host, port);

        using var stream = client.GetStream();
        using var reader = new StreamReader(stream);
        using var writer = new StreamWriter(stream) { AutoFlush = true };

        // čtení ze serveru
        _ = Task.Run(async () =>
        {
            while (true)
            {
                var msg = await reader.ReadLineAsync();
                if (msg != null)
                    Console.WriteLine(msg);
            }
        });

        // posílání inputu
        while (true)
        {
            var input = Console.ReadLine();
            await writer.WriteLineAsync(input);
        }
    }
}