using System.Collections.Generic;

public class Inventory
{
    private int maxWeight;
    public Dictionary<string, Item> items { get; private set; }

    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    private int currentWeight()
    {
        int weight = 0;
        foreach (Item item in items.Values)
            weight += item.Weight;

        return weight;
    }

    public int FreeWeight()
    {
        return maxWeight - currentWeight();
    }

    public bool Put(string name, Item item)
    {
        if (!items.ContainsKey(name) &&
            currentWeight() + item.Weight <= maxWeight)
        {
            items.Add(name, item);
            return true;
        }

        return false;
    }

    public Item Get(string name)
    {
        if (items.TryGetValue(name, out Item item))
        {
            items.Remove(name);
            return item;
        }

        return null;
    }

    public Item Remove(string name)
    {
        return Get(name);
    }

    public string Show()
    {
        if (items.Count == 0)
            return "Inventory is empty";

        return string.Join(", ", items.Keys);
    }
}