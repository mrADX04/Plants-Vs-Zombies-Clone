using Godot;
using System;

public partial class Wallnut : BasePlant
{
    [Export] public Texture2D FullHealthTexture;
    [Export] public Texture2D DamagedTexture;
    [Export] public Texture2D CriticalTexture;

    [Export] public float DamagedThreshold = 0.66f;
    [Export] public float CriticalThreshold = 0.33f;

    private Sprite2D sprite;

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode<Sprite2D>("Sprite2D");

        UpdateVisualState();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        float healthPercent = (float)currentHealth / MaxHealth;

        // Critical state
        if (healthPercent <= CriticalThreshold)
        {
            sprite.Texture = CriticalTexture;
        }

        // Damaged state
        else if (healthPercent <= DamagedThreshold)
        {
            sprite.Texture = DamagedTexture;
        }

        // Full health state
        else
        {
            sprite.Texture = FullHealthTexture;
        }
    }
}