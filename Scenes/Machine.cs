using Godot;
using System;

public partial class Machine : Node2D
{
	[Export] public double ProductionTimeSec { get; set; } = 3;
	[Export] public string OutputId { get; set; } = "concrete";

	private Timer? _timer;
	public override void _Ready()
	{
		_timer = new Timer { OneShot = true, WaitTime = ProductionTimeSec };
		AddChild(_timer);
	}

	public void ProduceOnce() => _timer?.Start();
}
