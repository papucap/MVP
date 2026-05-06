using System.Numerics;
namespace MVP.Server.Server {
    public class GameManager
    {
        public Dictionary<string, Player> Players = new();
        public Dictionary<string, Room> Rooms = new();

        // systémy
        public TradeSystem Trade { get; set; }
        public DialogueSystem Dialogue { get; set; }
        public QuestSystem Quest { get; set; }

        // přidání hráče
        public void AddPlayer(Player player)
        {
            Players[player.Name] = player;

            // spawn do první místnosti
            if (Rooms.Count > 0)
                player.CurrentRoom = Rooms.Values.First();

            Broadcast($"{player.Name} se připojil.");
            DescribeRoom(player);
        }

        // odebrání hráče
        public void RemovePlayer(Player player)
        {
            Players.Remove(player.Name);
            Broadcast($"{player.Name} se odpojil.");
        }

        // pohyb hráče
        public string MovePlayer(Player player, string roomName)
        {
            if (!Rooms.ContainsKey(roomName))
                return "Taková místnost neexistuje.";

            player.CurrentRoom = Rooms[roomName];

            return DescribeRoom(player);
        }

        // popis místnosti
        public string DescribeRoom(Player player)
        {
            var room = player.CurrentRoom;

            return $"== {room.Name} ==\n" +
                   $"{room.Description}\n" +
                   $"Východy: {string.Join(", ", room.Exits)}";
        }

        // zpráva všem
        public void Broadcast(string message)
        {
            foreach (var p in Players.Values)
                p.SendMessage(message);
        }

        // zpráva do místnosti (M1)
        public void BroadcastRoom(Player sender, string message)
        {
            foreach (var p in Players.Values)
            {
                if (p.CurrentRoom == sender.CurrentRoom)
                    p.SendMessage($"{sender.Name}: {message}");
            }
        }
    }
}