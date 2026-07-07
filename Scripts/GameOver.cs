using Godot;

public partial class GameOver : Control
{
    private Button retryButton;
    private Button mainMenuButton;

    public override void _Ready()
    {
        Label currentScoreLabel = GetNode<Label>("CenterContainer/VBoxContainer/CurrentScoreLabel");

        Label highScoreLabel = GetNode<Label>("CenterContainer/VBoxContainer/HighScoreLabel");

        Label newHighScoreLabel = GetNode<Label>("CenterContainer/VBoxContainer/NewHighScoreLabel");

        retryButton = GetNode<Button>("CenterContainer/VBoxContainer/RetryButton");

        mainMenuButton = GetNode<Button>("CenterContainer/VBoxContainer/MainMenuButton");

        currentScoreLabel.Text = $"Score: {SurvivalTimer.Instance.SurvivalSeconds}";

        highScoreLabel.Text = $"High Score: {HighScoreManager.Instance.HighScore}";

        GD.Print("IsNewHighScore = " + HighScoreManager.Instance.IsNewHighScore);
        newHighScoreLabel.Visible = HighScoreManager.Instance.IsNewHighScore;

        retryButton.Pressed += OnRetryButtonPressed;
        mainMenuButton.Pressed += OnMainMenuButtonPressed;

    }

    private void OnRetryButtonPressed()
    {
        SurvivalTimer.Instance.ResetTimer();

        GameManager.Instance.ResetGame();

        GetTree().ChangeSceneToFile("res://Scenes/game.tscn");
    }

    private void OnMainMenuButtonPressed()
    {
        SurvivalTimer.Instance.ResetTimer();

        GameManager.Instance.ResetGame();

        GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
    }
}