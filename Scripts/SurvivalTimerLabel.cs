using Godot;
using System;

public partial class SurvivalTimerLabel : Label
{
    public override void _Process(double delta)
    {
        int totalSeconds = SurvivalTimer.Instance.SurvivalSeconds;

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        Text = $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}