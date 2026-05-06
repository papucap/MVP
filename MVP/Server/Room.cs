namespace MVP.Server
{
    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<string> Exits { get; set; } = new();
        public List<Item> Items { get; set; } = new();
        public List<string> NPCsIds { get; set; } = new(); 
        public List<NPC> NPCs { get; set; } = new();
    }
}