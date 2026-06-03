using MVP.Server.Server;
using System.Linq;

namespace MVP.Server
{
    public class GameManager
    {
        public Dictionary<string, Player> Players = new();
        public Dictionary<string, Room> Rooms = new();
        public List<Item> Items = new();

        public void AddPlayer(Player player)
        {
            Players[player.Name] = player;

            if (player.CurrentRoom == null && Rooms.Count > 0)
                player.CurrentRoom = Rooms.Values.First();

            Broadcast($"{player.Name} se připojil.");
            Logger.Info($"Hráč přidán do hry: {player.Name}, místnost: {player.CurrentRoom?.Name}");
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player.Name);
            Broadcast($"{player.Name} se odpojil.");
        }

        public string MovePlayer(Player player, string roomName)
        {
            if (!Rooms.ContainsKey(roomName))
                return "Taková místnost neexistuje.";

            player.CurrentRoom = Rooms[roomName];
            Logger.Info($"[{player.Name}] přesunul se do: {roomName}");

            return DescribeRoom(player) + "\n" + CheckQuest(player);
        }

        public string DescribeRoom(Player player)
        {
            var room = player.CurrentRoom;

            var items = room.Items.Count > 0
                ? "Itemy: " + string.Join(", ", room.Items.Select(i => i.Name))
                : "Žádné itemy";

            var npcs = room.NPCs.Count > 0
                ? "NPC: " + string.Join(", ", room.NPCs.Select(n => n.Name))
                : "Žádná NPC";

            return $"== {room.Name} ==\n" +
                   $"{room.Description}\n" +
                   $"Východy: {string.Join(", ", room.Exits)}\n" +
                   $"{items}\n" +
                   $"{npcs}";
        }

        public void Broadcast(string message)
        {
            foreach (var p in Players.Values)
                p.SendMessage(message);
        }

        public void Say(Player player, string text)
        {
            foreach (var p in Players.Values.Where(p => p.CurrentRoom == player.CurrentRoom))
                p.SendMessage($"{player.Name}: {text}");
        }

        public string TakeItem(Player player, string itemName)
        {
            var room = player.CurrentRoom;
            var item = room.Items.FirstOrDefault(i => i.Name == itemName);

            if (item == null)
                return "Item tu není.";

            room.Items.Remove(item);
            player.Inventory.Add(item);

            return $"Sebral jsi {item.Name}.";
        }

        public string DropItem(Player player, string itemName)
        {
            var item = player.Inventory.FirstOrDefault(i => i.Name == itemName);

            if (item == null)
                return "Ten item nemáš.";

            player.Inventory.Remove(item);
            player.CurrentRoom.Items.Add(item);

            return $"Zahodil jsi {item.Name}.";
        }

        public string Buy(Player player, string itemName)
        {
            var item = Items.FirstOrDefault(i => i.Name == itemName);
            if (item == null) return "Item neexistuje.";

            int price = 100;

            if (player.Money < price)
                return "Nemáš peníze.";

            player.Money -= price;
            player.Inventory.Add(item);

            return $"Koupil jsi {item.Name} za {price} Kč.";
        }

        public string Sell(Player player, string itemName)
        {
            var item = player.Inventory.FirstOrDefault(i => i.Name == itemName);

            if (item == null)
                return "Ten item nemáš.";

            int price = 100;
            player.Inventory.Remove(item);
            player.Money += price;

            return $"Prodal jsi {item.Name} za {price} Kč.";
        }

        public string TalkToNPC(Player player, string npcName)
        {
            var npc = player.CurrentRoom.NPCs.FirstOrDefault(n => n.Name == npcName);

            if (npc == null)
                return "NPC tu není.";

            return npc.Dialogues[new Random().Next(npc.Dialogues.Count)];
        }

        public string CheckQuest(Player player)
        {
            if (player.ActiveQuest == "Doruč balíček")
            {
                player.Money += 200;
                player.ActiveQuest = null;
                Logger.Info($"[{player.Name}] splnil quest: Doruč balíček");

                return "✅ Quest splněn! +200 Kč";
            }

            return "";
        }

        public string CheckWinCondition(Player player)
{
    if (player.Money >= 2000 && player.CompletedQuests.Contains("Doruč balíček"))
    {
        player.GameCompleted = true;
        return "🏆 GRATULUJEME! Dokončil jsi hru!";
    }
    return "";
}
        

        public async Task StartNPCMovement()
        {
            while (true)
            {
                await Task.Delay(10000);

                var allNPCs = Rooms.Values.SelectMany(r => r.NPCs).ToList();

                foreach (var npc in allNPCs)
                {
                    var oldRoom = npc.CurrentRoom;
                    var newRoom = Rooms.Values.ElementAt(new Random().Next(Rooms.Count));

                    oldRoom.NPCs.Remove(npc);
                    newRoom.NPCs.Add(npc);
                    npc.CurrentRoom = newRoom;

                    Broadcast($"🚶 {npc.Name} odešel do {newRoom.Name}");
                }
            }
        }
    }
}
