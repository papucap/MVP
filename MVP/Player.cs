namespace MVP
{ 
public class Player
{
    public string Name { get; set; }
    public Room CurrentRoom { get; set; }
    public List<Item> Inventory { get; set; } = new();
    public int Money { get; set; } = 1000;

    public void SendMessage(string msg)
    {
        Console.WriteLine(msg);
    }

    public void ShowInventory()
    {
        if (Inventory.Count == 0)
        {
            SendMessage("Inventář je prázdný.");
            return;
        }

        foreach (var item in Inventory)
            {
                SendMessage(item.Name);
            }
    }
}
}