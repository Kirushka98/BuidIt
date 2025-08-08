using System;
using System.Collections.Generic;
namespace BuidIt;

public sealed class Inventory
{
	private readonly Dictionary<string,int> _items = new();

	public int Get(string id) => _items.TryGetValue(id, out var v) ? v : 0;
	public void Add(string id, int qty) => _items[id] = Get(id) + qty;
	public bool TryConsume(Dictionary<string,int> need)
	{
		foreach (var (k,v) in need) if (Get(k) < v) return false;
		foreach (var (k,v) in need) _items[k] -= v;
		return true;
	}
}
