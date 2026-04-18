using Godot;
using System;

public partial class EnemySpawner : Node
{
    [Export] public PackedScene EnemyScene;

    [Export] public NodePath[] LaneSpawnPoints;
    
    [Export] public Godot.Collections.Array<WaveData> Waves;

    private Timer spawnTimer;
    private Random random = new Random();

    private int currentWave = 0;
    private int enemiesSpawnedInWave = 0;

    public override void _Ready()
    {
        spawnTimer = new Timer();
        AddChild(spawnTimer);

        spawnTimer.Timeout += OnSpawnTimerTimeout;

        StartWave();

    }

    private void OnSpawnTimerTimeout()
    {
        var wave = Waves[currentWave];

        if (enemiesSpawnedInWave >= wave.EnemyCount)
        {
            spawnTimer.Stop();
            currentWave++;

            GetTree().CreateTimer(3.0f).Timeout += StartWave;
            return;
        }

        int min = Mathf.Min(wave.MinSpawnPerTick, wave.MaxSpawnPerTick);
        int max = Mathf.Max(wave.MinSpawnPerTick, wave.MaxSpawnPerTick);

        int spawnAmount = random.Next(min, max + 1);

        for (int i = 0; i < spawnAmount; i++)
        {
            if (enemiesSpawnedInWave >= wave.EnemyCount)
                break;

            SpawnEnemy();
            enemiesSpawnedInWave++;
        }
    }

    private void SpawnEnemy()
    {
        if (LaneSpawnPoints.Length == 0)
        {
            GD.PrintErr("❌ EnemySpawner: No lane spawn points assigned in Inspector!");
            return;
        }

        int laneIndex = random.Next(LaneSpawnPoints.Length);

        var spawnPoint = GetNode<Node2D>(LaneSpawnPoints[laneIndex]);

        var enemy = EnemyScene.Instantiate<BaseEnemy>();

        // 🔥 Assign lane BEFORE adding to scene
        enemy.Lane = laneIndex;

        enemy.GlobalPosition = spawnPoint.GlobalPosition;

        GetParent().AddChild(enemy);
    }

    private void StartWave()
    {
        if (Waves.Count == 0)
        {
            GD.PrintErr("❌ No waves assigned!");
            return;
        }

        if (currentWave >= Waves.Count)
        {
            GD.Print("✅ All waves completed!");
            return;
        }

        var wave = Waves[currentWave];

        spawnTimer.WaitTime = wave.SpawnInterval;
        spawnTimer.Start();

        enemiesSpawnedInWave = 0;

        GD.Print($"🌊 Starting Wave {currentWave + 1}");
    }
}