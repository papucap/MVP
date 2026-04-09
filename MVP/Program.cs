namespace MVP
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var gameManager = new GameManager();

            // načtení světa
            var loader = new WorldLoader();
            gameManager.Rooms = loader.LoadRooms("data/rooms.json");

            // systémy
            gameManager.Trade = new TradeSystem();
            gameManager.Dialogue = new DialogueSystem();
            gameManager.Quest = new QuestSystem();

            // server
            var server = new Server(gameManager);
            await server.Start(5000);
        }
    }
}
