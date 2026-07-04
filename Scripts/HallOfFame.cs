using Godot;
using System;

public partial class HallOfFame : Control
{
    public override void _Ready()
    {
        Label scoreLabel = GetNode<Label>("HighScoreLabel");

        //scoreLabel.Text = "Highest Score\n\n" + ScoreManager.Instance.HighScore.ToString(); --> Older Version to be decided which one to keep
        scoreLabel.Text = "Highest Score\n\n" + HighScoreManager.Instance.HighScore.ToString();

        GetNode<Button>("BackButton").Pressed += OnBackPressed;
    }

    private void OnBackPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
    }
}