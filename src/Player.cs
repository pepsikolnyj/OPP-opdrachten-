using System;
using System.Linq;
using System.Collections.Generic;

public class Player
{
    private int health;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    public Inventory backpack { get; }

    public Room CurrentRoom { get; set; }

    // constructor
    public Player()
    {
        backpack = new Inventory(25);
        health = 150;
        CurrentRoom = null;
    }
    public bool TakeFromChest(string name)
    {
        Item item = CurrentRoom.Chest.Get(name);

        if (item == null)
        {
            Console.WriteLine($"There is no {name} in the chest.");
            return false;
        }

        if (backpack.Put(name, item))
        {
            Console.WriteLine($"You have taken the {name} from the chest.");
            CheckCraftingCondition();
            return true;
        }
        else
        {
            CurrentRoom.Chest.Put(name, item);
            Console.WriteLine($"You cannot carry the {name}, it's too heavy!");
            return false;
        }
    }
    public bool DropToChest(string name)
    {
        Item item = backpack.Get(name);

        if (item == null)
        {
            Console.WriteLine($"You do not have {name} in your backpack.");
            return false;
        }

        if (CurrentRoom.Chest.Put(name, item))
        {
            Console.WriteLine($"You dropped the {name} into the chest.");
            return true;
        }
        else
        {
            backpack.Put(name, item);
            Console.WriteLine($"The chest cannot hold the {name}.");
            return false;
        }
    }

    public int Damage(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
        return Health;
    }

    public void Heal(int healPoints)
    {
        if (Health >= 100)
        {
            Console.WriteLine("You are completely healthy!");
        }
        else
        {
            Health += healPoints;
            if (Health > 100) Health = 100;
            Console.WriteLine($"You are healed for {healPoints} HP.");
            Console.WriteLine($"HP bar: {Health}");
        }
    }
    public bool isAlive()
    {
        return Health > 0;
    }
    private readonly List<string> requiredNames = new List<string> { 
        "Blade of Agility",
        "Point Booster",
        "Wizardy Staff",
        "Ogre Axe"
    };

    private void CheckCraftingCondition()
    {
        bool hasAll = requiredNames.All(name => backpack.items.ContainsKey(name));

        if (!hasAll)
            return;

        foreach (string name in requiredNames)
            backpack.Remove(name);

        Item artifact = new Item("Aghanim's Scepter", 15);
        backpack.Put("Aghanim's Scepter", artifact);

        Console.WriteLine("You crafted the Ancient Artifact!");
        Console.WriteLine("You win!");
        Environment.Exit(0);
    }
}
