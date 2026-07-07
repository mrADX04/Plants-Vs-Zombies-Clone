using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance;

    private bool gameOverTriggered = false;

    public override void _Ready()
    {
        Instance = this;
    }

    public void GameOver()
    {

        if (gameOverTriggered)
            return;

        gameOverTriggered = true;

        HighScoreManager.Instance.IsNewHighScore = false;

        int finalScore = SurvivalTimer.Instance.SurvivalSeconds;

        GD.Print("Final Score: ", finalScore);
        GD.Print("Old High Score: ", HighScoreManager.Instance.HighScore);

        if (finalScore > HighScoreManager.Instance.HighScore)
        {
            GD.Print("NEW HIGH SCORE!");

            HighScoreManager.Instance.IsNewHighScore = true;

            HighScoreManager.Instance.SetHighScore(finalScore);
        }

        GD.Print("Flag = ", HighScoreManager.Instance.IsNewHighScore);

        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile,"res://Scenes/game_over.tscn");
    }

    public void ResetGame()
    {
        gameOverTriggered = false;
    }
}