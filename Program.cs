/*
Class for location
Class for Mouse
Class for Obstacle

*/


// Stats class to hold character attributes

namespace QuestForCamembert
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
    public class Game()
    {
        public void Start()
        {
            // Game loop
            bool isRunning = true;

            while (isRunning)
            {
                Character player = chooseCharacter();
                Console.WriteLine($"You have chosen: {player.Name}");
                isRunning = false; // End the game after character selection for now
            }
        }
        public Character chooseCharacter()
    {
        Console.WriteLine("Choose your character:");
        Console.WriteLine("1. Danger Mouse");
        Console.WriteLine("2. Sherlock Mouse");
        Console.WriteLine("3. Micky Mouse");

        string? choice = Console.ReadLine();
        Character playerCharacter;

        switch (choice)
        {
            case "1":
                playerCharacter = new DangerMouse("Danger Mouse");
                break;
            case "2":
                playerCharacter = new SherlockMouse("Sherlock Mouse");
                break;
            case "3":
                playerCharacter = new MickyMouse("Micky Mouse");
                break;
            default:
                Console.WriteLine("Invalid choice, defaulting to Danger Mouse.");
                playerCharacter = new DangerMouse("Danger Mouse");
                break;
        }

        return playerCharacter;
    }
    }
    public class Stats
    {
        public int Health { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Agility { get; set; } = 10;
        public int Intelligence { get; set; } = 10;

        public int Charisma { get; set; } = 10;

        public void LoseHealth(int amount)
        {
            Health = Math.Max(0, Health - amount);
        }
        public void GainHealth(int amount)
        {
            Health += amount;
        }
    }

    // Abstract base class for characters
    public abstract class Character
    {
        public string Name { get; set; }
        public Stats CharacterStats { get; private set; } = new Stats();
        public Character(string name)
        {
            this.Name = name;
        }
    }
    // First Character subclass
    public class DangerMouse : Character
    {
        public DangerMouse(string name) : base(name)
        {
            this.CharacterStats.Strength += 5;
        }
    }

    public class SherlockMouse : Character
    {
        public SherlockMouse(string name) : base(name)
        {
            this.CharacterStats.Intelligence += 5;
        }
    }

    public class MickyMouse : Character
    {
        public MickyMouse(string name) : base(name)
        {
            this.CharacterStats.Charisma += 20;
        }
    }
}