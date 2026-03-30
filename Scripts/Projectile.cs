using Godot;
using System;

public partial class Projectile : Node2D
{
    [Export] public float Speed = 200f;

    public override void _Process(double delta)
    {
        // move RIGHT
        GlobalPosition += Vector2.Right * Speed * (float)delta;
    }
}