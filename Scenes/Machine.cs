using BuildIt.Scenes;
using BuildIt.Scripts;
using Godot;

public partial class Machine : Node
{
	// Путь к инвентарю стройки (задаётся на ИНСТАНСЕ машины в ConstructionSite.tscn)
	[Export] public NodePath InventoryPath { get; set; }

	// Путь к кнопке (можно не заполнять, тогда возьмём ребёнка "ProduceButton")
	[Export] public NodePath ProduceButtonPath { get; set; }

	// Что производим по нажатию (для теста)
	[Export] public string OutputResourceId { get; set; } = "wood";
	[Export] public int OutputAmount { get; set; } = 1;

	private IInventory _siteInv;
	private Button _button;

	public override void _Ready()
	{
		// 1) Инвентарь
		if (InventoryPath == null || InventoryPath.IsEmpty)
		{
			GD.PushError("Machine: InventoryPath не задан в инспекторе (на инстансе в ConstructionSite).");
			return;
		}

		var invNode = GetNodeOrNull(InventoryPath);
		if (invNode is IInventory inv)
			_siteInv = inv;
		else
		{
			GD.PushError($"Machine: по пути '{InventoryPath}' узел не реализует IInventory.");
			return;
		}

		// 2) Кнопка
		_button = (ProduceButtonPath != null && !ProduceButtonPath.IsEmpty)
					? GetNodeOrNull<Button>(ProduceButtonPath)
					: GetNodeOrNull<Button>("ProduceButton");

		if (_button == null)
		{
			GD.PushError("Machine: не найдена кнопка ProduceButton. " +
						 "Либо задайте ProduceButtonPath, либо назовите ребёнка 'ProduceButton'.");
			return;
		}

		_button.Pressed += OnProduceButtonPressed;
	}

	public override void _ExitTree()
	{
		if (_button != null) _button.Pressed -= OnProduceButtonPressed;
	}

	private void OnProduceButtonPressed()
	{
		GD.Print($"[Machine] add to inv #{((_siteInv as Node)?.GetInstanceId() ?? 0)}");

		if (_siteInv == null) { GD.PushError("Machine: inventory is null"); return; }
		_siteInv.Add(OutputResourceId, OutputAmount); // простая проверка: +1 ресурс
	}
}
