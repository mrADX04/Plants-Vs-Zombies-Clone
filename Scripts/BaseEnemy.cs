using Godot;
using System;

public partial class BaseEnemy : CharacterBody2D
{
    [Export] public float Speed = 50f;
    [Export] public int MaxHealth = 100;

    private int currentHealth;

    public int Lane = 0; //assign in editor or via code by EnemySpawner

    public override void _Ready()
    {
        currentHealth = MaxHealth;

        LaneManager.Instance.RegisterEnemy(Lane, this);

        AddToGroup("Enemies");
    }

    public override void _ExitTree()
    {
        LaneManager.Instance.UnregisterEnemy(Lane, this);
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = new Vector2(-Speed, 0);
        MoveAndSlide();
    }

    // 🔥 Damage system
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        GD.Print("Enemy died");
        QueueFree();
    }
}