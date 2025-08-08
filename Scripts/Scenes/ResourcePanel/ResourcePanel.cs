using Godot;
using System;
using System.Text;

public partial class ResourcePanel : Control
{
	[Export] public NodePath InventoryPath { get; set; }
	[Export] public Label ResourcesLabel { get; set; } = default!;

	private InventoryNode _inv = default!;

	public override void _Ready()
	{
		_inv = GetNode<InventoryNode>(InventoryPath);
		_inv.Changed += UpdateView;
		UpdateView();
	}

	private void UpdateView()
	{
		var sb = new StringBuilder("Resources:\n");
		foreach (var kv in _inv.Snapshot())
			sb.AppendLine($"{kv.Key}: {kv.Value}");
		ResourcesLabel.Text = sb.ToString();
	}
}
