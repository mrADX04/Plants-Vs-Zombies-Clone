using Godot;
using System;

[Tool] // This allows the grid to update visually in the Inspector
public partial class GridManager : Node2D
{
    [Export] public int CellSize = 16;
    [Export] public Vector2 GridOffset = new Vector2(0, 0); // Change this in Inspector!

    public static GridManager Instance;

    private const int GridWidth = 9;
    private const int GridHeight = 5;

    // ❗ Track which object is in each cell
    private Node2D[,] gridObjects;
    // ❗ NEW: Dedicated occupancy grid
    private bool[,] occupiedCells;

    public override void _Ready()
    {
        gridObjects = new Node2D[GridWidth, GridHeight];

        occupiedCells = new bool[GridWidth, GridHeight];

        Instance = this;
    }

    public override void _Process(double delta)
    {
        // If in editor, redraw constantly so you can see the offset change live
        if (Engine.IsEditorHint())
            QueueRedraw();
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

            DrawLine(
                new Vector2(x, GridOffset.Y),
                new Vector2(x, GridOffset.Y + totalH),
                Colors.Green,
                1.0f
            );
        }

        // Draw Horizontal Lines
        for (int j = 0; j <= GridHeight; j++)
        {
            float y = GridOffset.Y + (j * CellSize);

            DrawLine(
                new Vector2(GridOffset.X, y),
                new Vector2(GridOffset.X + totalW, y),
                Colors.Green,
                1.0f
            );
        }
    }

    // ❗ Convert world position into snapped WORLD position
    public Vector2 GetSnappedPosition(Vector2 worldPos)
    {
        // Convert world mouse position into GridManager local space
        Vector2 localPos = ToLocal(worldPos);

        Vector2 gridSpacePos = localPos - GridOffset;

        int col = Mathf.FloorToInt(gridSpacePos.X / CellSize);
        int row = Mathf.FloorToInt(gridSpacePos.Y / CellSize);

        // Convert snapped LOCAL position
        Vector2 snappedLocalPos = new Vector2(
            GridOffset.X + (col * CellSize) + (CellSize / 2.0f),
            GridOffset.Y + (row * CellSize) + (CellSize / 2.0f)
        );

        // ❗ Convert back into WORLD coordinates
        return ToGlobal(snappedLocalPos);
    }

    // ❗ Convert world position into grid coordinates
    public Vector2I GetGridCoordinates(Vector2 worldPos)
    {
        // Convert world mouse position into GridManager local space
        Vector2 localPos = ToLocal(worldPos);

        Vector2 gridSpacePos = localPos - GridOffset;

        int col = Mathf.FloorToInt(gridSpacePos.X / CellSize);
        int row = Mathf.FloorToInt(gridSpacePos.Y / CellSize);

        return new Vector2I(col, row);
    }

    // ❗ Check if position is inside the playable lawn
    public bool IsInsideGrid(Vector2 worldPos)
    {
        Vector2 localPos = ToLocal(worldPos);

        float left = GridOffset.X;
        float top = GridOffset.Y;

        float right = left + (GridWidth * CellSize);
        float bottom = top + (GridHeight * CellSize);

        return localPos.X >= left &&
               localPos.X < right &&
               localPos.Y >= top &&
               localPos.Y < bottom;
    }

    // ❗ Check if a grid cell is free
    public bool IsCellFree(int col, int row)
    {
        // Bounds safety
        if (col < 0 || col >= GridWidth ||
            row < 0 || row >= GridHeight)
        {
            return false;
        }

        return !occupiedCells[col, row];
    }

    // ❗ Occupy a specific grid cell
    public void OccupyCell(int col, int row, Node2D plant)
    {
        // Safety bounds check
        if (col < 0 || col >= GridWidth ||
            row < 0 || row >= GridHeight)
        {
            GD.PrintErr("Tried to occupy invalid grid cell!");
            return;
        }
        occupiedCells[col, row] = true;

        gridObjects[col, row] = plant;

        // ❗ Assign grid position to BasePlant
        if (plant is BasePlant basePlant)
        {
            basePlant.SetGridPosition(col, row, this);
        }
    }

    // ❗ Called by plants when destroyed
    public void FreeCell(int x, int y)
    {
        if (x >= 0 && x < GridWidth &&
            y >= 0 && y < GridHeight)
        {
            occupiedCells[x, y] = false;

            gridObjects[x, y] = null;

            GD.Print($"Freed cell: {x}, {y}");
        }
    }

}