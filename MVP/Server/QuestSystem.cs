using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Server
{
    public class QuestSystem
    {
        public List<string> Completed = new();

        public string CompleteQuest(MVP.Server.Server.Player player, string quest)
        {
            if (Completed.Contains(quest))
            {
                return "Quest už splněn.";
            }
                

            Completed.Add(quest);
            player.Money += 500;

            return $"Splnil jsi quest: {quest}";
        }
    }
}
