using Godot;
using System;

public partial class UIManager : CanvasLayer
{
    private Label sunLabel;

    public override void _Ready()
    {

        sunLabel = GetNode<Label>("TopBar/SunCounter/SunLabel");

        // Initialize the sun counter with the current sun amount & This prevents UIManager from crashing if SunManager.Instance is null when UIManager is ready.
        if (SunManager.Instance != null)
        {
            sunLabel.Text =
                SunManager.Instance.CurrentSun.ToString();

            SunManager.Instance.SunChanged += UpdateSunCounter;
        }

    }
    private void UpdateSunCounter(int amount)
    {
        sunLabel.Text = amount.ToString();
    }
}