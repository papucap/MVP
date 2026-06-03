using MVP.Server.Server;

namespace MVP.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var gameManager = new GameManager();

            var loader = new WorldLoader();
            gameManager.Rooms = loader.LoadWorld();
            gameManager.Items = loader.LoadItems();
            gameManager.Quests = loader.LoadQuests();

            Logger.Info("Server spuštěn.");

            _ = gameManager.StartNPCMovement();

            var server = new GameServer(gameManager);
            await server.Start(5001);
        }
    }
}