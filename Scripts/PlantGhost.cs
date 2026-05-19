using Godot;
using System;

public partial class PlantGhost : Node2D
{
    [Export] public Node2D PlayableLayer;

    // ❗ Visual-only ghost sprite
    private Node2D ghostInstance;

    public override void _Process(double delta)
    {
        UpdateGhost();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent &&
            mouseEvent.Pressed &&
            mouseEvent.ButtonIndex == MouseButton.Left)
        {
            TryPlacePlant();
        }
    }

    private void UpdateGhost()
    {
        var selector = PlantSelector.Instance;

        // Nothing selected
        if (selector == null || selector.SelectedPlantScene == null)
        {
            RemoveGhost();
            return;
        }

        // ❗ Create ghost if it doesn't exist
        if (ghostInstance == null)
        {
            // Instantiate the real plant temporarily
            Node2D tempPlant =
                selector.SelectedPlantScene.Instantiate<Node2D>();

            // ❗ Get ONLY the visual sprite
            Sprite2D originalSprite =
                tempPlant.GetNode<Sprite2D>("Sprite2D");

            // ❗ Create a brand-new sprite for the ghost
            Sprite2D ghostSprite = new Sprite2D();

            // Copy visuals
            ghostSprite.Texture = originalSprite.Texture;
            ghostSprite.Scale = originalSprite.Scale;
            ghostSprite.Rotation = originalSprite.Rotation;
            ghostSprite.FlipH = originalSprite.FlipH;
            ghostSprite.FlipV = originalSprite.FlipV;

            // Semi-transparent silhouette
            ghostSprite.Modulate =
                new Color(1, 1, 1, 0.5f);

            // Use sprite as ghost
            ghostInstance = ghostSprite;

            // Cleanup temporary plant
            tempPlant.QueueFree();

            // Add ghost to scene
            AddChild(ghostInstance);
        }

        Vector2 mousePos = GetGlobalMousePosition();

        bool insideGrid =
            GridManager.Instance.IsInsideGrid(mousePos);

        // Hide outside lawn
        ghostInstance.Visible = insideGrid;

        if (!insideGrid)
            return;

        // Snap ghost to grid
        Vector2 snappedPos =
            GridManager.Instance.GetSnappedPosition(mousePos);

        ghostInstance.GlobalPosition = snappedPos;
    }

    private void TryPlacePlant()
    {
        var selector = PlantSelector.Instance;

        // Nothing selected
        if (selector.SelectedPlantScene == null)
            return;

        Vector2 mousePos = GetGlobalMousePosition();

        // Prevent outside placement
        if (!GridManager.Instance.IsInsideGrid(mousePos))
            return;

        Vector2I gridCoords =
            GridManager.Instance.GetGridCoordinates(mousePos);

        int col = gridCoords.X;
        int row = gridCoords.Y;

        // Prevent occupied placement
        if (!GridManager.Instance.IsCellFree(col, row))
            return;

        Vector2 snappedPos =
            GridManager.Instance.GetSnappedPosition(mousePos);

        // ❗ Create REAL plant
        Node2D plant =
            selector.SelectedPlantScene.Instantiate<Node2D>();

        // Add to gameplay layer
        PlayableLayer.AddChild(plant);

        // Position plant
        plant.GlobalPosition = snappedPos;

        // Mark cell occupied
        GridManager.Instance.OccupyCell(col, row, plant);

        // Clear selection after ONE placement
        selector.ClearSelection();

        // Remove ghost
        RemoveGhost();
    }

    private void RemoveGhost()
    {
        if (ghostInstance != null)
        {
            ghostInstance.QueueFree();
            ghostInstance = null;
        }
    }
}