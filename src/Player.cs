using System;

public class Player
{
    private int health;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public Room CurrentRoom { get; set; }

    // constructor
    public Player()
    {
        health = 15;
        CurrentRoom = null;
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
}
