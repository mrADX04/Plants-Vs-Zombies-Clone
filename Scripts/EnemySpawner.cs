using Godot;
using System;

public partial class EnemySpawner : Node
{
    [Export] public PackedScene[] EnemyScenes; // Support for multiple enemy types in future waves

    [Export] public NodePath[] LaneSpawnPoints;
    
    [Export] public Godot.Collections.Array<WaveData> Waves;

    [Export] public int MaxEnemiesOnScreen = 8;

    //public Action OnEnemyDied; -> Feature for later: Track enemy deaths to manage currentEnemiesAlive count.

    private int currentEnemiesAlive = 0;

    private Timer spawnTimer;
    private Random random = new Random();

    private int currentWave = 0;
    private int enemiesSpawnedInWave = 0;

    private int lastLane = -1; // For potential future use: Avoid spawning in the same lane repeatedly.

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

        // ✅ 1. Stop if wave finished
        if (enemiesSpawnedInWave >= wave.EnemyCount)
        {
            spawnTimer.Stop();
            currentWave++;

            GetTree().CreateTimer(3.0f).Timeout += StartWave;
            return;
        }

        // ✅ 2. Soft on-screen cap (temporary system)
        int currentEnemies = GetTree().GetNodesInGroup("Enemies").Count;
        if (currentEnemies >= 8) // tweak this value later
            return;

        // ✅ 3. Wave progress (0 → 1)
        float progress = (float)enemiesSpawnedInWave / wave.EnemyCount;

        // ✅ 4. Dynamic spawn amount (PvZ-style pacing)
        int spawnAmount;

        if (progress < 0.3f)
        {
            spawnAmount = 1; // slow start
        }
        else if (progress < 0.7f)
        {
            spawnAmount = random.Next(1, 3); // medium
        }
        else
        {
            spawnAmount = random.Next(2, 4); // rush
        }

        // ✅ 5. Clamp to wave settings
        int min = Mathf.Min(wave.MinSpawnPerTick, wave.MaxSpawnPerTick);
        int max = Mathf.Max(wave.MinSpawnPerTick, wave.MaxSpawnPerTick);

        spawnAmount = Mathf.Clamp(spawnAmount, min, max);

        // ✅ 6. Spawn loop
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

        // Randomly select a lane, ensuring we don't spawn in the same lane repeatedly if there are multiple lanes
        int laneIndex;

        do
        {
            laneIndex = random.Next(LaneSpawnPoints.Length);
        }
        while (LaneSpawnPoints.Length > 1 && laneIndex == lastLane);

        lastLane = laneIndex;

        var spawnPoint = GetNode<Node2D>(LaneSpawnPoints[laneIndex]);

        int enemyIndex = random.Next(EnemyScenes.Length);
        var enemy = EnemyScenes[enemyIndex].Instantiate<BaseEnemy>();

        // 🔥 Assign lane BEFORE adding to scene
        enemy.Lane = laneIndex;

        if (currentEnemiesAlive >= MaxEnemiesOnScreen)  // Soft on-screen cap on enemies
            return;

        enemy.GlobalPosition = spawnPoint.GlobalPosition;

        currentEnemiesAlive++;  // Increment alive count when spawning

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
//step 4 : continue