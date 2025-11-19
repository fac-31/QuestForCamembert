using System.Collections.Generic;

namespace QuestForCamembert
{
    public sealed class CharacterProfile
    {
        public CharacterProfile(string name, List<InventoryItem> inventory, Stats stats)
        {
            Name = name;
            Inventory = inventory;
            Stats = stats;
        }

        public string Name { get; }
        public List<InventoryItem> Inventory { get; }
        public Stats Stats { get; }
    }
}
