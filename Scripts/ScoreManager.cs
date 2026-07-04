using Godot;
using System;

public partial class ScoreManager : Node
{
    public static ScoreManager Instance;

    public float SurvivalTime = 0f;
    public float HighScore = 0f;

    private const string SavePath = "user://highscore.save";

    public override void _Ready()
    {
        Instance = this;
        LoadHighScore();
    }

    public override void _Process(double delta)
    {
        SurvivalTime += (float)delta;

        if (SurvivalTime > HighScore)
        {
            HighScore = SurvivalTime;
            SaveHighScore();
        }
    }

    public void ResetScore()
    {
        SurvivalTime = 0f;
    }

    private void SaveHighScore()
    {
        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);

        if (file != null)
            file.StoreFloat(HighScore);
    }

    private void LoadHighScore()
    {
        if (!FileAccess.FileExists(SavePath))
            return;

        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);

        if (file != null)
            HighScore = file.GetFloat();
    }
}