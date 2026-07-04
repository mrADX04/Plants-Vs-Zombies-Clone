using Godot;
using System;

public partial class SurvivalTimer : Node
{
    public static SurvivalTimer Instance;

    public int SurvivalSeconds = 0;

    public bool IsRunning = false;

    private double accumulatedTime = 0;

    public override void _Ready()
    {
        Instance = this;

        ProcessMode = ProcessModeEnum.Pausable;
    }

    public override void _Process(double delta)
    {
        if (!IsRunning)
            return;

        accumulatedTime += delta;

        while (accumulatedTime >= 1.0)
        {
            SurvivalSeconds++;
            accumulatedTime -= 1.0;
        }
    }

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void ResetTimer()
    {
        SurvivalSeconds = 0;
        accumulatedTime = 0;
        IsRunning = false;
    }
}