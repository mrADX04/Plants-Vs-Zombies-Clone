using Godot;
using System;

public partial class HighScoreManager : Node
{
    public static HighScoreManager Instance;

    private const string SavePath = "user://highscore.save";

    public int HighScore = 0;

    public bool IsNewHighScore = false; //recommendation accpeted

    public override void _Ready()
    {
        Instance = this;

        LoadHighScore();

        GD.Print("Load High Score:" + HighScore);
    }

    public void SetHighScore(int newScore)
    {
        HighScore = newScore;

        SaveHighScore();
    }

    public void SaveHighScore()
    {
        using var file = FileAccess.Open(
            SavePath,
            FileAccess.ModeFlags.Write);

        if (file != null)
            file.Store32((uint)HighScore);
    }

    private void LoadHighScore()
    {
        if (!FileAccess.FileExists(SavePath))
            return;

        using var file = FileAccess.Open(
            SavePath,
            FileAccess.ModeFlags.Read);

        if (file != null)
            HighScore = (int)file.Get32();
    }
}