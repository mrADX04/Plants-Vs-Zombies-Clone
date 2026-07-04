using Godot;

public partial class LoseZone : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print("Something entered: " + body.Name);

        GameManager.Instance.GameOver();
    }
}