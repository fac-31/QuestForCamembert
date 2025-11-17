/*
Class for location
Class for Mouse
Class for Obstacle

*/

using System.Text;
using System.Threading;

// Stats class to hold character attributes
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
    public string Name {get; set;}
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

public class Tom : Character
{
    public Tom(string name) : base(name)
    {
        this.CharacterStats.Charisma += 20;
    }
}

public static class Program
{
    private const int TypeDelayMilliseconds = 25;

    public static void Main(string[] args)
    {
        DisplayWelcomeMessage();
    }

    private static void DisplayWelcomeMessage()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var welcomeText = """
==========================================
🧀  WELCOME TO QUEST FOR CAMEMBERT!!  🧀
==========================================

You're a brave little mouse in a BIG house looking for Camembert!

Race across the board,
avoid the traps,
and outsmart anyone trying to catch you.

Press ENTER to scurry inside...
""";

        TypeOut(welcomeText, TypeDelayMilliseconds);
        Console.ReadLine();
    }

    private static void TypeOut(string text, int delayMilliseconds)
    {
        foreach (var character in text)
        {
            Console.Write(character);
            Thread.Sleep(delayMilliseconds);
        }
    }
}
