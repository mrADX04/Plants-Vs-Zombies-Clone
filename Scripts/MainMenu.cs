using Godot;

public partial class MainMenu : Control
{
    private Button startButton;
    private Button halloffameButton;
    private Button quitButton;

    public override void _Ready()
    {
        startButton = GetNode<Button>("ButtonContainer/StartButton");
        halloffameButton = GetNode<Button>("ButtonContainer/HallOfFameButton");
        quitButton = GetNode<Button>("ButtonContainer/QuitButton");

        startButton.Pressed += OnStartPressed;
        halloffameButton.Pressed += OnHallOfFamePressed;
        quitButton.Pressed += OnQuitPressed;
    }

    private void OnStartPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/game.tscn");
    }

    private void OnHallOfFamePressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/hall_of_fame.tscn");
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}