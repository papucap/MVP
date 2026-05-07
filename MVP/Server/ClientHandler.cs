using MVP.Server.Server;
using System.Net.Sockets;
using System.Text;

namespace MVP.Server
{
    public class ClientHandler
    {
        private TcpClient client;
        private GameManager gameManager;
        private CommandHandler commandHandler;

        public ClientHandler(TcpClient client, GameManager gm)
        {
            this.client = client;
            gameManager = gm;
            commandHandler = new CommandHandler(gameManager);
        }

        public async Task Handle()
        {
            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            await writer.WriteLineAsync("Zadej jméno:");
            string name = await reader.ReadLineAsync();

            var player = new Player { Name = name };
            gameManager.AddPlayer(player);

            await writer.WriteLineAsync($"Vítej {name}!");
            await writer.WriteLineAsync("Napiš 'pomoc' pro seznam příkazů.");

            await writer.WriteLineAsync(gameManager.DescribeRoom(player));

            try
            {
                while (true)
                {
                    var input = await reader.ReadLineAsync();
                    if (input == null) break;

                    var response = commandHandler.Handle(player, input);

                    if (!string.IsNullOrEmpty(response))
                        await writer.WriteLineAsync(response);
                }
            }
            catch { }
            finally
            {
                gameManager.RemovePlayer(player);
                client.Close();
            }
        }
    }
}