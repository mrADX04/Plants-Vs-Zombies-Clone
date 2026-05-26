using Godot;
using System;

public partial class PlantSelector : Node
{
    public static PlantSelector Instance;

    public PlantData SelectedPlantData;

    public override void _Ready()
    {
        Instance = this;
    }

    public void SelectPlant(PlantData plantData)
    {
        SelectedPlantData = plantData;
        GD.Print("Plant selected!");
    }

    public void ClearSelection()
    {
        SelectedPlantData = null;
    }
}