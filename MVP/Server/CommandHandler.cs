using MVP.Server.Server;
using System.Linq;

namespace MVP.Server
{
    public class CommandHandler
    {
        private GameManager gameManager;

        public CommandHandler(GameManager gm)
        {
            gameManager = gm;
        }

        public string Handle(Player player, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            var parts = input.Split(' ');
            var command = parts[0].ToLower();

            switch (command)
            {
                case "pomoc":
                    return "Příkazy: jdi <místo>, prozkoumat, inventar, vezmi <item>, zahod <item>, rekni <text>, krik <text>, kup <item>, prodej <item>, quest";

                case "jdi":
                    if (parts.Length < 2)
                        return "Kam chceš jít?";
                    return gameManager.MovePlayer(player, parts[1]);

                case "prozkoumat":
                    return gameManager.DescribeRoom(player);

                case "inventar":
                    return player.Inventory.Count > 0
                        ? "Inventář: " + string.Join(", ", player.Inventory.Select(i => i.Name))
                        : "Inventář je prázdný.";

                case "vezmi":
                    if (parts.Length < 2)
                        return "Co chceš vzít?";
                    return gameManager.TakeItem(player, parts[1]);

                case "zahod":
                    if (parts.Length < 2)
                        return "Co chceš zahodit?";
                    return gameManager.DropItem(player, parts[1]);

                case "rekni":
                    var msg = input.Length > 6 ? input.Substring(6) : "";
                    gameManager.Say(player, msg);
                    return "";

                case "krik":
                    var shout = input.Length > 5 ? input.Substring(5) : "";
                    gameManager.Broadcast($"📢 {player.Name}: {shout}");
                    return "";

                case "kup":
                    if (parts.Length < 2)
                        return "Co chceš koupit?";
                    return gameManager.Buy(player, parts[1]);

                case "prodej":
                    if (parts.Length < 2)
                        return "Co chceš prodat?";
                    return gameManager.Sell(player, parts[1]);

                case "mluv":
                    if (parts.Length < 2)
                        return "S kým chceš mluvit?";
                    return gameManager.TalkToNPC(player, parts[1]);

                case "quest":
                    player.ActiveQuest = "Doruč balíček";
                    return "📜 Přijal jsi quest: Doruč balíček";


                default:
                    return "Neznámý příkaz.";
            }
        }
    }
}