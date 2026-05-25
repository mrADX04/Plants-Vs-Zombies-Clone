using Godot;
using System;

public partial class BasePlant : StaticBody2D
{
    [Export] public int MaxHealth = 100;

    [Export] public int SunCost = 100;

    private int gridX;
    private int gridY;
    private GridManager gridManager;

    protected int currentHealth;

    public override void _Ready()
    {
        currentHealth = MaxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        GD.Print($"{Name} took {damage} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        GD.Print($"{Name} died");
        gridManager?.FreeCell(gridX, gridY);
        QueueFree(); // Remove the plant from the scene
    }

    public void SetGridPosition(int x, int y, GridManager manager) // This method is called by the GridManager when the plant is placed on the grid
    {
        gridX = x;
        gridY = y;
        gridManager = manager;
    }
}