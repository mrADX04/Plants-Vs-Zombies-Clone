using Godot;
using System;

public partial class PlantCard : TextureButton
{
    [Export] public PlantData PlantData;

    public override void _Ready()
    {
        Pressed += OnPressed;

        UpdateCardState();

        SunManager.Instance.SunChanged += OnSunChanged;
    }

    private void OnPressed()
    {
        if (PlantData == null)
            return;

        // Prevent selecting unaffordable card
        if (!SunManager.Instance.CanAfford(PlantData.SunCost))
            return;

        PlantSelector.Instance.SelectPlant(PlantData);
    }

    private void OnSunChanged(int newAmount)
    {
        UpdateCardState();
    }

    private void UpdateCardState()
    {
        if (PlantData == null)
            return;

        bool affordable =
            SunManager.Instance.CanAfford(PlantData.SunCost);

        Disabled = !affordable;

        // Grey out visually
        if (affordable)
        {
            Modulate = new Color(1, 1, 1, 1);
        }
        else
        {
            Modulate = new Color(0.4f, 0.4f, 0.4f, 1);
        }
    }
}