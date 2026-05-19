using Godot;
using System;

public partial class Sunflower : BasePlant
{
    [Export] public PackedScene SunScene;

    [Export] public float SunSpawnInterval = 8.0f;

    private Timer sunTimer;

    public override void _Ready()
    {
        base._Ready();

        // Get Timer node
        sunTimer = GetNode<Timer>("Timer");

        // Configure timer
        sunTimer.WaitTime = SunSpawnInterval;
        sunTimer.Timeout += SpawnSun;
        sunTimer.Start();
    }

    private void SpawnSun()
    {
        if (SunScene == null)
        {
            GD.Print("SunScene not assigned!");
            return;
        }

        // Create sun instance
        Node2D sunInstance = SunScene.Instantiate<Node2D>();

        // Add to main scene
        GetTree().CurrentScene.AddChild(sunInstance);

        // Random offset so suns don't stack perfectly
        float randomX = (float)GD.RandRange(-6, 0); //horizontal variation
        float randomY = (float)GD.RandRange(-0, 0); //vertical variation

        // Spawn near sunflower with slight variation
        sunInstance.GlobalPosition = GlobalPosition +
            new Vector2(randomX, randomY);

        //GD.Print("Spawned sun at: " + sunInstance.GlobalPosition);
    }
}