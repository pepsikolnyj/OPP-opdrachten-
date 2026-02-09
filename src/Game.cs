//De rest van Opdracht 4 ga ik samen met opdracht 5 maken, want ik wil dat de speler zal winnen door de oppakken van alle items. 
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");
		Room library = new Room("in the campus library");
		Room gym = new Room("in the campus gym");

        // Outside
        outside.AddExit("north", library);
        outside.AddExit("south", lab);
        outside.AddExit("west", pub);
        outside.AddExit("east", theatre);
        outside.AddExit("northeast", gym);

		// Up and Down exits
		outside.AddExit("up", library);
		outside.AddExit("down", lab);

        // Library
        library.AddExit("south", outside);
		library.AddExit("down", outside);

        // Pub
        pub.AddExit("east", outside);

        // Theatre
        theatre.AddExit("west", outside);

        // Gym
        gym.AddExit("southwest", outside);

        // Lab
        lab.AddExit("north", outside);
        lab.AddExit("south", office);
		lab.AddExit("up", outside);
		lab.AddExit("down", office);

        // Office
        office.AddExit("north", lab);
		office.AddExit("up", lab);

        // Create your Items here
        // ...
        // And add them to the Rooms
        // ...

        // Start game outside
        player.CurrentRoom = outside;

		Item blade = new Item("Blade of Agility", 1);
		Item pBooster = new Item("Point Booster", 5);
		Item oAxe = new Item("Ogre Axe", 6);
		Item wStaff = new Item("Wizardy Staff", 3);

		pub.Chest.Put("Blade of Agility", blade);
		theatre.Chest.Put("Point Booster", pBooster);
		gym.Chest.Put("Ogre Axe",  oAxe);
		lab.Chest.Put("Wizardy Staff", wStaff);

    }

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

        // Enter the main command loop. Here we repeatedly read commands and
        // execute them until the player wants to quit.
        bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
        
    }

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if(command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				// implement look command here
				Console.WriteLine(player.CurrentRoom.GetLongDescription());
				break;
			case "status":
				if (player.isAlive())
				{
					Console.WriteLine($"You are alive! Your HP: {player.Health}");
					Console.WriteLine($"Your inventory: {player.backpack.Show()}");
				}
				else
				{
					Console.WriteLine("YOU ARE DEAD...");
				}
				break;
            case "take":
				Take();
				break;
			case "drop":
				Drop();
				break;
        }

        return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################
	
	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if(!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to "+direction+"!");
			return;
		}

		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
        player.Damage(5);
        Console.WriteLine($"You are damaged for 5 HP. Your current health is {player.Health}");
		checkAlive();
    }
	private void checkAlive()
	{
        if (!player.isAlive())
        {
            Console.WriteLine("YOU ARE DEAD...");
            Console.WriteLine("Game Over!");
            Environment.Exit(0);
        }
    }
	private void Take(){
		var kv = player.CurrentRoom.Chest.items.FirstOrDefault();

		if (kv.Value != null)
		{		
			Console.WriteLine("Success");
			player.TakeFromChest(kv.Value.Description);
		}
		else
		{
			Console.WriteLine("Chest is empty");
		}
	}
	public void Drop()
	{
        var kv = player.backpack.items.FirstOrDefault();

        if (kv.Value != null)
        {
            Console.WriteLine("Success");
            player.DropToChest(kv.Value.Description);
        }
        else
        {
            Console.WriteLine("Backpack is empty");
        }
    }
}
 
