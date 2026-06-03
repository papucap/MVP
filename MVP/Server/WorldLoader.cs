using System.Text.Json;

namespace MVP.Server;
public class WorldLoader
{
    public Dictionary<string, Room> LoadWorld()
    {
        var basePath = AppContext.BaseDirectory;

        var roomsPath = Path.Combine(basePath, "Server", "Data", "rooms.json");
        var npcsPath = Path.Combine(basePath, "Server", "Data", "npcs.json");

        var rooms = JsonSerializer.Deserialize<Dictionary<string, Room>>(
            File.ReadAllText(roomsPath),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var npcs = JsonSerializer.Deserialize<List<NPC>>(
            File.ReadAllText(npcsPath),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        foreach (var room in rooms.Values)
        {
            foreach (var npcId in room.NPCsIds)
            {
                var npc = npcs.FirstOrDefault(n => n.Name == npcId);
                if (npc != null)
                {
                    npc.CurrentRoom = room;
                    room.NPCs.Add(npc);
                }
            }
        }

        return rooms;
    }

    public List<Item> LoadItems()
    {
        var basePath = AppContext.BaseDirectory;
        var itemsPath = Path.Combine(basePath, "Server", "Data", "items.json");
        return JsonSerializer.Deserialize<List<Item>>(
            File.ReadAllText(itemsPath),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public List<Quest> LoadQuests()
    {
        var basePath = AppContext.BaseDirectory;
        var questsPath = Path.Combine(basePath, "Server", "Data", "quests.json");
        return JsonSerializer.Deserialize<List<Quest>>(
            File.ReadAllText(questsPath),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}