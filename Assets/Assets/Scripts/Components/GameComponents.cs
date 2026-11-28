using Unity.Entities;
using Unity.Mathematics;

// Identifies an enemy entity.
public struct EnemyTag : IComponentData { }

// Enemy health and reward.
public struct EnemyHealth : IComponentData
{
    public float Current;
    public float Max;
    public int Reward;
}

// Enemy movement speed.
public struct EnemyMoveSpeed : IComponentData
{
    public float Value;
}

// Tracks the enemy's current waypoint.
public struct EnemyPathProgress : IComponentData
{
    public int Index;
}

// Tower combat configuration and attack cooldown.
public struct TowerData : IComponentData
{
    public float Range;
    public float Damage;
    public float FireRate;
    public float Cooldown;
}

// Main game configuration.
public struct GameConfig : IComponentData
{
    public Entity EnemyPrefab;
    public float EnemyHealth;
    public float EnemySpeed;
    public float SpawnInterval;
    public float TimeBetweenWaves;
    public int EnemiesPerWave;
    public int BaseHealth;
    public int Currency;
    public bool IsGameOver;
}

// Wave progression data.
public struct WaveState : IComponentData
{
    public int CurrentWave;
    public int SpawnedThisWave;
    public float SpawnTimer;
    public float IntermissionTimer;
    public bool WaitingForNextWave;
}

// Waypoint positions stored on the configuration entity.
public struct WaypointElement : IBufferElementData
{
    public float3 Position;
}