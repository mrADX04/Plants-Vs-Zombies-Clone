using Godot;
using System;

public partial class UIManager : CanvasLayer
{
    private Label sunLabel;

    private TextureButton settingsButton;

    private Panel settingsPopup;

    private Button resumeButton;
    private Button restartButton;
    private Button mainMenuButton;
    private Button exitButton;

    public bool IsShovelActive = false;

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

        settingsButton = GetNode<TextureButton>("TopBar/SettingsButton");

        settingsPopup = GetNode<Panel>("SettingsPopup");

        resumeButton = GetNode<Button>("SettingsPopup/VBoxContainer/ResumeButton");
        restartButton = GetNode<Button>("SettingsPopup/VBoxContainer/RestartButton");
        mainMenuButton = GetNode<Button>("SettingsPopup/VBoxContainer/MainMenuButton");
        exitButton = GetNode<Button>("SettingsPopup/VBoxContainer/ExitButton");

        // Connect button signals
        settingsButton.Pressed += OnSettingsButtonPressed;
        resumeButton.Pressed += OnResumeButtonPressed;
        restartButton.Pressed += OnRestartButtonPressed;
        mainMenuButton.Pressed += OnMainMenuButtonPressed;
        exitButton.Pressed += OnExitButtonPressed;

    }
    
    private void UpdateSunCounter(int amount)
    {
        sunLabel.Text = amount.ToString();
    }

    public void OnShovelButtonPressed()
    {
        IsShovelActive = !IsShovelActive;

        if (IsShovelActive)
        {
            PlantSelector.Instance.ClearSelection();
        }

        GD.Print("Shovel Active: " + IsShovelActive);

    }

    private void OnSettingsButtonPressed()
    {
        settingsPopup.Visible = true;
        GetTree().Paused = true;
    }

    private void OnResumeButtonPressed()
    {
        settingsPopup.Visible = false;
        GetTree().Paused = false;
    }

    private void OnRestartButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    private void OnMainMenuButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}