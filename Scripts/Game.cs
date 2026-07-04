using Godot;
using System;

public partial class Game : Node2D
{
    public override void _Ready()
    {
        SurvivalTimer.Instance.ResetTimer();
        SurvivalTimer.Instance.StartTimer();
    }
}