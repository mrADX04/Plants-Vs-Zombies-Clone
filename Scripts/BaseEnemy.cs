using Godot;
using System;

public partial class BaseEnemy : CharacterBody2D
{
    [Export] public float Speed = 50f;
    [Export] public int MaxHealth = 100;

    [Export] public int Damage = 10;
    [Export] public float AttackInterval = 1.0f;

    private int currentHealth;

    private BasePlant targetPlant = null;

    private float attackTimer = 0f;

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
        if (targetPlant != null)
        {
            Attack(delta);
            Velocity = Vector2.Zero; // stop movement
        }
        else
        {
            Move(delta);
        }

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
        //GD.Print("Enemy died");
        QueueFree();
    }

    private void _on_attack_area_body_entered(Node2D body)
    {
        if (body is BasePlant plant)
        {
            targetPlant = plant;
            attackTimer = 0f; // reset
            //GD.Print("Plant detected!");
        }
    }

    private void _on_attack_area_body_exited(Node2D body)
    {
        if (body == targetPlant)
        {
            targetPlant = null;
            //GD.Print("Plant left range!");
        }
    }

    private void Attack(double delta)
    {
        attackTimer += (float)delta;

        if (attackTimer >= AttackInterval)
        {
            attackTimer = 0f;

            if (targetPlant != null)
            {
                targetPlant.TakeDamage(Damage);
                //GD.Print("Attacking plant!");
            }
        }
    }

    private void Move(double delta)
    {
        Velocity = Vector2.Left * Speed;

    }
}