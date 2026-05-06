using MVP.Server.Server;

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
                    return "Příkazy: pomoc, jdi <místo>, inventar";

                case "jdi":
                    if (parts.Length < 2)
                        return "Kam chceš jít?";

                    return gameManager.MovePlayer(player, parts[1]);

                case "inventar":
                    return "Inventář: " +
                        (player.Inventory.Count > 0
                        ? string.Join(", ", player.Inventory.Select(i => i.Name))
                        : "prázdný");

                default:
                    return "Neznámý příkaz.";
            }
        }
    }
}