using System.Threading.Tasks;
using BuidIt;
using BuildIt.Scenes;
using BuildIt.Scripts.Scenes.Main;
using Godot;

public partial class Main : Node2D
{
	#region consts
	[Export] public bool StartBorderless = true; // если false — старт сразу в true fullscreen

	const string toggleScreenAction = "toggle_fullscreen";
	#endregion
	
	[Export] public PackedScene ConstructionSiteScene { get; set; }
	[Export] public PackedScene ResourcePanelScene { get; set; }
	
	private Window W => GetWindow();

	public override async void _Ready()
	{
		await ApplyInitialWindowMode();
		
		SeedDate();
		var panel = ResourcePanelScene.Instantiate<ResourcePanel>();
		AddChild(panel);
	}

	private void SeedDate()
	{
		var siteA = ConstructionSiteScene.Instantiate<ConstructionSite>();
		AddChild(siteA);

		var siteB = ConstructionSiteScene.Instantiate<ConstructionSite>();
		AddChild(siteB);
		SitesRegistry.I.SetCurrent(siteB);

		// пример стартовых остатков на складе
		WarehouseService.I.Add("wood", 1000);
	}

	#region Screen
	
	private async Task ApplyInitialWindowMode()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		if (StartBorderless) SetBorderless();
		else SetTrueFullscreen();
	}

	public override void _UnhandledInput(InputEvent e)
	{
		if (Input.IsActionJustPressed("toggle_fullscreen"))
		{
			ToggleFullscreen();
			GetViewport().SetInputAsHandled();
		}
	}

	private void SetBorderless()
	{
		W.Mode = Window.ModeEnum.Windowed;   // важен порядок: сначала окно
		W.Borderless = true;
		// растягиваем на текущий экран
		var scr = DisplayServer.WindowGetCurrentScreen();
		W.Size = DisplayServer.ScreenGetSize(scr);
		W.Position = Vector2I.Zero;
		// опц.: W.AlwaysOnTop = true; // если нужно поверх
	}

	private void SetTrueFullscreen()
	{
		W.Borderless = false; // на всякий случай
		W.Mode = Window.ModeEnum.Fullscreen; // или ExclusiveFullscreen
	}

	private void ToggleFullscreen()
	{
		var isFs = W.Mode == Window.ModeEnum.Fullscreen || W.Mode == Window.ModeEnum.ExclusiveFullscreen;
		if (isFs) SetBorderless();
		else SetTrueFullscreen();
	}
	#endregion

}
