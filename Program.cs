/*
Class for location
Class for Mouse
Class for Obstacle

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

// Stats class to hold character attributes

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

public static class Program
{
    private const int TypeDelayMilliseconds = 25;
    private const int InventorySelectionCount = 2;
    private const int FailureDamage = 25;
    private static readonly Random _random = new();

    private static readonly InventoryItem[] InventoryOptions =
    {
        new("sword", "Toothpick Sword", "Toothpick Sword (stabby)"),
        new("slingshot", "Slingshot", "Rubber Band Slingshot (ranged)"),
        new("pepper", "Chili Pepper", "Chili Pepper (distraction)"),
        new("shield", "Bottlecap Shield", "Bottlecap Shield (defense)"),
        new("thread", "Spool Thread", "Spool Thread (grappling line)"),
        new("crumb", "Cheese Crumb", "Cheese Crumb (bribe distractions)"),
        new("button", "Lucky Button", "Lucky Button (mystic charm)"),
        new("marble", "Marble", "Marble (rolling getaway)")
    };
    private static readonly IReadOnlyList<Scene> AdventureScenes = SceneLibrary.Scenes;

        public static void Main(string[] args)
        {
            DisplayWelcomeMessage();
            var character = CreateCharacter();
            Console.WriteLine();
            Console.WriteLine($"Welcome, {character.Name}!");
            Console.WriteLine(
                $"Inventory: {string.Join(", ", character.Inventory.Select(item => item.ShortName))}");
            Console.WriteLine($"Health: {character.Stats.Health}");

            PlayScenes(character);
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

        private static CharacterProfile CreateCharacter()
        {
            Console.WriteLine();
            Console.WriteLine("🧀 1. Character Creation");
            Console.WriteLine();
            Console.WriteLine("Welcome, little mouse!");
            Console.WriteLine("What is your name?");
            Console.Write("> ");

            var name = ReadNonEmptyLine();

            // Quiz to determine stats
        var stats = new Stats();

        Console.WriteLine();
        Console.WriteLine($"Well howdy do, {name}! Answer a few questions to shape your stats:");

        // Question 1: Strength
        Console.WriteLine("Q1: You find a heavy piece of cheese stuck under a crate. Do you:");
        Console.WriteLine("1) Try to lift it yourself");
        Console.WriteLine("2) Look for a lever or tool to help");
        Console.WriteLine("3) Leave it alone");
        Console.Write("> ");
        var choice = Console.ReadLine();
        if (choice == "1") stats.Strength += 5;
        else if (choice == "2") stats.Intelligence += 5;
        else stats.Agility += 2;

        // Question 2: Agility
        Console.WriteLine("\nQ2: A cat is chasing you! Do you:");
        Console.WriteLine("1) Run as fast as you can");
        Console.WriteLine("2) Hide behind something");
        Console.WriteLine("3) Try to outsmart it");
        Console.Write("> ");
        choice = Console.ReadLine();
        if (choice == "1") stats.Agility += 5;
        else if (choice == "2") stats.Charisma += 2; // maybe calm other mice?
        else stats.Intelligence += 3;

        // Question 3: Charisma
        Console.WriteLine("\nQ3: You meet another mouse. Do you:");
        Console.WriteLine("1) Share your cheese");
        Console.WriteLine("2) Ignore them");
        Console.WriteLine("3) Try to trick them");
        Console.Write("> ");
        choice = Console.ReadLine();
        if (choice == "1") stats.Charisma += 5;
        else if (choice == "2") stats.Strength += 2;
        else stats.Intelligence += 2;

        Console.WriteLine();
        Console.WriteLine($"Your starting stats are: Health={stats.Health}, Strength={stats.Strength}, Agility={stats.Agility}, Intelligence={stats.Intelligence}, Charisma={stats.Charisma}");


        Console.WriteLine();
        Console.WriteLine($"Choose {InventorySelectionCount} items to carry in your tiny backpack:");
        for (var i = 0; i < InventoryOptions.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {InventoryOptions[i].Description}");
        }

        Console.WriteLine();
        Console.WriteLine($"Enter {InventorySelectionCount} numbers separated by a space:");
        var inventory = ReadInventorySelection();

        // Create character with quiz-modified stats
        var character = new CharacterProfile(name, inventory);
        character.Stats.Health = stats.Health;
        character.Stats.Strength = stats.Strength;
        character.Stats.Agility = stats.Agility;
        character.Stats.Intelligence = stats.Intelligence;
        character.Stats.Charisma = stats.Charisma;

        return character;
    }

        private static string ReadNonEmptyLine()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                Console.Write("> ");
            }
        }

        private static List<InventoryItem> ReadInventorySelection()
        {
            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                var tokens = input
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length != InventorySelectionCount)
                {
                    Console.WriteLine($"Please enter exactly {InventorySelectionCount} numbers (e.g., 2 4).");
                    continue;
                }

                var uniqueSelections = new HashSet<int>();
                var orderedSelections = new List<int>();
                var valid = true;
                foreach (var token in tokens)
                {
                    if (!int.TryParse(token, out var number) ||
                        number < 1 || number > InventoryOptions.Length)
                    {
                        valid = false;
                        break;
                    }

                    if (!uniqueSelections.Add(number))
                    {
                        valid = false;
                        break;
                    }

                    orderedSelections.Add(number);
                }

                if (!valid)
                {
                    Console.WriteLine("Please pick two different numbers from the list.");
                    continue;
                }

                return orderedSelections
                    .Select(n => InventoryOptions[n - 1])
                    .ToList();
            }
        }

        private sealed class CharacterProfile
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

    private sealed record InventoryItem(string Key, string ShortName, string Description);
    private sealed record ItemOutcome(
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

    private static void PlayScenes(CharacterProfile character)
    {
        foreach (var scene in AdventureScenes)
        {
            PlayScene(scene, character);
                            if (character.Stats.Health <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Your health has dropped to zero.");
                    Console.WriteLine("Your quest ends here. The Camembert remains a distant dream.");
                    Console.WriteLine();
                    Console.WriteLine("GAME OVER");
                    return; // Exit the loop and the method
                }
            }

            CelebrateCamembert(character);
    }

    private static class SceneLibrary
    {
        public static IReadOnlyList<Scene> Scenes { get; } = new[]
        {
            CreateCatVsHoover(),
            CreateWorksurface(),
            CreateCurtainClimb(),
            CreatePantrySentries(),
            CreateToySoldiers()
        };

        private static Scene CreateCatVsHoover() =>
            new(
                "CAT vs HOOVER",
                """
You enter the living room.
To the LEFT: you hear the slow, hungry purr of the household cat…
To the RIGHT: the hoover sits dormant, but the switch is twitching like it might turn on.
""",
                "LEFT (Cat)",
                new ScenePath(
                    """
The CAT prowls toward you, eyes locked on your tail.
You must act fast!
""",
                    "Choose an item to use:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["slingshot"] = new ItemOutcome("""
You fire a tiny breadcrumb from your slingshot!
The cat flinches and gives you a window to escape.
You scurry to safety!
"""),
                        ["shield"] = new ItemOutcome("""
You hold the bottlecap shield between you and the cat's claws.
It clangs off the shield and loses interest.
You dart away while it licks its paw.
""")
                    },
                    item => new ItemOutcome(
                        $"You brandish {item.ShortName}, but the cat swats it back into your whiskers. Ouch!",
                        true,
                        FailureDamage),
                            "Reason with it",
                            character =>
                            {
                                var roll = _random.Next(1, 21);
                                var result = roll + character.Stats.Charisma;
                                const int difficulty = 18;
                                if (result > difficulty)
                                {
                                    return new ItemOutcome(
                                        $"You end up tell him the funniest joke in the world! You manage to get past while it chuckles to itself. You sadly can't remember it, this is just a tribute... (Rolled {roll} + Charisma {character.Stats.Charisma} = {result} vs Difficulty {difficulty})");
                                }
                                return new ItemOutcome(
                                    $"You try to reason, but the cat is unimpressed and swipes at you. (Rolled {roll} + Charisma {character.Stats.Charisma} = {result} vs Difficulty {difficulty})",
                                    true, FailureDamage
                                );
                            }),
                "RIGHT (Hoover)",
                new ScenePath(
                    """
As you approach, the Hoover suddenly ROARS to life!
Dust and hair swirl everywhere.
""",
                    "Choose an item to use:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["slingshot"] = new ItemOutcome("""
You launch a pebble at the power switch.
The hoover sputters off and you zip past the hose.
"""),
                        ["shield"] = new ItemOutcome("""
You crouch behind your bottlecap as debris flies.
You survive the storm and crawl onward.
""")
                    },
                    item => new ItemOutcome(
                        $"You wave {item.ShortName}, but the hoover gust blasts it into you.",
                        true,
                        FailureDamage),
                            "Jump over it to safety",
                            character =>
                            {
                                var roll = _random.Next(1,21);
                                var result = roll + character.Stats.Agility;
                                const int difficulty = 12;
                                if (result > difficulty)
                                {
                                    return new ItemOutcome(
                                        $"You front flip over the hoover with style! (Rolled {roll} + Agility {character.Stats.Agility} = {result} vs Difficulty {difficulty})");
                                } 
                                return new ItemOutcome(
                                    $"You try to jump, but get caught in the gust and slammed back down. (Rolled {roll} + Agility {character.Stats.Agility} = {result} vs Difficulty {difficulty})",
                                    true, FailureDamage
                                );
                            }));

        private static Scene CreateWorksurface() =>
            new(
                "THE WORKSURFACE",
                """
You climb onto the kitchen counter.
LEFT: A row of shiny KNIVES. The handles look slippery.
RIGHT: A BLENDER sits open… and someone’s hand just reached for the power button.
""",
                "LEFT (Knives)",
                new ScenePath(
                    """
You slip onto the cutting board. One wrong move and—
A knife starts tipping toward you!
""",
                    "Choose an item:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["slingshot"] = new ItemOutcome("""
You snap a rubber band at the falling knife.
It nudges the blade aside just enough for you to scurry past the handle.
"""),
                        ["shield"] = new ItemOutcome("""
You brace the bottlecap above you.
The knife clangs on the metal and sticks in the board while you dive to safety.
""")
                    },
                    item => new ItemOutcome(
                        $"You raise {item.ShortName}, but the knife hammers it back into your paws.",
                        true,
                        FailureDamage)),
                "RIGHT (Blender)",
                new ScenePath(
                    """
The blender whirs to life!
The air vortex is pulling you toward it.
""",
                    "Use an item to escape:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["slingshot"] = new ItemOutcome("""
You aim at the power button and let fly.
The button pops back out and the blades grind to a halt.
"""),
                        ["shield"] = new ItemOutcome("""
You grip the countertop with one paw and anchor the shield behind a knob.
The suction tugs at you, but you hold on until the blender powers down.
""")
                    },
                    item => new ItemOutcome(
                        $"You cling to {item.ShortName}, but the blender's wind pelts it back at you.",
                        true,
                        FailureDamage)));

        private static Scene CreateCurtainClimb() =>
            new(
                "CURTAIN CLIMB",
                """
You slip into the dining room where enormous velvet curtains sway.
LEFT: Climb the tasseled cord toward the curtain rod lookout.
RIGHT: Dash along the window ledge and leap to the chandelier.
""",
                "LEFT (Curtain Cord)",
                new ScenePath(
                    """
The cord is slick with polish and twice as thick as you are.
If you fall, it's a long drop to the marble floor.
""",
                    "Choose something to steady your climb:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["thread"] = new ItemOutcome("""
You loop your spool thread around the cord like a harness.
Step by step you rappel upward until you reach the rod.
"""),
                        ["marble"] = new ItemOutcome("""
You wedge the marble beneath you as a foothold, rolling it up with each tug.
The improvised step keeps you glued to the cord, and you reach the top.
""")
                    },
                    item => new ItemOutcome(
                        $"You cling to {item.ShortName}, but it slips and smacks you as you tumble a few inches.",
                        true,
                        FailureDamage),
                        "Try to climb anyway",
                        character =>
                        {
                            var roll = _random.Next(1, 21); // d20 roll
                            var result = roll + character.Stats.Agility;
                            const int difficulty = 15;
                            if (result > difficulty)
                            {
                                return new ItemOutcome(
                                    $"You scramble up the cord with surprising grace! Your agility pays off. (Rolled {roll} + Agility {character.Stats.Agility} = {result} vs Difficulty {difficulty})");
                            }

                            return new ItemOutcome(
                                $"You try to climb, but lose your grip and fall. (Rolled {roll} + Agility {character.Stats.Agility} = {result} vs Difficulty {difficulty})",
                                true, FailureDamage);
                        }),
                "RIGHT (Chandelier Leap)",
                new ScenePath(
                    """
Wind rushes in from an open window, swinging the chandelier wildly.
Miss the timing and you'll be tossed into the sideboard.
""",
                    "Choose something to control the swing:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["thread"] = new ItemOutcome("""
You sling the thread like a lasso, snagging a chandelier arm.
With a quick yank you stabilize the swing and land neatly on the fixture.
"""),
                        ["marble"] = new ItemOutcome("""
You roll the marble down the ledge, letting it drop onto the chandelier chain.
The added weight evens out the sway, giving you a perfect landing.
""")
                    },
                    item => new ItemOutcome(
                        $"You toss {item.ShortName}, but the chandelier bats it back at your nose.",
                        true,
                        FailureDamage),
                        "Try to leap anyway",
                        character =>
                        {
                            var roll = _random.Next(1, 21); // d20 roll
                            var result = roll + character.Stats.Agility;
                            const int difficulty = 15;
                            if (result > difficulty)
                            {
                                return new ItemOutcome(
                                    $"You time your leap perfectly and land on the chandelier! Your agility is amazing! (Rolled {roll} + Agility {character.Stats.Agility} = {result} vs Difficulty {difficulty})");
                            }

                            return new ItemOutcome(
                                $"You misjudge the swing and get tossed into the sideboard. (Rolled {roll} + Agility {character.Stats.Agility} = {result} vs Difficulty {difficulty})",
                                true, FailureDamage);
                        }));

        private static Scene CreatePantrySentries() =>
            new(
                "PANTRY SENTRIES",
                """
Two pantry guardians stand between you and the cheese cupboard.
LEFT: A phalanx of flour-coated ants sniff the air for trespassers.
RIGHT: A spice rack rattles as a pepper mill prepares to sneeze out a storm.
""",
                "LEFT (Ant Patrol)",
                new ScenePath(
                    """
The lead ant clicks its mandibles, demanding tribute.
Without an offering they'll swarm your tail.
""",
                    "Choose something to appease them:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["crumb"] = new ItemOutcome("""
You toss a cheese crumb onto the floor.
The ants salute you and march after the snack, leaving a clear path.
"""),
                        ["pepper"] = new ItemOutcome("""
You wave the chili pepper like a torch.
The spicy fumes make the ants sneeze and scatter, giving you time to slip past.
""")
                    },
                    item => new ItemOutcome(
                        $"You offer {item.ShortName}, but the ants lob it back at your snout.",
                        true,
                        FailureDamage)),
                "RIGHT (Pepper Mill Gust)",
                new ScenePath(
                    """
The mill cranks faster and faster, whipping up a sneeze-spice cyclone.
You'll need to disrupt the blast or shield yourself.
""",
                    "Choose something to counter the storm:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["pepper"] = new ItemOutcome("""
You squeeze the chili pepper, launching seeds straight into the mill gears.
They jam at once and the gust dies out with a sputter.
"""),
                        ["crumb"] = new ItemOutcome("""
You crumble cheese into the airflow.
The mill gets clogged with gooey goodness and grinds to a halt while you scoot by.
""")
                    },
                    item => new ItemOutcome(
                        $"You brandish {item.ShortName}, but the pepper gale whiplashes it into you.",
                        true,
                        FailureDamage)));

        private static Scene CreateToySoldiers() =>
            new(
                "TOY SOLDIER GAUNTLET",
                """
The nursery hallway is patrolled by wind-up soldiers and a towering doll captain.
LEFT: March straight through the soldier line.
RIGHT: Slip behind the captain's command post.
""",
                "LEFT (Soldier Line)",
                new ScenePath(
                    """
Tin sabers flash as the soldiers stomp toward you.
You'll need to parry or break their formation.
""",
                    "Choose something to duel with:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["sword"] = new ItemOutcome("""
You draw your toothpick sword and tap-tap parry the sabers.
One fancy twirl later, the soldiers bow and let you pass.
"""),
                        ["button"] = new ItemOutcome("""
You polish the lucky button and hold it high.
Its gleam dazzles the tin troops, freezing them mid-step while you weave through.
""")
                    },
                    item => new ItemOutcome(
                        $"You raise {item.ShortName}, but a tin boot punts it right back into your chest.",
                        true,
                        FailureDamage)),
                "RIGHT (Command Post)",
                new ScenePath(
                    """
The doll captain holds the winding key to reactivate the patrol.
If she turns it, you'll be surrounded.
""",
                    "Choose something to distract her:",
                    new Dictionary<string, ItemOutcome>
                    {
                        ["button"] = new ItemOutcome("""
You flick the lucky button like a coin across the floor.
The captain follows the shiny arc, leaving the winding key unguarded so you zip past.
"""),
                        ["sword"] = new ItemOutcome("""
You tap the captain's boot with the toothpick sword.
She topples like a felled tree, harmlessly blocking the key.
""")
                    },
                    item => new ItemOutcome(
                        $"You try {item.ShortName}, but the doll captain bats it back like a paddle.",
                        true,
                        FailureDamage)));
    }

        private static void PlayScene(Scene scene, CharacterProfile character)
        {
            Console.WriteLine();
            Console.WriteLine($"SCENE: {scene.Title}");
            Console.WriteLine();
            Console.WriteLine(scene.Intro);
            Console.WriteLine("Which way do you go?");
            Console.WriteLine($"1. {scene.LeftChoiceLabel}");
            Console.WriteLine($"2. {scene.RightChoiceLabel}");

            var direction = ReadMenuChoice(2);
            var path = direction == 1 ? scene.LeftPath : scene.RightPath;

            Console.WriteLine();
            Console.WriteLine(path.Description);
            Console.WriteLine(path.ItemPrompt);

            var usableItems = character.Inventory
                .Where(item => path.ItemOutcomes.ContainsKey(item.Key))
                .ToList();

            var hasStatCheckOption = path.StatCheck is not null && path.StatCheckPrompt is not null;

            if (!usableItems.Any() && !hasStatCheckOption)
            {
                Console.WriteLine("Uh-oh! None of your items help here. You improvise and barely escape!");
                return;
            }

            for (var i = 0; i < usableItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {usableItems[i].ShortName}");
            }

            if (hasStatCheckOption)
            {
                Console.WriteLine($"{usableItems.Count + 1}. {path.StatCheckPrompt}");
            }

            var choiceCount = usableItems.Count + (hasStatCheckOption ? 1 : 0);
            var menuChoice = ReadMenuChoice(choiceCount);

            Console.WriteLine();
            ItemOutcome outcome;
            if (menuChoice <= usableItems.Count)
            {
                var selectedItem = usableItems[menuChoice - 1];
                outcome = path.ItemOutcomes[selectedItem.Key].Resolve(character, selectedItem);
            }
            else
            {
                // This must be a stat check choice
                outcome = path.StatCheck!.Invoke(character);
            }

            Console.WriteLine(outcome.Description);
            if (outcome.CausesDamage)
            {
                character.Stats.LoseHealth(outcome.DamageAmount);
                Console.WriteLine(
                    $"You lose {outcome.DamageAmount} health! Current health: {character.Stats.Health}");
            }
    

        Console.WriteLine("\nPress ENTER to continue...");
        Console.ReadLine();
    }

        private static int ReadMenuChoice(int optionCount)
        {
            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out var number) &&
                    number >= 1 &&
                    number <= optionCount)
                {
                    return number;
                }

                Console.WriteLine($"Enter a number between 1 and {optionCount}.");
            }
        }

        private sealed class Scene
        {
            public Scene(
                string title,
                string intro,
                string leftChoiceLabel,
                ScenePath leftPath,
                string rightChoiceLabel,
                ScenePath rightPath)
            {
                Title = title;
                Intro = intro;
                LeftChoiceLabel = leftChoiceLabel;
                LeftPath = leftPath;
                RightChoiceLabel = rightChoiceLabel;
                RightPath = rightPath;
            }

            public string Title { get; }
            public string Intro { get; }
            public string LeftChoiceLabel { get; }
            public ScenePath LeftPath { get; }
            public string RightChoiceLabel { get; }
            public ScenePath RightPath { get; }
        }

        private sealed class ScenePath
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
                foreach (var item in InventoryOptions)
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
            new($"Oh no! You fumble with {item.ShortName}. Not good!", true, FailureDamage);
    }

        private static void CelebrateCamembert(CharacterProfile character)
        {
            Console.WriteLine();
            Console.WriteLine("🏁 You reach the heart of the pantry...");
            Console.WriteLine("A golden glow spills from a tiny cheese dome.");
            Console.WriteLine();

            var asciiCheese = """
                              ______________________________________________¶¶¶¶¶¶¶¶¶¶¶
                              ___________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶
                              __________________________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶___________¶¶¶¶¶
                              _____________________¶¶¶¶¶¶¶¶__________________________¶¶¶¶¶¶
                              __________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_____________¶¶¶¶¶¶¶___________¶¶¶¶¶¶
                              ____¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_____________¶¶¶44444¶¶____________¶¶¶¶¶
                              ¶¶¶¶¶¶¶¶____________________________444444444444___________¶¶¶¶¶
                              ¶¶¶¶¶¶¶¶_______________________________44444_______________¶¶¶¶
                              _¶¶¶¶¶¶¶¶¶¶¶¶¶¶___________________________________________¶¶¶¶¶
                              _¶¶¶¶__¶¶¶¶¶¶¶¶¶¶¶¶¶_______________________________________¶¶¶¶¶
                              _¶¶¶¶________¶¶¶¶¶¶¶¶¶¶¶¶¶____444___________________________¶¶¶¶¶¶¶
                              _¶¶¶¶_____________¶¶¶¶¶¶¶¶¶¶¶¶¶¶444444______________¶¶¶¶¶¶_____¶¶¶¶¶¶¶
                              __¶¶¶¶_4______________¶¶¶¶¶¶¶¶¶¶¶¶¶444______________¶4444¶_________¶¶¶¶
                              __¶¶¶¶_44_____________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_____________4444444_______¶¶¶¶¶
                              __¶¶¶¶__4_______________¶¶¶¶¶¶¶¶¶¶_¶¶¶¶¶¶¶¶¶¶¶¶¶¶__________444________¶¶¶¶¶
                              __¶¶¶¶__44_______________________________¶¶¶¶¶¶¶¶¶¶¶¶¶_________________¶¶¶¶¶
                              __¶¶¶¶__444___________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶____________¶¶¶¶¶¶
                              __¶¶¶¶__444________________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶¶________¶¶¶¶¶
                              __¶¶¶¶¶_4444______________________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶___¶¶¶¶¶¶
                              ___¶¶¶¶__444___________________________________¶¶¶¶¶¶¶¶¶¶_____¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶
                              ___¶¶¶¶__4444________¶¶¶¶¶¶¶_________________¶¶4444444¶¶¶¶¶¶¶_______¶¶¶¶¶¶¶¶¶¶¶¶
                              ___¶¶¶¶__44444_____¶¶¶¶¶¶¶¶¶¶¶¶_____________444444444444444¶¶¶____________¶¶¶¶¶¶
                              ___¶¶¶¶¶__44444__¶¶¶444444444¶¶¶______________4444444444444¶_______________¶¶¶¶¶
                              ___¶¶¶¶¶__44444__44444444444444____________________________________________¶¶¶¶¶
                              ___¶¶¶¶¶__4444444_444444444444____________________________________________¶¶¶¶¶
                              ___¶¶¶¶¶__4444444____444444_______________________________________________¶¶¶¶¶
                              ____¶¶¶¶___44444444_____________________________________________________¶¶¶¶¶¶¶
                              ____¶¶¶¶___444444444___________________________________________________¶¶¶¶¶¶¶¶
                              ____¶¶¶¶¶¶_____4444444________________________________________________¶¶¶¶¶¶¶¶¶
                              ____¶¶¶¶¶¶¶¶¶¶¶¶______________________________________________________¶¶¶¶¶¶¶¶
                              _________¶¶¶¶¶¶¶¶¶¶¶¶________________________________¶¶¶______________¶¶¶¶¶¶¶¶
                              _____________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶______________¶¶¶¶¶¶¶¶__________¶¶¶¶¶¶¶¶
                              __________________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶____________444444¶¶¶¶________¶¶¶¶¶¶¶¶
                              _______________________¶¶¶¶¶______¶¶¶¶¶¶¶____________44444444¶¶¶_______¶¶¶¶¶¶
                              ____________________________________¶¶¶¶¶¶____________444444444¶¶______¶¶¶¶¶¶
                              _____________________________________¶¶¶¶¶¶¶¶¶¶_________44444444¶¶_____¶¶¶¶¶¶
                              ______________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶¶_______44444¶______¶¶¶¶¶¶
                              ___________________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶______________¶¶¶¶¶¶¶
                              _________________________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶________¶¶¶¶¶¶¶
                              ______________________________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶¶__¶¶¶¶¶¶
                              ___________________________________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶
                              ________________________________________________________________¶¶¶¶¶¶¶¶¶¶¶¶
                              __________________________________________________________¶¶¶¶¶¶¶
                              """;

            Console.WriteLine(asciiCheese);
            Console.WriteLine();
            Console.WriteLine($"Congratulations, {character.Name}! The Camembert is yours!");
        }
    }
}