using Godot;

[GlobalClass]
public partial class WaveData : Resource
{
    [Export] public float SpawnInterval = 2.0f;
    [Export] public int EnemyCount = 10;

    [Export] public int MinSpawnPerTick = 1;
    [Export] public int MaxSpawnPerTick = 3;
}