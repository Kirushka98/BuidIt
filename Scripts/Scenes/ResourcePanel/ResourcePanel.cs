using BuidIt;
using BuildIt.Scenes;
using BuildIt.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using BuildIt.Scripts.Scenes.Main;

public partial class ResourcePanel : Control
{
	// Контейнер, куда кладём строки (VBoxContainer)
	[Export] public NodePath ItemsRootPath { get; set; }

	// Необязательно: префаб строки. Внутри два Label с уникальными именами %Name и %Amount
	[Export] public PackedScene RowScene { get; set; }

	// Показывать ли ресурсы с нулём
	[Export] public bool ShowZeroAmounts { get; set; } = false;

	private VBoxContainer _root;
	private IInventory _inv;

	private struct RowRefs
	{
		public Control Node;
		public Label Name;
		public Label Amount;
	}
	private readonly Dictionary<string, RowRefs> _rows = new();

	public override void _Ready()
	{
		// Ищем корневой контейнер
		_root = (ItemsRootPath != null && !ItemsRootPath.IsEmpty)
					? GetNodeOrNull<VBoxContainer>(ItemsRootPath)
					: GetNodeOrNull<VBoxContainer>("Items");

		if (_root == null)
		{
			GD.PushError("ResourcePanel: не найден VBoxContainer с ресурсами. " +
						 "Укажи ItemsRootPath или добавь ребёнка 'Items'.");
			return;
		}

		// Привязываемся к текущей стройке
		var site = SitesRegistry.I?.Current;
		if (site != null) Bind(site.Inventory);

		SitesRegistry.I.CurrentChanged += s => Bind(s.Inventory);
	}

	public override void _ExitTree()
	{
		if (_inv != null) _inv.Changed -= OnInventoryChanged;
	}

	public void Bind(IInventory inv)
	{
		if (_inv != null) _inv.Changed -= OnInventoryChanged;
		_inv = inv;
		if (_inv != null) _inv.Changed += OnInventoryChanged;

		GD.Print($"[Panel] bind to inv #{((_inv as Node)?.GetInstanceId() ?? 0)} ({_inv.GetType().Name})");

		RebuildAll();
	}

	private void OnInventoryChanged() => UpdateFromSnapshot();

	private void RebuildAll()
	{
		foreach (Node c in _root.GetChildren()) c.QueueFree();
		_rows.Clear();
		UpdateFromSnapshot();
	}

	private void UpdateFromSnapshot()
	{
		if (_inv == null) return;

		var snapshot = _inv.GetAll();                 // id -> qty
		var seen = new HashSet<string>();

		foreach (var kv in snapshot)
		{
			var id = kv.Key;
			var amount = kv.Value;

			if (!ShowZeroAmounts && amount == 0)
			{
				if (_rows.TryGetValue(id, out var old))
				{
					old.Node.QueueFree();
					_rows.Remove(id);
				}
				continue;
			}

			if (!_rows.TryGetValue(id, out var row))
			{
				row = CreateRow(id);
				_rows[id] = row;
			}

			row.Amount.Text = amount.ToString();
			seen.Add(id);
		}

		// Удаляем строки по ресурсам, которых больше нет
		var toRemove = _rows.Keys.Where(id => !seen.Contains(id)).ToList();
		foreach (var id in toRemove)
		{
			_rows[id].Node.QueueFree();
			_rows.Remove(id);
		}
	}

	private RowRefs CreateRow(string id)
	{
		Control node;
		Label name;
		Label amount;

		if (RowScene != null)
		{
			node   = RowScene.Instantiate<Control>();
			name   = node.GetNodeOrNull<Label>("%Name")   ?? node.GetNode<Label>("Name");
			amount = node.GetNodeOrNull<Label>("%Amount") ?? node.GetNode<Label>("Amount");
		}
		else
		{
			// Простейшая строка кодом
			var h = new HBoxContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
			name   = new Label { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
			amount = new Label { HorizontalAlignment = HorizontalAlignment.Right };
			h.AddChild(name);
			h.AddChild(amount);
			node = h;
		}

		name.Text = id;
		_root.AddChild(node);

		return new RowRefs { Node = node, Name = name, Amount = amount };
	}
}
