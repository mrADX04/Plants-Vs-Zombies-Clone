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

    // ❗ NEW: Track which object is in each cell instead of just bool
    private Node2D[,] gridObjects;

    public override void _Ready()
    {
        // ❗ NEW: Initialize grid storage
        gridObjects = new Node2D[GridWidth, GridHeight];
    }

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
                // ❗ NEW: Check if this cell is already occupied
                if (gridObjects[x, y] != null)
                {
                    GD.Print("Cell already occupied!");
                    return;
                }

                // 4. Calculate the Top-Left of the specific cell
                float cellX = GridOffset.X + (x * CellSize);
                float cellY = GridOffset.Y + (y * CellSize);

                // 5. Add half the cell size to reach the center of that cell
                Vector2 snapPos = new Vector2(
                    cellX + (CellSize / 2.0f),
                    cellY + (CellSize / 2.0f)
                );

                // ❗ UPDATED: store the returned object
                Node2D placed = SpawnAt(snapPos);

                // ❗ NEW: store reference in grid
                gridObjects[x, y] = placed;

                // ❗ NEW: assign grid position to plant (for auto-free later)
                if (placed is BasePlant plant)
                {
                    plant.SetGridPosition(x, y, this);
                }
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
            DrawLine(new Vector2(x, GridOffset.Y), new Vector2(x, GridOffset.Y + totalH), Colors.Green, 1.0f);
        }

        // Draw Horizontal Lines
        for (int j = 0; j <= GridHeight; j++)
        {
            float y = GridOffset.Y + (j * CellSize);
            DrawLine(new Vector2(GridOffset.X, y), new Vector2(GridOffset.X + totalW, y), Colors.Green, 1.0f);
        }
    }

    private Node2D SpawnAt(Vector2 pos)
    {
        GD.Print("Spawning at: " + pos);

        // 1. Safety check: make sure you've assigned something in the Inspector
        if (ObjectToSpawn == null)
        {
            GD.PrintErr("Error: ObjectToSpawn is not assigned in the Inspector!");
            return null;
        }

        // 2. Instantiate as a Node2D
        Node2D newObject = ObjectToSpawn.Instantiate<Node2D>();

        // 3. Set the position to our calculated snap point
        newObject.Position = pos;

        // 4. Add it as a child of the GridManager
        AddChild(newObject);

        GD.Print("Successfully spawned at: " + pos);

        // ❗ NEW: return object so we can track it
        return newObject;
    }

    // ❗ NEW: Called by plants when they die
    public void FreeCell(int x, int y)
    {
        if (x >= 0 && x < GridWidth && y >= 0 && y < GridHeight)
        {
            gridObjects[x, y] = null;
            GD.Print($"Freed cell: {x}, {y}");
        }
    }
}