public class Item
{
    // fields
    
    public int Weight { get; }
    public string Description { get; }

    public override string ToString()
    {
        return Description; 
    }

    // constructor
    public Item(string description, int weight)
    {
        
        Weight = weight;
        Description = description;
    }
}