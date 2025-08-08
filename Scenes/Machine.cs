using Godot;

public partial class Machine : Node2D
{
	[Export] public NodePath InventoryPath { get; set; }
	[Export] public string OutputId { get; set; } = "concrete";
	[Export] public double ProductionTimeSec { get; set; } = 3.0;

	private InventoryNode _inv = default!;
	private Timer _timer = default!;
	private Button _button = default!;

	public override void _Ready()
	{
		_inv = GetNode<InventoryNode>(InventoryPath);

		_timer = new Timer { OneShot = true, WaitTime = ProductionTimeSec };
		AddChild(_timer);
		_timer.Timeout += OnProduced;

		_button = GetNode<Button>("ProduceButton");
		_button.Pressed += OnPressed;
	}

	private void OnPressed()
	{
		_button.Disabled = true;
		_timer.Start();
	}

	private void OnProduced()
	{
		_inv.Add(OutputId, 1);
		_button.Disabled = false;
	}
}
