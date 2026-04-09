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
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(stream) { AutoFlush = true };

            await writer.WriteLineAsync("Zadej jméno:");
            string name = await reader.ReadLineAsync();

            var player = new Player { Name = name };
            gameManager.AddPlayer(player);

            await writer.WriteLineAsync($"Vítej {name}!");
            await writer.WriteLineAsync("Napiš 'pomoc' pro seznam příkazů.");

            try
            {
                while (true)
                {
                    var input = await reader.ReadLineAsync();

                    if (input == null)
                        break;

                    commandHandler.Handle(player, input);
                }
            }
            catch (Exception)
            {
                // ignoruj chyby (např. odpojení)
            }
            finally
            {
                gameManager.RemovePlayer(player);
                client.Close();
            }
        }
    }
}
