using Godot;
using System;

public partial class Sun : Area2D
{
    [Export] public int SunValue = 50;

    [Export] public float MoveSpeed = 500f;

    // Position of the UI counter
    private Vector2 targetPosition = new Vector2(90, 35);

    private bool isCollected = false;

    private CollisionShape2D collisionShape;

    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>(
        "CollisionShape2D" );
        
        // Detect mouse clicks
        InputEvent += OnInputEvent;
    }

    public override void _Process(double delta)
    {
        // Only move after being clicked
        if (!isCollected)
            return;

        // Move toward UI counter
        GlobalPosition = GlobalPosition.MoveToward(
            targetPosition,
            MoveSpeed * (float)delta
        );

        // Reached counter
        if (GlobalPosition.DistanceTo(targetPosition) < 10f)
        {
            // Add sun currency
            SunManager.Instance.AddSun(SunValue);

            // Delete sun
            QueueFree();
        }
    }

    private void OnInputEvent(
        Node viewport,
        InputEvent @event,
        long shapeIdx
    )
    {
        // Prevent multiple clicks
        if (isCollected)
            return;

        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed &&
                mouseButton.ButtonIndex == MouseButton.Left)
            {
                isCollected = true;

                // Disable further interaction
                collisionShape.Disabled = true;

                // Stop click from reaching other suns
                GetViewport().SetInputAsHandled();
            }
        }
    }
}