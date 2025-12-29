using System.Globalization;
using System.Linq;
using Godot;

[GlobalClass]
public partial class Ticket: PanelContainer
{
    [Export] public Label orderNumberLabel;
    [Export] public Label orderDetailsLabel;
    private Order order;
    public Ticket(){}
    public void SetupBeforeSceneTree(Order order, int orderNum)
    {
        GD.Print(order.foodItems.Count);
        orderNumberLabel.Text = "Order #"+orderNum;
        var grouped = order.foodItems
            .GroupBy(f => new { f.food, f.doneness })
            .Select(g => new {
                Food = g.Key.food,
                Doneness = g.Key.doneness,
                Quantity = g.Count()
            })
            .ToList();
        string ticketString = "";
        foreach(var test in grouped)
        {
            ticketString += test.Quantity + "x " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(OrderGenerator.DonenessToString(test.Doneness)) + " " + test.Food + "\n";
        }
        orderDetailsLabel.Text = ticketString.Trim();
    }
    public void SetOrder(Order order)
    {
        this.order = order;
    }
    public Order GetOrder()
    {
        return order;
    }
}