using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Customer : Node3D
{
    [Export] public PackedScene CustomerModelScene;
    [Export] public float Speed = 3f; 
    [Export] public MeshInstance3D textBubble;
    [Export] public RichTextLabel orderLabel;
    public CustomerManager customerManager;
    public Order order;
    public CustomerProfile profile;
    public float CurrentDistance = 0f; 
    public bool orderTaken;
    public bool givingOrder;
    private string fullText;
    private int typedIndex = 0;


    private Node3D _modelInstance;

    public void SetupCustomerBeforeSceneTree(CustomerManager customerManager, LevelProfile level, CustomerProfile profile)
    {
        this.customerManager = customerManager;
        order = OrderGenerator.GenerateOrder(level);
        this.profile = profile;
    }

    public override void _Ready()
    {
        if (CustomerModelScene != null)
        {
            //TODO use customer profile to load model
            _modelInstance = CustomerModelScene.Instantiate<Node3D>();
            AddChild(_modelInstance);
        }
        else
        {
            GD.PushWarning("Customer has no CustomerModelScene assigned.");
        }
        textBubble.Visible = false;
        fullText = OrderGenerator.GenerateSpeech(order, profile);
        orderLabel.Text = fullText;
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

    public void HandleKeyPress(InputEventKey keyEvent)
    {
        // Convert key to a char
        string keyString = OS.GetKeycodeString(keyEvent.Keycode);
        if (string.IsNullOrEmpty(keyString) || keyEvent.Unicode <= 0)
            return;

        char typedChar = (char)keyEvent.Unicode;
        char expectedChar = fullText[typedIndex];

        if (typedChar == expectedChar)
        {
            typedIndex++;
            UpdateTextBubble();

            if (typedIndex >= fullText.Length)
            {
                OnOrderTaken();
            }
        }
        else
        {
            OnWrongKey(typedChar, expectedChar);
        }
    }

    private void OnOrderTaken()
    {
        GD.Print("Customer's order taken");
        customerManager.FrontCustomerOrderTaken();
        orderTaken = true;
        givingOrder = false;
        CurrentDistance = 0;
    }
    private void OnWrongKey(char typed, char expected)
    {
        GD.Print($"Typed: |{typed}| Expected: |{expected}|");
        //visual shake or something
    }

    private void UpdateTextBubble()
    {
        string typed = fullText.Substring(0, typedIndex);
        string remaining = fullText.Substring(typedIndex);

        orderLabel.Text = $"[color=green]{typed}[/color]{remaining}";
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
}
