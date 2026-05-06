using System.Numerics;
namespace MVP.Server
{
    public class DialogueSystem
    {
        public string GetDialogue(NPC npc, Server.Player player)
        {
            if (player.Money < 500)
            {
                return "Nejdřív vydělej víc peněz.";
            }


            return npc.Talk();
        }
    }

}