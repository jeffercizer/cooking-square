using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class CustomerManager : Node
{
	[Export] public Path3D Path;
	[Export] public Marker3D spawnMarker;
	[Export] public float SlotSpacing = 2.0f;
	[Export] public float CustomerSpawnTimer = 10.0f;
	[Export] public int NumCustomersToSpawn = 5;
	[Export] public Camera3D camera1;

	private float CurrentCustomerSpawnTimer;
	private float pathLength;

	private Curve3D _curve;
	public List<Customer> Customers = new List<Customer>();
	private float _lastControlDistance;
	private float _secondLastControlDistance;
	public override void _Ready()
	{
		_curve = Path.Curve;
		CurrentCustomerSpawnTimer = 1;
		pathLength = Path.Curve.GetBakedLength();
	}



	public override void _Process(double delta)
	{
		UpdateQueue(delta);
		if(NumCustomersToSpawn > 0)
		{
			CurrentCustomerSpawnTimer -= (float)delta;
			if(CurrentCustomerSpawnTimer <= 0)
			{
				NumCustomersToSpawn--;
				SpawnCustomer();
				CurrentCustomerSpawnTimer = CustomerSpawnTimer;
			}
		}
	}

	private void UpdateQueue(double delta)
	{
		if (Customers.Count == 0)
			return;

		float dt = (float)delta;
		int pathOffset = 0;
		for (int i = 0; i < Customers.Count; i++)
		{
			Customer c = Customers[i];
			float desiredDist = 0;
			if(c.orderTaken)
			{
				desiredDist = pathLength - (pathOffset * SlotSpacing);
			}
			else
			{
				desiredDist = pathLength - 3.4f - (pathOffset * SlotSpacing);
				pathOffset++;
			}

			c.CurrentDistance = Mathf.MoveToward(
				c.CurrentDistance,
				desiredDist,
				c.Speed * dt
			);

			if(c.CurrentDistance == desiredDist) //we are standing still
			{
				if(pathOffset == 1) //we are the front customer and are in position
				{
					c.givingOrder = true;
				}
				else //all other customers
				{
					c.givingOrder = false;
				}
			}
			else
			{
				// Sample the curve at the customer's current distance
				Vector3 localPoint = Path.Curve.SampleBaked(c.CurrentDistance);
				Vector3 worldPoint = Path.ToGlobal(localPoint);

				// Tell the customer where to be
				c.SetWorldPosition(worldPoint);
				
				// Apply rotation to customer
				if(c.givingOrder)
				{
					c.CustomerLookAt(camera1.GlobalPosition, Vector3.Up);
				}
				else
				{         			
					Vector3 localNow = Path.Curve.SampleBaked(c.CurrentDistance);
					Vector3 worldNow = Path.ToGlobal(localNow);

					// Sample a point slightly ahead on the curve
					float lookAhead = 0.1f; // tweak this for smoother turning
					float aheadDist = c.CurrentDistance + lookAhead;

					Vector3 localAhead = Path.Curve.SampleBaked(aheadDist);
					Vector3 worldAhead = Path.ToGlobal(localAhead);

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
		AddChild(customer);
		AddCustomer(customer);
	}

	private void AddCustomer(Customer c)
	{
		Customers.Add(c);
	}

	public void FrontCustomerOrderTaken()
	{
		if (Customers.Count == 0)
			return;

		Customer front = Customers[0];
		front.OrderTaken();
	}

	public void RemoveCustomer(Customer c)
	{
		Customers.Remove(c);
	}
}
