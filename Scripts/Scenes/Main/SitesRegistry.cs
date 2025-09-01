using System;
using System.Collections.Generic;
using BuildIt.Scenes;
using Godot;

namespace BuildIt.Scripts.Scenes.Main;

public partial class SitesRegistry : Node
{
    public event Action<ConstructionSite> CurrentChanged;

    public static SitesRegistry I { get; private set; }
    public override void _EnterTree() => I = this;

    private readonly List<ConstructionSite> _sites = new();
    public ConstructionSite Current { get; private set; }

    public void Register(ConstructionSite site)
    {
        _sites.Add(site);
        if (Current == null) Current = site;
        CurrentChanged?.Invoke(site);
    }

    public void SetCurrent(ConstructionSite site)
    {
        Current = site;
        CurrentChanged?.Invoke(site);
    }

    public IReadOnlyList<ConstructionSite> All => _sites;
}
