using Godot;

public partial class GameOver : Control
{
    private Button retryButton;
    private Button mainMenuButton;

    public override void _Ready()
    {
        GD.Print("GameOver Ready!");

        GD.Print("GM = " + GameManager.Instance);

        GD.Print("HS = " + HighScoreManager.Instance);

        Label currentScoreLabel = GetNode<Label>("CenterContainer/VBoxContainer/CurrentScoreLabel");

        Label highScoreLabel = GetNode<Label>("CenterContainer/VBoxContainer/HighScoreLabel");

        Label newHighScoreLabel = GetNode<Label>("CenterContainer/VBoxContainer/NewHighScoreLabel");

        Button retryButton = GetNode<Button>("CenterContainer/VBoxContainer/RetryButton");

        Button mainMenuButton = GetNode<Button>("CenterContainer/VBoxContainer/MainMenuButton");

        currentScoreLabel.Text = $"Score: {GameManager.Instance.CurrentScore}";

        highScoreLabel.Text = $"High Score: {HighScoreManager.Instance.HighScore}";

        newHighScoreLabel.Visible = HighScoreManager.Instance.IsNewHighScore;

    }

    private void OnRetryButtonPressed()
    {
        GD.Print(GameManager.Instance);

        GameManager.Instance.CurrentScore = 0;

        SurvivalTimer.Instance.ResetTimer();

        GetTree().ChangeSceneToFile("res://Scenes/game.tscn");
    }

    private void OnMainMenuButtonPressed()
    {
        GD.Print(GameManager.Instance);

        GameManager.Instance.CurrentScore = 0;

        SurvivalTimer.Instance.ResetTimer();

        GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
    }
}