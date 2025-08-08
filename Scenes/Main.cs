using Godot;

public partial class Main : Node2D
{
	[Export] public PackedScene? ConstructionSiteScene { get; set; }
	[Export] public PackedScene? ResourcePanelScene { get; set; }

	public override void _Ready()
	{
		var site = ConstructionSiteScene!.Instantiate<Node2D>();
		AddChild(site);

		var ui = ResourcePanelScene!.Instantiate<Control>();
		AddChild(ui);
	}
}
