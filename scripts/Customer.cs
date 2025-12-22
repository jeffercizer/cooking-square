using Godot;

[GlobalClass]
public partial class Customer : Node3D
{
    // The visual model (MeshInstance3D, AnimatedCharacter, etc.)
    [Export] public PackedScene CustomerModelScene;
    [Export] public float Speed = 3f; 
    [Export] public MeshInstance3D textBubble;
    public float CurrentDistance = 0f; 
    public bool orderTaken;
    public bool givingOrder;

    private Node3D _modelInstance;

    public override void _Ready()
    {
        if (CustomerModelScene != null)
        {
            _modelInstance = CustomerModelScene.Instantiate<Node3D>();
            AddChild(_modelInstance);
        }
        else
        {
            GD.PushWarning("Customer has no CustomerModelScene assigned.");
        }
        textBubble.Visible = false;
    }

    public override void _Process(double delta)
    {
        if(givingOrder && !textBubble.Visible)
        {
            textBubble.Visible = true;
        }
        else if(!givingOrder && textBubble.Visible)
        {
            textBubble.Visible = false;
        }
    }

    public void SetWorldPosition(Vector3 pos)
    {
        GlobalPosition = pos;
    }

    //helper function to only move the model
    public void CustomerLookAt(Vector3 target, Vector3 up)
    {
        _modelInstance.LookAt(target, up);
    }


    public void OrderTaken()
    {
        GD.Print("Customer's order taken");
        orderTaken = true;
        givingOrder = false;
    }
}
