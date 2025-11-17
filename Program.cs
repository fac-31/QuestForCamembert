/*
Class for location
Class for Mouse
Class for Obstacle

*/


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
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Quest for Camembert!");
    }
}