using Godot;
using System;

public partial class Projectile : Area2D
{
    [Export] public float Speed = 200f;
    [Export] public int Damage = 25;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _Process(double delta)
    {
        GlobalPosition += Vector2.Right * Speed * (float)delta;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is BaseEnemy enemy)
        {
            enemy.TakeDamage(Damage);
            QueueFree();

            GD.Print("Hit something: ", body.Name); // Debug log to confirm collision [TEMPORARY]
        }
    }
}