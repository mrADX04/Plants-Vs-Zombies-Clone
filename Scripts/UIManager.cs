using Godot;
using System;

public partial class UIManager : CanvasLayer
{
    [Export] public PackedScene PeashooterScene;
    [Export] public PackedScene SunflowerScene;
    [Export] public PackedScene WallnutScene;

    private Label sunLabel;

    public override void _Ready()
    {

        sunLabel = GetNode<Label>("TopBar/SunCounter/SunLabel");

        // Initialize the sun counter with the current sun amount & This prevents UIManager from crashing if SunManager.Instance is null when UIManager is ready.
        if (SunManager.Instance != null)
        {
            sunLabel.Text =
                SunManager.Instance.CurrentSun.ToString();

            SunManager.Instance.SunChanged += UpdateSunCounter;
        }


        var peashooterButton =
            GetNode<TextureButton>(
                "TopBar/CardContainer/PeashooterCard"
            );

        peashooterButton.Pressed += OnPeashooterButtonPressed;

        var sunflowerButton =
            GetNode<TextureButton>(
                "TopBar/CardContainer/SunflowerCard"
            );

        sunflowerButton.Pressed += OnSunflowerButtonPressed;

        var wallnutButton =
            GetNode<TextureButton>(
                "TopBar/CardContainer/WallnutCard"
            );

        wallnutButton.Pressed += OnWallnutButtonPressed;
    }

    private void OnPeashooterButtonPressed()
    {
        PlantSelector.Instance.SelectPlant(PeashooterScene);
    }

    private void OnSunflowerButtonPressed()
    {
        PlantSelector.Instance.SelectPlant(SunflowerScene);
    }

    private void OnWallnutButtonPressed()
    {
        PlantSelector.Instance.SelectPlant(WallnutScene);
    }
    private void UpdateSunCounter(int amount)
    {
        sunLabel.Text = amount.ToString();
    }
}