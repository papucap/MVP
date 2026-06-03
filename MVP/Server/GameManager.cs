using MVP.Server.Server;
using System.Linq;

namespace MVP.Server
{
    public class GameManager
    {
        public Dictionary<string, Player> Players = new();
        public Dictionary<string, Room> Rooms = new();
        public List<Item> Items = new();
        public List<Quest> Quests = new();

        public void AddPlayer(Player player)
        {
            Players[player.Name] = player;

            if (player.CurrentRoom == null && Rooms.Count > 0)
                player.CurrentRoom = Rooms.Values.First();

            Broadcast($"{player.Name} se pripojil.");
            Logger.Info($"Hrac pridan do hry: {player.Name}, mistnost: {player.CurrentRoom?.Name}");
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player.Name);
            Broadcast($"{player.Name} se odpojil.");
        }

        public string MovePlayer(Player player, string roomName)
        {
            if (!Rooms.ContainsKey(roomName))
                return "Takova mistnost neexistuje.";

            if (!player.CurrentRoom.Exits.Contains(roomName))
                return $"Tudy se tam dostat neda. Vychody: {string.Join(", ", player.CurrentRoom.Exits)}";

            player.CurrentRoom = Rooms[roomName];
            Logger.Info($"[{player.Name}] presunul se do: {roomName}");

            return DescribeRoom(player);
        }

        public string DescribeRoom(Player player)
        {
            var room = player.CurrentRoom;

            var items = room.Items.Count > 0
                ? "Itemy: " + string.Join(", ", room.Items.Select(i => i.Name))
                : "Zadne itemy";

            var npcs = room.NPCs.Count > 0
                ? "NPC: " + string.Join(", ", room.NPCs.Select(n => n.Name))
                : "Zadna NPC";

            var questHint = player.ActiveQuest != null
                ? $"Aktivni quest: {player.ActiveQuest}"
                : "";

            return $"== {room.Name} ==\n" +
                   $"{room.Description}\n" +
                   $"Vychody: {string.Join(", ", room.Exits)}\n" +
                   $"{items}\n" +
                   $"{npcs}" +
                   (string.IsNullOrEmpty(questHint) ? "" : "\n" + questHint);
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
                return "Item tu neni.";

            room.Items.Remove(item);
            player.Inventory.Add(item);

            var result = $"Sebral jsi {item.Name}.";

            var questResult = CheckItemQuest(player, itemName);
            if (!string.IsNullOrEmpty(questResult))
                result += "\n" + questResult;

            return result;
        }

        public string DropItem(Player player, string itemName)
        {
            var item = player.Inventory.FirstOrDefault(i => i.Name == itemName);

            if (item == null)
                return "Ten item nemas.";

            player.Inventory.Remove(item);
            player.CurrentRoom.Items.Add(item);

            return $"Zahodil jsi {item.Name}.";
        }

        public string Buy(Player player, string itemName)
        {
            var item = Items.FirstOrDefault(i => i.Name == itemName);
            if (item == null) return "Item neexistuje.";

            int price = item.Price > 0 ? item.Price : 100;

            if (player.Money < price)
                return "Nemas penize.";

            player.Money -= price;
            player.Inventory.Add(new Item { Name = item.Name, Price = item.Price });

            return $"Koupil jsi {item.Name} za {price} Kc.";
        }

        public string Sell(Player player, string itemName)
        {
            var item = player.Inventory.FirstOrDefault(i => i.Name == itemName);

            if (item == null)
                return "Ten item nemas.";

            int price = item.Price > 0 ? item.Price : 100;
            player.Inventory.Remove(item);
            player.Money += price;

            return $"Prodal jsi {item.Name} za {price} Kc.";
        }

