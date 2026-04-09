using System.Numerics;

public class GameManager
{
    public Dictionary<string, Player> Players = new();
    public Dictionary<string, Room> Rooms = new();

    public void AddPlayer(Player player)
    {
        Players[player.Name] = player;
        Broadcast($"{player.Name} se připojil.");
    }

    public void RemovePlayer(Player player)
    {
        Players.Remove(player.Name);
        Broadcast($"{player.Name} se odpojil.");
    }

    public void MovePlayer(Player player, string roomId)
    {
        if (!Rooms.ContainsKey(roomId)) return;

        player.CurrentRoom = Rooms[roomId];
        player.SendMessage($"Přesunul ses do: {roomId}");
    }

    public void Broadcast(string message)
    {
        foreach (var p in Players.Values)
            p.SendMessage(message);
    }
}