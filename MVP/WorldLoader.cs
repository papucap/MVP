using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MVP
{
    public class WorldLoader
    {
        public Dictionary<string, Room> LoadWorld()
        {
            var rooms = JsonSerializer.Deserialize<Dictionary<string, Room>>(
                File.ReadAllText("data/rooms.json"));

            var npcs = JsonSerializer.Deserialize<List<NPC>>(
                File.ReadAllText("data/npcs.json"));

            var items = JsonSerializer.Deserialize<List<Item>>(
                File.ReadAllText("data/items.json"));

            // napojení NPC do místností
            foreach (var npc in npcs)
            {
                if (rooms.ContainsKey(npc.CurrentRoomId))
                {
                    rooms[npc.CurrentRoomId].NPCs.Add(npc);
                }
            }

            // (volitelně) napojení itemů podle jména
            foreach (var room in rooms.Values)
            {
                room.Items = items
                    .Where(i => room.Items.Any(x => x.Name == i.Name))
                    .ToList();
            }

            return rooms;
        }
    }
}
