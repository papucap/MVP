using System.Numerics;

public class CommandHandler
{
    private GameManager gameManager;

    public CommandHandler(GameManager gm)
    {
        gameManager = gm;
    }

    public void Handle(Player player, string input)
    {
        var parts = input.Split(' ');
        var command = parts[0].ToLower();

        switch (command)
        {
            case "jdi":
                if (parts.Length > 1)
                    gameManager.MovePlayer(player, parts[1]);
                break;

            case "rekni":
                var msg = input.Substring(6);
                gameManager.Broadcast($"{player.Name}: {msg}");
                break;

            case "inventar":
                player.ShowInventory();
                break;

            default:
                player.SendMessage("Neznámý příkaz.");
                break;
        }
    }
}