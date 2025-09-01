using System;
using System.Collections.Generic;

namespace BuildIt.Scripts;

public interface IInventory
{
    event Action Changed;
    int Get(string id);
    bool CanTake(string id, int amount);
    bool Take(string id, int amount); // true если списали
    void Add(string id, int amount);
    IReadOnlyDictionary<string, int> GetAll();
}

public interface IHasInventory
{
    IInventory Inventory { get; }
}
