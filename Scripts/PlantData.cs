using Godot;

[GlobalClass]
public partial class PlantData : Resource
{
    [Export] public string PlantName { get; set; } = "";
    [Export] public Texture2D Icon { get; set; }
    [Export] public int SunCost { get; set; } = 100;
    [Export] public float Cooldown { get; set; } = 7.5f;
    [Export] public PackedScene PlantScene { get; set; }
}