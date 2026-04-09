using System.Numerics;

public class TradeSystem
{
    public string Buy(Player player, Item item, int price)
    {
        if (player.Money < price)
            return "Nemáš dost peněz.";

        player.Money -= price;
        player.Inventory.Add(item);

        return $"Koupil jsi {item.Name} za {price} Kč.";
    }

    public string Sell(Player player, Item item, int price)
    {
        if (!player.Inventory.Contains(item))
            return "Tento item nemáš.";

        player.Inventory.Remove(item);
        player.Money += price;

        return $"Prodal jsi {item.Name} za {price} Kč.";
    }
}
