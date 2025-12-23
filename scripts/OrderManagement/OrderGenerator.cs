
using System;
using System.Collections.Generic;
using System.Linq;

public static class OrderGenerator
{
    static Random rng = new Random();
    static T Pick<T>(IList<T> arr) => arr[rng.Next(arr.Count)];

    public static Order GenerateOrder(LevelProfile level)
    {
        var order = new Order();

        // Decide how many items total
        int totalItems = rng.Next(level.TotalItemsRange.Min, level.TotalItemsRange.Max + 1);

        // Shuffle available foods
        var foods = level.AvailableFoods.OrderBy(_ => rng.Next()).ToList();

        foreach (var food in foods)
        {
            if (order.foodItems.Count >= totalItems)
                break;

            //for chance based items
            if (level.InclusionChances.TryGetValue(food, out double chance))
            {
                if (rng.NextDouble() > chance)
                    continue;
            }

            //quantity
            var (min, max) = level.QuantityRanges.ContainsKey(food)
                ? level.QuantityRanges[food]
                : (1, 1);

            int count = rng.Next(min, max + 1);

            //check against totalItems
            count = Math.Min(count, totalItems - order.foodItems.Count);

            for (int i = 0; i < count; i++)
            {
                //find if there are donenessOptions and pick one otherwise null
                var donenessOptions = FoodInfo.AllowedDoneness.ContainsKey(food)
                    ? FoodInfo.AllowedDoneness[food]
                    : null;
                order.foodItems.Add(new FoodItem {
                    food = food,
                    doneness = donenessOptions != null && donenessOptions.Count > 0
                        ? Pick(donenessOptions)
                        : null,
                });
            }
        }

        return order;
    }

    public class ItemSpeechParts
    {
        public string ItemList;
        public string[] Items; // individual items
        public int Count;
    }

    static ItemSpeechParts BuildItemSpeechParts(List<string> itemStrings, string connector)
    {
        var parts = new ItemSpeechParts();
        parts.Items = itemStrings.ToArray();
        parts.Count = itemStrings.Count;

        if (parts.Count == 0)
        {
            parts.ItemList = "";
            return parts;
        }

        if (parts.Count == 1)
        {
            parts.ItemList = parts.Items[0];
            return parts;
        }

        if (parts.Count == 2)
        {
            parts.ItemList = $"{parts.Items[0]} {connector} {parts.Items[1]}";
            return parts;
        }

        // 3 or more items
        // "A, B, C and D"
        string allButLast = string.Join(", ", parts.Items.Take(parts.Count - 1));
        string last = parts.Items.Last();
        parts.ItemList = $"{allButLast} {connector} {last}";

        return parts;
    }


    public static string GenerateSpeech(Order order, CustomerProfile profile)
    {
        string greeting = Pick(profile.Greetings);
        string filler = Pick(profile.Fillers);
        string opener = Pick(profile.Openers);
        string connector = Pick(profile.Connectors);

        //group items of same type and doneness to get quantity
        var grouped = order.foodItems
            .GroupBy(f => new { f.food, f.doneness })
            .Select(g => new {
                Food = g.Key.food,
                Doneness = g.Key.doneness,
                Quantity = g.Count()
            })
            .ToList();

        List<string> itemStrings = new();
        foreach (var g in grouped)
            itemStrings.Add(ItemToSpeech(g.Food, g.Doneness, g.Quantity));

        int itemCount = itemStrings.Count;

        //build ItemList , this is a bit weird for good reason
        // I did it to support combining 2x steaks welldone, 
        // but then I need to split them apart for dynamic speech
        ItemSpeechParts parts = BuildItemSpeechParts(itemStrings, connector);

        //filter templates based on how many items they require
        var compatibleTemplates = profile.Templates
            .Where(t => GetRequiredItemCount(t) <= itemCount)
            .ToList();

        //fallback if none match
        string template = compatibleTemplates.Count > 0
            ? Pick(compatibleTemplates)
            : "{Greeting}, {Opener} {ItemList}.";

        //replace placeholders
        string result = template
            .Replace("{Greeting}", greeting)
            .Replace("{Opener}", opener)
            .Replace("{Filler}", filler)
            .Replace("{ItemList}", parts.ItemList)
            .Replace("{Item1}", parts.Count > 0 ? parts.Items[0] : "")
            .Replace("{Item2}", parts.Count > 1 ? parts.Items[1] : "")
            .Replace("{Item3}", parts.Count > 2 ? parts.Items[2] : "");

        return result;
    }


    static string ItemToSpeech(Food name, Doneness? doneness, int qty)
    {
        string baseText = $"{qty} {FoodToString(name)}";
        if (doneness.HasValue) baseText += $" {DonenessToString(doneness.Value)}";
        if (qty > 1) baseText += "s";
        return baseText;
    }

    static int GetRequiredItemCount(string template)
    {
        int required = 0;

        if (template.Contains("{Item1}")) required = Math.Max(required, 1);
        if (template.Contains("{Item2}")) required = Math.Max(required, 2);
        if (template.Contains("{Item3}")) required = Math.Max(required, 3);

        // templates that only use {ItemList} effectively require only 1 item
        if (required == 0 && template.Contains("{ItemList}"))
            required = 1;

        return required;
    }



    public class FoodSpeechConfig
    {
        public string Singular;
        public string Plural;
    }

    //START FOOD INFO FILL IN////
    static readonly Dictionary<Food, FoodSpeechConfig> FoodSpeech = new()
    {
        { Food.Steak, new FoodSpeechConfig { Singular = "steak", Plural = "steaks" } },
        { Food.Bacon, new FoodSpeechConfig { Singular = "piece of bacon", Plural = "pieces of bacon" } },
        { Food.Ketchup, new FoodSpeechConfig { Singular = "side of ketchup", Plural = "sides of ketchup" } },
        { Food.Pancake, new FoodSpeechConfig { Singular = "pancake", Plural = "pancakes" } },
        { Food.Egg, new FoodSpeechConfig { Singular = "egg", Plural = "eggs" } },
        { Food.Syrup, new FoodSpeechConfig { Singular = "side of syrup", Plural = "sides of syrup" } },
    };
    static string FoodToString(Food f) => f switch {
        Food.Steak => "steak",
        Food.Ketchup => "ketchup",
        Food.Pancake => "pancake",
        Food.Syrup => "syrup",
        Food.Bacon => "bacon",
        Food.Egg => "egg",
        _ => f.ToString()
    };
    static string DonenessToString(Doneness d) => d switch {
        Doneness.Raw => "raw",
        Doneness.Rare => "rare",
        Doneness.MediumRare => "medium rare",
        Doneness.Medium => "medium",
        Doneness.MediumWell => "medium well",
        Doneness.WellDone => "well done",
        Doneness.Burnt => "burnt",
        _ => d.ToString()
    };
    ////End Food Info///////////////////////////////////////////////////////////////////
}

