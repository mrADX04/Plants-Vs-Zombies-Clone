using Godot;
using System;

public partial class ShovelTool : Node
{
    public static ShovelTool Instance;

    public bool IsActive = false;

    public override void _Ready()
    {
        Instance = this;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Toggle()
    {
        IsActive = !IsActive;
    }
}