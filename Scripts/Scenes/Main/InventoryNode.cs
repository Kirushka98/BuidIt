using Godot;
using System.Collections.Generic;

public partial class InventoryNode : Node
{
	private readonly Dictionary<string,int> _items = new();
	[Signal] public delegate void ChangedEventHandler();

	public int Get(string id) => _items.TryGetValue(id, out var v) ? v : 0;

	public void Add(string id, int qty)
	{
		_items[id] = Get(id) + qty;
		EmitSignal(SignalName.Changed);
	}

	public bool TryConsume(IReadOnlyDictionary<string,int> need)
	{
		foreach (var (k,v) in need) if (Get(k) < v) return false;
		foreach (var (k,v) in need) _items[k] -= v;
		EmitSignal(SignalName.Changed);
		return true;
	}

	public IReadOnlyDictionary<string,int> Snapshot() => _items;
}
