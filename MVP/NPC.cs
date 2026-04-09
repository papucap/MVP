public class NPC
{
    public string Name { get; set; }
    public string CurrentRoomId { get; set; }
    public List<string> Dialogues { get; set; }

    private int dialogueIndex = 0;

    public string Talk()
    {
        if (Dialogues == null || Dialogues.Count == 0)
        {
            return "NPC mlčí.";
        }
            

        var response = Dialogues[dialogueIndex];
        dialogueIndex = (dialogueIndex + 1) % Dialogues.Count;

        return response;
    }
}