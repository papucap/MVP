using System.Text.Json;

namespace MVP.Server;
public class WorldLoader
{
    public Dictionary<string, Room> LoadWorld()
    {
        var basePath = AppContext.BaseDirectory;

        var roomsPath = Path.Combine(basePath, "Server", "Data", "rooms.json");
        var npcsPath = Path.Combine(basePath, "Server", "Data", "npcs.json");
        var itemsPath = Path.Combine(basePath, "Server", "Data", "items.json");

        var rooms = JsonSerializer.Deserialize<Dictionary<string, Room>>(File.ReadAllText(roomsPath));
        var npcs = JsonSerializer.Deserialize<List<NPC>>(File.ReadAllText(npcsPath));
        var items = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(itemsPath));

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

        foreach (var room in rooms.Values)
        {
            
        }

        return rooms;
    }

    public List<Item> LoadItems()
    {
        var basePath = AppContext.BaseDirectory;
        var itemsPath = Path.Combine(basePath, "Server", "Data", "items.json");

        return JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(itemsPath));
    }

}