
using System.Collections.Generic;

public class LevelProfile
{
    // Which foods can appear
    public List<Food> AvailableFoods = new();
    public Dictionary<Food, (int Min, int Max)> QuantityRanges = new();
    public Dictionary<Food, double> InclusionChances = new();
    public (int Min, int Max) TotalItemsRange = (1, 3);
    public List<CustomerProfile> AllowedCustomerProfiles = new();

    public static readonly LevelProfile firstlevel = new LevelProfile {
        AvailableFoods = new List<Food> { Food.Steak, Food.Ketchup },
        QuantityRanges = new Dictionary<Food, (int, int)> { { Food.Steak, (1, 2) }}, 
        InclusionChances = new Dictionary<Food, double> { { Food.Ketchup, 0.3 } },
        TotalItemsRange = (1, 2), // level 1 orders are 1 or 2 items

        AllowedCustomerProfiles = new List<CustomerProfile> { CustomerProfile.PoliteProfile, CustomerProfile.CasualProfile }
    };

    public static readonly LevelProfile scottishLevel = new LevelProfile {
        AvailableFoods = new List<Food> { Food.Steak, Food.Ketchup },
        QuantityRanges = new Dictionary<Food, (int, int)> { { Food.Steak, (1, 2) }}, 
        InclusionChances = new Dictionary<Food, double> { { Food.Ketchup, 0.3 } },
        TotalItemsRange = (1, 2),

        AllowedCustomerProfiles = new List<CustomerProfile> { CustomerProfile.ScottishProfile }
    };


    public static readonly LevelProfile breakfastLevel = new LevelProfile {
        AvailableFoods = new List<Food> { Food.Pancake, Food.Egg, Food.Bacon, Food.Syrup },
        QuantityRanges = new Dictionary<Food, (int, int)> {
            { Food.Pancake, (1, 2) },
            { Food.Egg, (1, 2) },
            { Food.Bacon, (2, 3) },
            { Food.Syrup, (1, 1) }
        },
        InclusionChances = new Dictionary<Food, double> {
            { Food.Syrup, 0.4 }
        },
        TotalItemsRange = (1, 4), // breakfast orders should be 1-4 items total

        AllowedCustomerProfiles = new List<CustomerProfile> { CustomerProfile.PoliteProfile, CustomerProfile.CasualProfile }
    };
}