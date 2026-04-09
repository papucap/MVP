using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP
{
    public class QuestSystem
    {
        public List<string> Completed = new();

        public string CompleteQuest(Player player, string quest)
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
