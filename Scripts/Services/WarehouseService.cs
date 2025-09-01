using System;
using System.Collections.Generic;
using BuildIt.Scripts;
using Godot;

namespace BuidIt;

public partial class WarehouseService : Node, IInventory
{
	public static WarehouseService I { get; private set; }
	public override void _EnterTree() => I = this;

	private readonly Dictionary<string,int> _items = new();
	public event Action Changed;
	public int Get(string id) => _items.TryGetValue(id, out var v) ? v : 0;
	public bool CanTake(string id, int amount) => Get(id) >= amount;

	public bool Take(string id, int amount)
	{
		if (!CanTake(id, amount)) return false;
		_items[id] -= amount;
		Changed?.Invoke();
		return true;
	}

	public void Add(string id, int amount)
	{
		_items[id] = Get(id) + amount;
		Changed?.Invoke();
	}
	public IReadOnlyDictionary<string,int> GetAll() => _items;
}
