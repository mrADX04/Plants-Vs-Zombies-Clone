using Godot;
using System;

public partial class BasePlant : StaticBody2D
{
    [Export] public int MaxHealth = 100;

    [Export] public int SunCost = 100;

    private int gridX;
    private int gridY;
    private GridManager gridManager;

    private UIManager uiManager;

    protected int currentHealth;

    public override void _Ready()
    {
        currentHealth = MaxHealth;

        uiManager = GetTree().Root.GetNode<UIManager>("Game/UIManager");

        InputPickable = true; // Enable input picking for this plant
    }

    public override void _InputEvent(
    Viewport viewport,
    InputEvent @event,
    int shapeIdx)
    {
        if (!uiManager.IsShovelActive)
            return;

        if (@event is InputEventMouseButton mouseButton &&
            mouseButton.Pressed &&
            mouseButton.ButtonIndex == MouseButton.Left)
        {
            RemovePlant();

            uiManager.IsShovelActive = false;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //GD.Print($"{Name} took {damage} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        //GD.Print($"{Name} died");
        gridManager?.FreeCell(gridX, gridY);
        QueueFree(); // Remove the plant from the scene
    }

    public void SetGridPosition(int x, int y, GridManager manager) // This method is called by the GridManager when the plant is placed on the grid
    {
        gridX = x;
        gridY = y;
        gridManager = manager;
    }

    public virtual void RemovePlant()
    {
        //GD.Print($"{Name} removed by shovel");

        gridManager?.FreeCell(gridX, gridY);

        QueueFree();
    }
}