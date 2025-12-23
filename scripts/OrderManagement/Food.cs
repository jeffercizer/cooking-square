using System.Collections.Generic;

public static class FoodInfo
{
    public static Dictionary<Food, List<Doneness>> AllowedDoneness = new Dictionary<Food, List<Doneness>> { { Food.Steak, new List<Doneness> { Doneness.Rare, Doneness.MediumRare, Doneness.Medium, Doneness.MediumWell, Doneness.WellDone } } }; 
}

public enum Food
{

    Steak,
    Ketchup,

    Pancake,
    Syrup,
    Bacon,
    Egg,
} //Make sure to fill in the string info above
public enum Doneness
{
    Raw,
    Rare,
    MediumRare,
    Medium,
    MediumWell,
    WellDone,
    Burnt,
} //Make sure to fill in the string info above