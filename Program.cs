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

    public static class Program
    {
        private const int TypeDelayMilliseconds = 25;
        private const int InventorySelectionCount = 2;
        public const int FailureDamage = 25;

        public static readonly InventoryItem[] InventoryOptions = 
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

            var welcomeText = @"==========================================
🧀  WELCOME TO QUEST FOR CAMEMBERT!!  🧀
==========================================

You're a brave little mouse in a BIG house looking for Camembert!

Race across the board,
avoid the traps,
and outsmart anyone trying to catch you.

Press ENTER to scurry inside...
";

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
            return new CharacterProfile(name, inventory, stats);
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

            if (character.Stats.Health > 0)
            {
                CelebrateCamembert(character);
            }
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

        private static void CelebrateCamembert(CharacterProfile character)
        {
            Console.WriteLine();
            Console.WriteLine("🏁 You reach the heart of the pantry...");
            Console.WriteLine("A golden glow spills from a tiny cheese dome.");
            Console.WriteLine();

            var asciiCheese = @"______________________________________________¶¶¶¶¶¶¶¶¶¶¶
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
_¶¶¶¶_4______________¶¶¶¶¶¶¶¶¶¶¶¶¶444______________¶4444¶_________¶¶¶¶
__¶¶¶¶_44_____________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_____________444444_______¶¶¶¶¶
__¶¶¶¶__4_______________¶¶¶¶¶¶¶¶¶¶_¶¶¶¶¶¶¶¶¶¶¶¶¶¶__________444________¶¶¶¶¶
__¶¶¶¶__44_______________________________¶¶¶¶¶¶¶¶¶¶¶¶¶_________________¶¶¶¶¶
__¶¶¶¶__444___________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶____________¶¶¶¶¶¶
__¶¶¶¶__444________________________________________¶¶¶¶¶¶¶¶¶¶¶¶¶________¶¶¶¶¶
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
                              ";

            Console.WriteLine(asciiCheese);
            Console.WriteLine();
            Console.WriteLine($"Congratulations, {character.Name}! The Camembert is yours!");
        }
    }
}