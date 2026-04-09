using System.Numerics;

public class DialogueSystem
{
    public string GetDialogue(NPC npc, Player player)
    {
        if (player.Money < 500)
        {
            return "Nejdřív vydělej víc peněz.";
        }
            

        return npc.Talk();
    }
}