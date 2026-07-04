using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance;

    public int CurrentScore = 0;

    public override void _Ready()
    {
        Instance = this;

        GD.Print("GameManager Ready!"); //check only , remove later
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
    }

    public void GameOver()
    {
        HighScoreManager.Instance.IsNewHighScore = false;

        if (CurrentScore > HighScoreManager.Instance.HighScore)
        {
            HighScoreManager.Instance.IsNewHighScore = true;

            HighScoreManager.Instance.SetHighScore(CurrentScore);
        }

        GetTree().CallDeferred(
            SceneTree.MethodName.ChangeSceneToFile,"res://Scenes/game_over.tscn");
    }
}