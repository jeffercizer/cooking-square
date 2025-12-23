using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class FoodItem
{
    public Food food;
    public Doneness? doneness;
}

public partial class Order : Node
{
    public List<FoodItem> foodItems;
    public string orderText;
    public double timeLeft;
    private bool timeRanOut;
    public event Action OrderTimerRanOut;
    public bool orderTaken;
    public Order()
    {
        foodItems = new();
    }
    public Order(List<FoodItem> foodItems, double time=300)
    {
        this.foodItems = foodItems;
    }
    public override void _PhysicsProcess(double delta)
    {
        if(orderTaken)
        {
            timeLeft -= delta;
        }
        if(timeLeft <= 0 && !timeRanOut)
        {
            timeRanOut = true;
            OrderTimerRanOut?.Invoke();
        }
    }
}

