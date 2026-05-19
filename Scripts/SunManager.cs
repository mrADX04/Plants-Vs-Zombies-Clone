using Godot;
using System;

public partial class SunManager : Node
{
    public static SunManager Instance;

    public int CurrentSun = 50;

    [Signal]
    public delegate void SunChangedEventHandler(int newAmount);

    public override void _Ready()
    {
        Instance = this;
    }

    public void AddSun(int amount)
    {
        CurrentSun += amount;

        EmitSignal(SignalName.SunChanged, CurrentSun);

        GD.Print("Sun: " + CurrentSun);
    }
}