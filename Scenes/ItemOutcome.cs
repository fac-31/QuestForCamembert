using System;

namespace QuestForCamembert
{
    public sealed record ItemOutcome(
        string Description, 
        bool CausesDamage = false, 
        int DamageAmount = 0,
        Func<CharacterProfile, InventoryItem, ItemOutcome>? DynamicOutcome = null
    )
    {
        public ItemOutcome Resolve(CharacterProfile character, InventoryItem item)
        {
            if (DynamicOutcome != null)
                return DynamicOutcome(character, item);
            return this;
        }
    };
}
