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
        public Dictionary<string, Room> LoadRooms(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Dictionary<string, Room>>(json);
        }
    }
}
