using BuildIt.Scripts;
using BuildIt.Scripts.Scenes.Main;
using Godot;

namespace BuildIt.Scenes;

public partial class ConstructionSite : Node2D, IHasInventory
{
    [Export] public NodePath InventoryPath;
    public IInventory Inventory { get; private set; }

    public override void _Ready()
    {
        var inv = GetNodeOrNull<SiteInventoryNode>(InventoryPath)
                  ?? GetNodeOrNull<SiteInventoryNode>("SiteInventory");
        Inventory = inv;
        SitesRegistry.I.Register(this);
    }
}