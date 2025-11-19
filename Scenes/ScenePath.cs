using System;
using System.Collections.Generic;

namespace QuestForCamembert
{
    public sealed class ScenePath
    {
        public ScenePath(
            string description,
            string itemPrompt,
            Dictionary<string, ItemOutcome> itemOutcomes,
            Func<InventoryItem, ItemOutcome>? failureOutcomeFactory = null,
            string? statCheckPrompt = null,
            Func<CharacterProfile, ItemOutcome>? statCheck = null)
        {
            Description = description;
            ItemPrompt = itemPrompt;
            ItemOutcomes = EnsureAllItemOutcomes(itemOutcomes, failureOutcomeFactory);
            StatCheckPrompt = statCheckPrompt;
            StatCheck = statCheck;
        }

        public string Description { get; }
        public string ItemPrompt { get; }
        public Dictionary<string, ItemOutcome> ItemOutcomes { get; }
        public string? StatCheckPrompt { get; }
        public Func<CharacterProfile, ItemOutcome>? StatCheck { get; }

        private static Dictionary<string, ItemOutcome> EnsureAllItemOutcomes(
            Dictionary<string, ItemOutcome> provided,
            Func<InventoryItem, ItemOutcome>? failureOutcomeFactory)
        {
            var completed = new Dictionary<string, ItemOutcome>(provided);
            foreach (var item in Program.InventoryOptions)
            {
                if (!completed.ContainsKey(item.Key))
                {
                    var fallback = (failureOutcomeFactory ?? DefaultFailureOutcomeFactory).Invoke(item);
                    completed[item.Key] = fallback;
                }
            }

            return completed;
        }

    private static ItemOutcome DefaultFailureOutcomeFactory(InventoryItem item) =>
        new($"Oh no! You fumble with {item.ShortName}. Not good!", true, Program.FailureDamage);
    }
}
