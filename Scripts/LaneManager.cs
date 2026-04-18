using Godot;
using System.Collections.Generic;

public partial class LaneManager : Node
{
    public static LaneManager Instance;

    private Dictionary<int, List<Node2D>> lanes = new();

    public override void _Ready()
    {
        Instance = this;
    }

    public void RegisterEnemy(int lane, Node2D enemy)
    {
        if (!lanes.ContainsKey(lane))
            lanes[lane] = new List<Node2D>();

        lanes[lane].Add(enemy);
    }

    public void UnregisterEnemy(int lane, Node2D enemy)
    {
        if (lanes.ContainsKey(lane))
            lanes[lane].Remove(enemy);
    }

    public bool HasEnemyInLane(int lane, float plantX)
    {
        if (!lanes.ContainsKey(lane))
            return false;

        foreach (var enemy in lanes[lane])
        {
            if (enemy.GlobalPosition.X > plantX)
                return true;
        }

        return false;
    }
}