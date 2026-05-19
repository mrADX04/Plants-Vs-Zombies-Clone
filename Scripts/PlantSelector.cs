using Godot;
using System;

public partial class PlantSelector : Node
{
    public static PlantSelector Instance;

    public PackedScene SelectedPlantScene;

    public override void _Ready()
    {
        Instance = this;
    }

    public void SelectPlant(PackedScene plantScene)
    {
        SelectedPlantScene = plantScene;
        GD.Print("Plant selected!");
    }

    public void ClearSelection()
    {
        SelectedPlantScene = null;
    }
}