        public string TalkToNPC(Player player, string npcName)
        {
            var npc = player.CurrentRoom.NPCs.FirstOrDefault(n => n.Name == npcName);

            if (npc == null)
                return "NPC tu neni.";

            var dialogue = npc.Dialogues[new Random().Next(npc.Dialogues.Count)];

            var questResult = CheckNPCQuest(player, npcName);

            return dialogue + (string.IsNullOrEmpty(questResult) ? "" : "\n" + questResult);
        }

        public string AcceptQuest(Player player, int index)
        {
            if (player.ActiveQuest != null)
                return $"Uz mas aktivni quest: {player.ActiveQuest}. Dokoncit ho nejdriv.";

            if (index < 0 || index >= Quests.Count)
                return "Takovy quest neexistuje.";

            var quest = Quests[index];

            if (player.CompletedQuests.Contains(quest.Name))
                return "Tento quest jsi uz splnil.";

            player.ActiveQuest = quest.Name;
            Logger.Info($"[{player.Name}] prijal quest: {quest.Name}");

            return $"Prijal jsi quest: {quest.Name}\n{quest.Description}";
        }

        public string ListQuests(Player player)
        {
            var lines = new List<string> { "=== QUESTY ===" };

            for (int i = 0; i < Quests.Count; i++)
            {
                var q = Quests[i];
                string status;

                if (player.CompletedQuests.Contains(q.Name))
                    status = "[splneno]";
                else if (player.ActiveQuest == q.Name)
                    status = "[aktivni]";
                else
                    status = "[nesplneno]";

                lines.Add($"{i + 1}. {status} {q.Name} - odmena: {q.RewardMoney} Kc");
                lines.Add($"   {q.Description}");
            }

            lines.Add("\nPro prijeti questu napis: quest <cislo>");
            return string.Join("\n", lines);
        }

        private string CheckItemQuest(Player player, string pickedItemName)
        {
            if (player.ActiveQuest == null) return "";

            var quest = Quests.FirstOrDefault(q => q.Name == player.ActiveQuest);
            if (quest == null) return "";

            if (!string.IsNullOrEmpty(quest.TargetItem) && quest.TargetItem == pickedItemName)
                return TryCompleteQuest(player, quest);

            return "";
        }

        private string CheckNPCQuest(Player player, string npcName)
        {
            if (player.ActiveQuest == null) return "";

            var quest = Quests.FirstOrDefault(q => q.Name == player.ActiveQuest);
            if (quest == null) return "";

            if (!string.IsNullOrEmpty(quest.TargetNPC) && quest.TargetNPC == npcName)
                return TryCompleteQuest(player, quest);

            return "";
        }

        private string TryCompleteQuest(Player player, Quest quest)
        {
            if (!string.IsNullOrEmpty(quest.RequiredItem))
            {
                var requiredItem = player.Inventory.FirstOrDefault(i => i.Name == quest.RequiredItem);
                if (requiredItem == null)
                    return $"Potrebujes mit u sebe: {quest.RequiredItem}";

                player.Inventory.Remove(requiredItem);
            }

            player.Money += quest.RewardMoney;
            player.CompletedQuests.Add(quest.Name);
            player.ActiveQuest = null;

            Logger.Info($"[{player.Name}] splnil quest: {quest.Name}, odmena: {quest.RewardMoney} Kc");

            var result = $"Quest splnen: {quest.Name}!\n+{quest.RewardMoney} Kc (mas celkem {player.Money} Kc)";

            var winMsg = CheckWinCondition(player);
            if (!string.IsNullOrEmpty(winMsg))
                result += "\n" + winMsg;

            return result;
        }

        private string CheckWinCondition(Player player)
        {
            if (player.CompletedQuests.Count >= Quests.Count)
            {
                player.GameCompleted = true;
                Logger.Info($"[{player.Name}] dokoncil hru s {player.Money} Kc");

                return "\n=== GRATULUJEME! ===\n" +
                       "Dokoncil jsi vsechny questy!\n";
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

                    Broadcast($"{npc.Name} odsel do {newRoom.Name}");
                }
            }
        }
    }
}