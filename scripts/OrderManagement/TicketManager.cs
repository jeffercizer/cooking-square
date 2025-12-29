using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class TicketManager : MarginContainer
{
    [Export] public HBoxContainer ticketContainer;
    public List<Ticket> orderTickets = new();
    public int latestOrderNum;
    public Dictionary<Order, Ticket> orderTicketDictionary = new();
    public void AddOrderTicket(Order order)
    {
        PackedScene ticketScene = GD.Load<PackedScene>("res://scenes/ticket.tscn");
		Ticket ticket= ticketScene.Instantiate<Ticket>();
        ticket.SetupBeforeSceneTree(order, ++latestOrderNum);
        ticketContainer.AddChild(ticket);
        orderTicketDictionary.Add(order, ticket);
        orderTickets.Add(ticket);
    }
    public void RemoveOrderTicket(Order order)
    {
        Ticket ticket = orderTicketDictionary[order];
        orderTickets.Remove(ticket);
        ticket.QueueFree();

    }
}