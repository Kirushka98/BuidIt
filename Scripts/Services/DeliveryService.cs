using BuildIt.Scripts;
using Godot;

namespace BuidIt;

public record DeliveryJob(string ResourceId, int Amount, IInventory From, IInventory To, double EtaSec);

public partial class DeliveryService : Node
{
    public static DeliveryService I { get; private set; }
    public override void _EnterTree() => I = this;

    public DeliveryJob? Request(string id, int amount, IInventory from, IInventory to, double distanceKm)
    {
        if (!from.CanTake(id, amount)) return null;
        from.Take(id, amount);
        var eta = CalcEta(distanceKm, amount);
        // Можно создать движущийся “грузовик”; для MVP — просто таймер:
        var t = GetTree().CreateTimer(eta);
        t.Timeout += () => to.Add(id, amount);
        return new DeliveryJob(id, amount, from, to, eta);
    }

    private double CalcEta(double distanceKm, int amount) => 1.0 + distanceKm * 0.5; // заглушка
}
