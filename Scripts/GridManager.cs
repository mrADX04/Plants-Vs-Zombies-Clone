using Godot;
using System;

[Tool] // This allows the grid to update visually in the Inspector
public partial class GridManager : Node2D
{
    [Export] public int CellSize = 64;
    [Export] public Vector2 GridOffset = new Vector2(0, 0); // Change this in Inspector!
    [Export] PackedScene ObjectToSpawn;

    private const int GridWidth = 9;
    private const int GridHeight = 5;

    public override void _Process(double delta)
    {
        // If in editor, redraw constantly so you can see the offset change live
        if (Engine.IsEditorHint()) QueueRedraw();
    }

    public override void _Input(InputEvent @event)
    {
        // Don't run click logic in the editor
        if (Engine.IsEditorHint()) return;

        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            // 1. Get Mouse relative to the Node
            Vector2 localMouse = GetLocalMousePosition();

            // 2. Subtract the Offset to find the "Grid Space"
            Vector2 gridSpacePos = localMouse - GridOffset;

            int x = (int)Mathf.Floor(gridSpacePos.X / CellSize);
            int y = (int)Mathf.Floor(gridSpacePos.Y / CellSize);

            // 3. Check Bounds
            if (x >= 0 && x < GridWidth && y >= 0 && y < GridHeight)
            {
                // 4. Calculate the Top-Left of the specific cell
                float cellX = GridOffset.X + (x * CellSize);
                float cellY = GridOffset.Y + (y * CellSize);

                // 5. Add half the cell size to reach the center of that cell
                Vector2 snapPos = new Vector2(
                    cellX + (CellSize / 2.0f),
                    cellY + (CellSize / 2.0f)
                    );

                SpawnAt(snapPos);
            }
        }
    }

    public override void _Draw()
    {
        float totalW = GridWidth * CellSize;
        float totalH = GridHeight * CellSize;

        // DRAWING USES THE OFFSET
        // Draw Vertical Lines
        for (int i = 0; i <= GridWidth; i++)
        {
            float x = GridOffset.X + (i * CellSize);
            DrawLine(new Vector2(x, GridOffset.Y), new Vector2(x, GridOffset.Y + totalH), Colors.White, 1.0f);
        }

        // Draw Horizontal Lines
        for (int j = 0; j <= GridHeight; j++)
        {
            float y = GridOffset.Y + (j * CellSize);
            DrawLine(new Vector2(GridOffset.X, y), new Vector2(GridOffset.X + totalW, y), Colors.White, 1.0f);
        }
    }

    private void SpawnAt(Vector2 pos)
    {
        GD.Print("Spawning at: " + pos);
        // 1. Safety check: make sure you've assigned something in the Inspector
        if (ObjectToSpawn == null)
        {
            GD.PrintErr("Error: ObjectToSpawn is not assigned in the Inspector!");
            return;
        }

        // 1. Instantiate as a CharacterBody2D
        // We use <CharacterBody2D> here instead of <Node2D>
        CharacterBody2D newCharacter = ObjectToSpawn.Instantiate<CharacterBody2D>();

        // 3. Set the position to our calculated snap point
        newCharacter.Position = pos;

        // 4. Add it as a child of the GridManager
        AddChild(newCharacter);

        GD.Print("Successfully spawned at: " + pos);

    }
}