using MVP.Server;
using MVP.Server.Server;

namespace MVP.Server;
class Program
{
    static async Task Main(string[] args)
    {
        var gameManager = new GameManager();

        // načtení světa
        var loader = new WorldLoader();
        gameManager.Rooms = loader.LoadWorld();

        // systémy
        gameManager.Trade = new TradeSystem();
        gameManager.Dialogue = new DialogueSystem();
        gameManager.Quest = new QuestSystem();

        // server
        var server = new GameServer(gameManager); 
        await server.Start(5001);
    }
}