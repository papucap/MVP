using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MVP
{
    public class PlayerData
    {
        public void Save(Player player)
        {
            var json = JsonSerializer.Serialize(player);
            File.WriteAllText($"data/{player.Name}.json", json);
        }

        public Player Load(string name)
        {
            var path = $"data/{name}.json";
            if (!File.Exists(path)) return null;

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Player>(json);
        }
    }
}
