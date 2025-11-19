using System;

namespace QuestForCamembert
{
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
}
