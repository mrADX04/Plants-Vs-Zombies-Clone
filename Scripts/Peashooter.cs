using Godot;
using System;

public partial class Peashooter : BasePlant
{
    [Export] public PackedScene ProjectileScene;

    private Timer shootTimer;
    private Marker2D shootPoint;

    public override void _Ready()
    {
        base._Ready(); // Call base method to run _Ready() logic from BasePlant
        shootTimer = GetNode<Timer>("Timer");
        shootTimer.Timeout += OnShoot;
        shootPoint = GetNode<Marker2D>("ShootPoint");
        shootTimer.Start(); // ← ADD THIS if Autostart is OFF
    }

    private void OnShoot()
    {
        Shoot();
    }

    private void Shoot()
    {
        Node2D projectile = ProjectileScene.Instantiate<Node2D>();

        // spawn at plant position
        projectile.GlobalPosition = shootPoint.GlobalPosition;

        // add to main scene (important)
        GetTree().CurrentScene.AddChild(projectile);
    }
}