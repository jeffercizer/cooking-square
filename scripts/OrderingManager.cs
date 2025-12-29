
using Godot;
[GlobalClass]
public partial class OrderingManager : Node
{
    [Export] public CustomerManager customerManager;
    private Customer activeCustomer;

    public void SetActiveCustomer(Customer customer)
    {
        activeCustomer = customer;
    }
    public Customer GetActiveCustomer()
    {
        return activeCustomer;
    }
    public void ClearActiveCustomer()
    {
        activeCustomer = null;
    }
    public override void _Input(InputEvent @event)
    {
        if (activeCustomer == null || !activeCustomer.givingOrder || !activeCustomer.textBubble.Visible)
            return;

        if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
        {
            activeCustomer.HandleKeyPress(keyEvent);
        }
    }

    
}