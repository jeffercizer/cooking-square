public enum CustomerType
{
    Polite,
    Casual,
    Scottish,
}
public class CustomerProfile
{
    public CustomerType Type;
    public string[] Greetings;
    public string[] Fillers;
    public string[] Openers;
    public string[] Connectors;
    public string[] Templates;

    public static readonly CustomerProfile PoliteProfile = new CustomerProfile {
        Type = CustomerType.Polite,
        Greetings = new[] { "Good evening", "Hello" },
        Fillers = new[] { "umm", "uhhh" },
        Openers = new[] { "May I please have", "I would like" },
        Connectors = new[] { "and", "as well as" },
        Templates = new[] {
            "{Greeting}, {Opener} {Filler} {ItemList}.",
            "{Greeting}! {Opener} {ItemList}, {Filler}.",
            "{Greeting}, {Filler} {Opener} {ItemList}.",
            "{Greeting}, {Opener} {ItemList}.",
            "{Greeting}! I'm craving {ItemList} today.",
            "{Greeting}, could I get {ItemList} please?"
        }
    };

    public static readonly CustomerProfile CasualProfile = new CustomerProfile {
        Type = CustomerType.Casual,
        Greetings = new[] { "", "Yo! ", "Hiya, ", "Hi, ", "Wassup ", "Howdy, " },
        Fillers = new[] { "", "hmm, ", "lemme think, " },
        Openers = new[] {"", "gimme, ", "I want " },
        Connectors = new[] { "and", "plus", "also" },
        Templates = new[] {
            "{Greeting}{Opener}{Filler}{ItemList}.",
            "{Greeting}{Opener}{ItemList}.",
            "{Greeting}{Filler}{Opener}{ItemList}.",
            "{Greeting}{Opener}{ItemList}.",
            "{Greeting}I'm craving {ItemList} today.",
            "{Greeting}could I get {ItemList} please?"
        }
    };

    public static readonly CustomerProfile ScottishProfile = new CustomerProfile {
        Type = CustomerType.Scottish,

        Greetings = new[] {
            "", 
            "Awrite, ", 
            "Haw, ", 
            "Heya pal, ", 
            "Alright there, ", 
            "Mornin’, ", 
            "Evenin’, "
        },

        Fillers = new[] {
            "", 
            "ehm… ", 
            "lemme think… ", 
            "hold on… ", 
            "hmm… ", 
            "right… "
        },

        Openers = new[] {
            "", 
            "gonnae get ", 
            "could ye sort me ", 
            "I’ll take ", 
            "I’m after ", 
            "gimme ", 
            "I fancy "
        },

        Connectors = new[] {
            "and", 
            "an’", 
            "plus", 
            "as well as"
        },

        Templates = new[] {
            "{Greeting}I’m pure starvin’, {Opener}{ItemList}.",
            "{Greeting}right, {Opener}{ItemList}.",
            "{Greeting}could ye get me {ItemList}?",
            "{Greeting}I’ll hae {ItemList} the day.",
            "{Greeting}I’m fancyin’ {ItemList} the noo.",
            "{Greeting}aye, {Opener}{ItemList}.",
            "{Greeting}listen, {Opener}{ItemList}.",
            "{Greeting}I’ll just take {ItemList}, cheers.",
            "{Greeting}I’m in the mood for {ItemList}.",
            "{Greeting}och aye, {Opener}{ItemList}.",
            "{Greeting}right then, {Opener}{ItemList}.",
            "{Greeting}could ye sort us oot wi’ {ItemList}?",
            "{Greeting}I’ll grab {ItemList}, thanks.",
            "{Greeting}ye wouldnae believe the morning I’ve had… anyway, {Opener}{ItemList}.",
            "{Greeting}hold on… naw, that’s it — {ItemList}.",
            "{Greeting}I was thinkin’ aboot somethin’ else, but nah, {ItemList} will dae.",
            "{Greeting}I’ve been dreamin’ aboot {ItemList} since last night.",
            "{Greeting}right, before I change my mind again, {Opener}{ItemList}.",
            "{Greeting}ye ken what, {ItemList} sounds braw the day.",
            "{Greeting}I swear I ordered this yesterday too… {ItemList}, please.",
            "{Greeting}I’m no’ picky, but {ItemList} is hittin’ the spot.",
            "{Greeting}dinnae judge me, but I’m goin’ for {ItemList}.",
            "{Greeting}I’ve had a long one, pal — just gimme {ItemList}.",
            "{Greeting}I was gonnae be healthy, but ach well… {ItemList}.",
            "{Greeting}ye ever crave somethin’ so specific it’s weird? {ItemList} for me.",
            "{Greeting}I promised masel’ I’d cut back, but here we are… {ItemList}.",
            "{Greeting}I’m no’ sayin’ I’m hungover, but {ItemList} might save me.",
            "{Greeting}I’ll take {ItemList}. Don’t ask why, long story.",
            "{Greeting}ye know what, life’s too short — {ItemList}.",
            "{Greeting}I’ve been hummin’ and hawin’ all mornin’, but {ItemList} it is.",
            "{Greeting}I’m tryin’ to treat masel’ the day, so {ItemList}.",
            "{Greeting}I had a whole plan, but it’s gone — {ItemList} instead.",
            "{Greeting}I’m no’ in the mood for thinkin’, just {ItemList}.",
            "{Greeting}I’ll hae {ItemList}. If it’s rubbish, that’s on me.",
            "{Greeting}I’m tellin’ ye, {ItemList} is the only thing keepin’ me goin’ the noo.",
            "{Greeting}I was talkin’ tae ma pal aboot this earlier — {ItemList}.",
            "{Greeting}I’ll take {ItemList}. Don’t suppose ye’ve got Irn‑Bru as well?"
        }
    };
}