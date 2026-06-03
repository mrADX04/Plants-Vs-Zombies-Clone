using Godot;
using System;

public partial class PlantGhost : Node2D
{
    [Export] public Node2D PlayableLayer;
    [Export] public Texture2D ShovelGhostTexture;

    private bool showingShovelGhost = false;
    private UIManager uiManager;

    // Tracks which plant the ghost currently represents
    private PlantData currentGhostPlantData;

    // ❗ Visual-only ghost sprite
    private Node2D ghostInstance;

    public override void _Ready()
    {
        uiManager = GetTree().Root.GetNode<UIManager>("Game/UIManager");
    }

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

        // SHOVEL MODE
        if (uiManager != null && uiManager.IsShovelActive)
        {
            ShowShovelGhost();
            return;
        }

        // NORMAL PLANT MODE
        if (selector == null || selector.SelectedPlantData == null)
        {
            RemoveGhost();
            return;
        }

        PlantData selectedData = selector.SelectedPlantData;

        // ❗ Create/rebuild ghost when selection changes
        if (ghostInstance == null || currentGhostPlantData != selectedData)
        {
            RemoveGhost();

            currentGhostPlantData = selectedData;

            Sprite2D ghostSprite = new Sprite2D();

            // Use the GhostTexture directly from PlantData
            ghostSprite.Texture = selectedData.GhostTexture;
            ghostSprite.Centered = true;

            // Semi-transparent silhouette
            ghostSprite.Modulate = new Color(1, 1, 1, 0.5f);

            // Use sprite as ghost
            ghostInstance = ghostSprite;

            // Add ghost to scene
            AddChild(ghostInstance);
        }

        Vector2 mousePos = GetGlobalMousePosition();

        bool insideGrid = GridManager.Instance.IsInsideGrid(mousePos);

        // Hide outside lawn
        ghostInstance.Visible = insideGrid;

        if (!insideGrid)
            return;

        // Snap ghost to grid
        Vector2 snappedPos = GridManager.Instance.GetSnappedPosition(mousePos);

        ghostInstance.GlobalPosition = snappedPos;
    }

    private void ShowShovelGhost()
    {
        if (!showingShovelGhost)
        {
            RemoveGhost();

            Sprite2D ghostSprite = new Sprite2D();

            ghostSprite.Texture = ShovelGhostTexture;
            ghostSprite.Centered = true;
            ghostSprite.Modulate = new Color(1, 1, 1, 0.5f);

            ghostInstance = ghostSprite;

            AddChild(ghostInstance);

            showingShovelGhost = true;
        }

        ghostInstance.GlobalPosition = GetGlobalMousePosition();
    }

    private void TryPlacePlant()
    {
        var selector = PlantSelector.Instance;

        // Nothing selected
        if (selector.SelectedPlantData == null)
            return;

        Vector2 mousePos = GetGlobalMousePosition();

        // Prevent outside placement
        if (!GridManager.Instance.IsInsideGrid(mousePos))
            return;

        Vector2I gridCoords = GridManager.Instance.GetGridCoordinates(mousePos);

        int col = gridCoords.X;
        int row = gridCoords.Y;

        // Prevent occupied placement
        if (!GridManager.Instance.IsCellFree(col, row))
            return;

        Vector2 snappedPos = GridManager.Instance.GetSnappedPosition(mousePos);

        // ❗ Create REAL plant
        BasePlant plant = selector.SelectedPlantData.PlantScene.Instantiate<BasePlant>();

        // Add to gameplay layer
        PlayableLayer.AddChild(plant);

        // Position plant
        plant.GlobalPosition = snappedPos;

        // Mark cell occupied
        GridManager.Instance.OccupyCell(col, row, plant);

        // Deduct sun cost
        SunManager.Instance.SpendSun(selector.SelectedPlantData.SunCost);

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

        currentGhostPlantData = null;
        showingShovelGhost = false;
    }
}