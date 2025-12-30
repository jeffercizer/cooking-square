using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[GlobalClass]
public partial class CustomerManager : Node
{
	[Export] public Path3D orderPath;
	[Export] public Path3D pickupPath;
	[Export] public Marker3D spawnMarker;
	[Export] public float SlotSpacing = 2.0f;
	[Export] public Camera3D camera1;
	[Export] public GameManager GameManager;
	[Export] public OrderingManager OrderingManager;
	public LevelProfile levelProfile;
	public int remainingCustomersToSpawn;
	private float CurrentCustomerSpawnTimer;
	private float pathLength;
	private bool levelStarted = false;

	public List<Customer> WaitingCustomers = new List<Customer>();
	public List<Customer> OrderedCustomers = new List<Customer>();
	public override void _Ready()
	{
		CurrentCustomerSpawnTimer = 9999999999999999999f;
	}

	public override void _Process(double delta)
	{
		if (levelStarted)
		{
			UpdateQueue(delta, WaitingCustomers);
			UpdateQueue(delta, OrderedCustomers);
			if(remainingCustomersToSpawn > 0)
			{
				CurrentCustomerSpawnTimer -= (float)delta;
				if(CurrentCustomerSpawnTimer <= 0)
				{
					remainingCustomersToSpawn--;
					SpawnCustomer();
					CurrentCustomerSpawnTimer = levelProfile.CustomerSpawnTimer;
				}
			}
			if(remainingCustomersToSpawn <= 0 && !OrderedCustomers.Any() && !WaitingCustomers.Any())
			{
				WonLevel();
			}
		}
	}

	public void StartLevel(LevelProfile levelProfile)
	{
		this.levelProfile = levelProfile;
		remainingCustomersToSpawn = levelProfile.NumCustomersToSpawn;
		CurrentCustomerSpawnTimer = 0f;
		levelStarted = true;
	}

	private void UpdateQueue(double delta, List<Customer> customers)
	{
		Path3D path;
		if (customers.Count == 0)
			return;

		float dt = (float)delta;
		for (int i = 0; i < customers.Count; i++)
		{
			Customer c = customers[i];
			path = c.orderTaken ? pickupPath : orderPath;
			pathLength = path.Curve.GetBakedLength();
			float desiredDist = pathLength - (i * SlotSpacing);

			c.CurrentDistance = Mathf.MoveToward(
				c.CurrentDistance,
				desiredDist,
				c.Speed * dt
			);

			if(c.CurrentDistance == desiredDist) //we are standing still
			{
				if(i == 0 && !c.orderTaken) //we are the front ordering customer and are in position
				{
					c.givingOrder = true;
					OrderingManager.SetActiveCustomer(c);
				}
				else //all other customers
				{
					c.givingOrder = false;
				}
				if(c.orderTaken)
				{
					//GameManager.ticketManager.RemoveOrderTicket(c.order);//remove this too
					OrderedCustomers.Remove(c); //TODO REMOVE, this is so levels will end 
					//normally after an order is taken we would keep them in this list until
					//player 4 gives them their order.
				}
			}
			else
			{
				// Sample the curve at the customer's current distance
				Vector3 localPoint = path.Curve.SampleBaked(c.CurrentDistance);
				Vector3 worldPoint = path.ToGlobal(localPoint);

				// Tell the customer where to be
				c.SetWorldPosition(worldPoint);
				
				// Apply rotation to customer
				if(c.givingOrder)
				{
					c.CustomerLookAt(camera1.GlobalPosition, Vector3.Up);
				}
				else
				{         			
					Vector3 localNow = path.Curve.SampleBaked(c.CurrentDistance);
					Vector3 worldNow = path.ToGlobal(localNow);

					// Sample a point slightly ahead on the curve
					float lookAhead = 0.1f; // tweak this for smoother turning
					float aheadDist = c.CurrentDistance + lookAhead;

					Vector3 localAhead = path.Curve.SampleBaked(aheadDist);
					Vector3 worldAhead = path.ToGlobal(localAhead);

					// Compute forward direction
					Vector3 forward = (worldAhead - worldNow).Normalized();
					c.CustomerLookAt(worldNow + forward, Vector3.Up);
				}
			}
		}
	}

	public void SpawnCustomer()
	{
		PackedScene customerScene = GD.Load<PackedScene>("res://scenes/customer.tscn");
		Customer customer = customerScene.Instantiate<Customer>();
		customer.GlobalTransform = spawnMarker.GlobalTransform;

		Random rng = new();
		CustomerProfile customerProfile = levelProfile.AllowedCustomerProfiles.OrderBy(_ => rng.Next()).First();
		customer.SetupCustomerBeforeSceneTree(this, levelProfile, customerProfile);
		AddChild(customer);
		AddCustomer(customer);
	}

	private void AddCustomer(Customer c)
	{
		WaitingCustomers.Add(c);
	}

	public void FrontCustomerOrderTaken()
	{
		if (WaitingCustomers.Count == 0)
			return;

		Customer front = WaitingCustomers[0];
		GameManager.SetScore(GameManager.GetScore() + 100);
		OrderedCustomers.Add(front);
		WaitingCustomers.Remove(front);
		GameManager.AddOrderTicket(front.order);
	}

	public void RemoveCustomer(Customer c)
	{
		WaitingCustomers.Remove(c);
		OrderedCustomers.Remove(c);
	}

	public void Reset()
	{
		foreach(Customer c in WaitingCustomers)
		{
			c.QueueFree();
		}
		foreach(Customer c in OrderedCustomers)
		{
			c.QueueFree();
		}
		WaitingCustomers = new();
		OrderedCustomers = new();
		levelStarted = false;
	}

	public void WonLevel()
	{
		GameManager.WonLevel();
	}
	public void LostLevel()
	{
		GameManager.LostLevel();
	}
}
