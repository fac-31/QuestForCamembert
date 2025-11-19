using System;
using System.Collections.Generic;

namespace QuestForCamembert
{
    public static class SceneLibrary
    {
        private static readonly Random _random = new();

        public static IReadOnlyList<Scene> Scenes { get; } = new[]
        {
            CreateCatVsHoover(),
            CreateWorksurface(),
            CreateCurtainClimb(),
            BuildPantrySentriesScene(),
            BuildToySoldiersScene()
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
                        ["slingshot"] = new ItemOutcome(
                    "You fire a tiny breadcrumb from your slingshot!",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Agility >= 15)
                            return new ItemOutcome("You fire with perfect aim! The cat jumps aside and you escape unscathed.");
                        return new ItemOutcome("The cat flinches slightly, but you stumble and barely escape.", true, Program.FailureDamage);
                    }
                ),
                        ["shield"] = new ItemOutcome("You hold the bottlecap shield between you and the cat's claws.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Strength >= 15)
                            return new ItemOutcome("You brace the shield firmly. The cat loses interest and walks away.");
                        return new ItemOutcome("The cat knocks the shield aside and swipes at you!", true, Program.FailureDamage);
                    }),  ["marble"] = new ItemOutcome(
                    "You roll the marble towards the cat...",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Charisma >= 15)
                            return new ItemOutcome("The cat is distracted by your charming antics while the marble rolls away. You escape safely!");
                        return new ItemOutcome("The cat bats at the marble. You barely scurry away.", true, Program.FailureDamage);
                    }
                )

                    },
                    item => new ItemOutcome(
                        $"You brandish {item.ShortName}, but the cat swats it back into your whiskers. Ouch!",
                        true,
                        Program.FailureDamage),
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
                                    true, Program.FailureDamage
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
                ["slingshot"] = new ItemOutcome(
                    "You launch a pebble at the power switch.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Intelligence >= 15)
                            return new ItemOutcome("Your aim is precise! The hoover sputters off and you zip past the hose safely.");
                        return new ItemOutcome("The pebble bounces harmlessly. You dodge debris as best you can.", true, Program.FailureDamage);
                    }
                ),
                ["shield"] = new ItemOutcome(
                    "You crouch behind your bottlecap as debris flies.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Agility >= 15)
                            return new ItemOutcome("You move swiftly, using the shield to survive the gust unscathed.");
                        return new ItemOutcome("The wind blows the shield into you! Ouch.", true, Program.FailureDamage);
                    }
                )
            },
                    item => new ItemOutcome(
                        $"You wave {item.ShortName}, but the hoover gust blasts it into you.",
                        true,
                        Program.FailureDamage),
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
                                    true, Program.FailureDamage
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
                ["slingshot"] = new ItemOutcome(
                    Description: "Snap a rubber band to manipulate the knife.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Intelligence >= 15)
                            return new ItemOutcome("You cleverly snap a rubber band to nudge the knife aside safely.");
                        return new ItemOutcome("The knife tips closer! You barely dodge.", true, Program.FailureDamage);
                    }
                ),
                ["shield"] = new ItemOutcome(
                    Description: "Use the shield to block the knife.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Strength >= 15)
                            return new ItemOutcome("You brace the shield firmly; the knife clangs off safely.");
                        return new ItemOutcome("The knife clangs off the shield but grazes your paw!", true, Program.FailureDamage);
                    }
                )
                    },
                    item => new ItemOutcome(
                        $"You raise {item.ShortName}, but the knife hammers it back into your paws.",
                        true,
                        Program.FailureDamage),
                    null,
                    null),
                "RIGHT (Blender)",
                new ScenePath(
                    """
The blender whirs to life!
The air vortex is pulling you toward it.
""",
                    "Use an item to escape:",
                    new Dictionary<string, ItemOutcome>
                                {
                ["slingshot"] = new ItemOutcome(
                    Description: "Shoot a pebble at the blender button.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Intelligence >= 15)
                            return new ItemOutcome("You hit the button perfectly; the blender stops, and you escape.");
                        return new ItemOutcome("The pebble misses the button! You dodge debris frantically.", true, Program.FailureDamage);
                    }
                ),
                ["shield"] = new ItemOutcome(
                    Description: "Block the wind with the shield.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Agility >= 15)
                            return new ItemOutcome("You maneuver behind the shield, holding steady as the blender sucks harmlessly past.");
                        return new ItemOutcome("The wind throws the shield into you!", true, Program.FailureDamage);
                    }
                )
                    },
                    item => new ItemOutcome(
                        $"You cling to {item.ShortName}, but the blender's wind pelts it back at you.",
                        true,
                        Program.FailureDamage),
                    null,
                    null));

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
                ["thread"] = new ItemOutcome(
                    Description: "Tie a thread for a secure grip.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Intelligence >= 15)
                            return new ItemOutcome("You fashion a secure harness with the thread and climb safely.");
                        return new ItemOutcome("The thread slips a little! You hang on tightly and reach the top.", true, Program.FailureDamage / 2);
                    }
                ),
                ["marble"] = new ItemOutcome(
                    Description: "Hold the marble as a counterweight.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Charisma >= 15)
                            return new ItemOutcome("Using your clever distraction tactics, the marble rolls perfectly, helping you ascend.");
                        return new ItemOutcome("The marble wobbles as you climb. A small slip bruises your paw.", true, Program.FailureDamage / 2);
                    }
                )
                    },
                    item => new ItemOutcome(
                        $"You cling to {item.ShortName}, but it slips and smacks you as you tumble a few inches.",
                        true,
                        Program.FailureDamage),
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
                                true, Program.FailureDamage);
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
                ["thread"] = new ItemOutcome(
                    Description: "Hook a thread to steady the chandelier.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Agility >= 15)
                            return new ItemOutcome("You hook the thread expertly, stabilize the chandelier, and land safely.");
                        return new ItemOutcome("You grab the thread, but swing wildly and bump yourself.", true, Program.FailureDamage / 2);
                    }
                ),
                ["marble"] = new ItemOutcome(
                    Description: "Use the marble to balance your leap.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Charisma >= 15)
                            return new ItemOutcome("You roll the marble strategically; it steadies the chandelier, and you land perfectly.");
                        return new ItemOutcome("The marble slides off! You tumble slightly.", true, Program.FailureDamage / 2);
                    }
                )
            },
                    item => new ItemOutcome(
                        $"You toss {item.ShortName}, but the chandelier bats it back at your nose.",
                        true,
                        Program.FailureDamage),
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
                                true, Program.FailureDamage);
                        }));

        private static Scene BuildPantrySentriesScene() => new(
               "PANTRY SENTRIES",
               """
        You slip into the pantry. Shelves tower above, jars rattling ominously.
        LEFT: Two stacked cans wobble like watchful guards.
        RIGHT: A jar of pickles teeters near the edge.
        """,
               "LEFT (Cans)",
               new ScenePath(
                   "The cans sway precariously. One wrong move and you'll be buried under the clang!",
                   "Choose an item:",
                   new Dictionary<string, ItemOutcome>
                   {
                ["slingshot"] = new ItemOutcome(
                    Description: "Launch a pebble to topple the cans safely.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Agility >= 15)
                            return new ItemOutcome("You launch a pebble with precision. The cans topple harmlessly away from you.");
                        return new ItemOutcome("The pebble misses! A can topples and knocks you slightly off balance.", true, Program.FailureDamage / 2);
                    }
                ),
                ["marble"] = new ItemOutcome(
                    Description: "Roll a marble to nudge the cans.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Intelligence >= 15)
                            return new ItemOutcome("You roll the marble cleverly; it knocks the lower can into place, and you slip past safely.");
                        return new ItemOutcome("The marble rolls unpredictably, brushing your paw. You dodge just in time.", true, Program.FailureDamage / 2);
                    }
                ),
                ["shield"] = new ItemOutcome(
                    Description: "Brace a shield against falling cans.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Strength >= 15)
                            return new ItemOutcome("You brace the shield. The cans clang against it, but you remain safe.");
                        return new ItemOutcome("The shield slips under the clang! You get slightly hit.", true, Program.FailureDamage / 2);
                    }
                )
                   }
               ),
               "RIGHT (Pickle Jar)",
               new ScenePath(
                   "The pickle jar tilts closer to the edge with each step you take.",
                   "Choose an item to secure it:",
                   new Dictionary<string, ItemOutcome>
                   {
                ["slingshot"] = new ItemOutcome(
                    Description: "Use the slingshot to nudge the jar safely.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Intelligence >= 15)
                            return new ItemOutcome("You nudge the jar with perfect aim; it settles safely and you slip past.");
                        return new ItemOutcome("The jar wobbles but doesn’t fall. You jump back quickly.", true, Program.FailureDamage / 2);
                    }
                ),
                ["marble"] = new ItemOutcome(
                    Description: "Roll a marble to stabilize the jar.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Agility >= 15)
                            return new ItemOutcome("You roll the marble carefully to steady the jar. You pass unharmed.");
                        return new ItemOutcome("The marble slips and jars rattle; you dodge just in time.", true, Program.FailureDamage / 2);
                    }
                )
                   }
               )
           );

        private static Scene BuildToySoldiersScene() => new(
            "TOY SOLDIER GAUNTLET",
            """
        You enter the playroom battlefield. Toy soldiers march menacingly in formation.
        LEFT: Rows of soldiers wielding plastic swords.
        RIGHT: Catapulted blocks and spinning tops litter the floor.
        """,
            "LEFT (Toy Soldiers)",
            new ScenePath(
                "The soldiers tighten their ranks, ready to swipe at you with tiny plastic swords.",
                "Choose an item to face them:",
                new Dictionary<string, ItemOutcome>
                {
                ["slingshot"] = new ItemOutcome(
                    Description: "Fire a pebble to scatter the soldiers.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Agility >= 15)
                            return new ItemOutcome("You fire accurately, scattering the soldiers without harm.");
                        return new ItemOutcome("The shot goes awry! A sword taps your paw as you dodge.", true, Program.FailureDamage / 2);
                    }
                ),
                ["shield"] = new ItemOutcome(
                    Description: "Block attacks with your shield.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Strength >= 15)
                            return new ItemOutcome("You hold the shield firm; the soldiers clatter harmlessly against it.");
                        return new ItemOutcome("A soldier slips past your shield and grazes you!", true, Program.FailureDamage / 2);
                    }
                ),
                ["marble"] = new ItemOutcome(
                    Description: "Roll a marble to disrupt the soldiers.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Intelligence >= 15)
                            return new ItemOutcome("You roll the marble strategically; it topples a row, giving you safe passage.");
                        return new ItemOutcome("The marble rolls unpredictably. You leap aside to avoid it.", true, Program.FailureDamage / 2);
                    }
                )
                }
            ),
            "RIGHT (Blocks & Tops)",
            new ScenePath(
                "The floor is a chaotic mess. One false step and you'll be launched into a spinning top or toppled block.",
                "Choose an item to navigate:",
                new Dictionary<string, ItemOutcome>
                {
                ["slingshot"] = new ItemOutcome(
                    Description: "Move a block with your slingshot to clear the path.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Intelligence >= 15)
                            return new ItemOutcome("You knock a block into a safer spot and dash across without harm.");
                        return new ItemOutcome("The block shifts under your paw! You tumble slightly.", true, Program.FailureDamage / 2);
                    }
                ),
                ["shield"] = new ItemOutcome(
                    Description: "Use the shield to deflect hazards.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Agility >= 15)
                            return new ItemOutcome("You roll and dodge behind the shield, avoiding spinning tops and blocks.");
                        return new ItemOutcome("A spinning top hits your shield but nudges you sideways!", true, Program.FailureDamage / 2);
                    }
                ),
                ["marble"] = new ItemOutcome(
                    Description: "Roll a marble to distract moving hazards.",
                    DynamicOutcome: (character, item) =>
                    {
                        if (character.Stats.Charisma >= 15)
                            return new ItemOutcome("You cleverly roll the marble to distract the toys and slip past safely.");
                        return new ItemOutcome("The marble bounces wildly. You scramble to avoid the flying blocks.", true, Program.FailureDamage / 2);
                    }
                )
                }
            )
        );
    }
}
