using System.Numerics;
namespace MVP {
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
        public void MovePlayer(Player player, string roomId)
        {
            if (!Rooms.ContainsKey(roomId))
            {
                player.SendMessage("Tato místnost neexistuje.");
                return;
            }

            player.CurrentRoom = Rooms[roomId];
            player.SendMessage($"Přesunul ses do: {player.CurrentRoom.Name}");

            DescribeRoom(player);
        }

        // popis místnosti
        public void DescribeRoom(Player player)
        {
            var room = player.CurrentRoom;

            player.SendMessage($"== {room.Name} ==");
            player.SendMessage(room.Description);

            // východy
            player.SendMessage("Východy: " + string.Join(", ", room.Exits));

            // itemy
            if (room.Items.Count > 0)
                player.SendMessage("Itemy: " + string.Join(", ", room.Items.Select(i => i.Name)));

            // NPC
            if (room.NPCs.Count > 0)
                player.SendMessage("NPC: " + string.Join(", ", room.NPCs.Select(n => n.Name)));

            // ostatní hráči
            var others = Players.Values
                .Where(p => p != player && p.CurrentRoom == room)
                .Select(p => p.Name);

            if (others.Any())
                player.SendMessage("Hráči: " + string.Join(", ", others));
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