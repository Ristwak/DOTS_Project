using Unity.Entities;
using UnityEngine;

public class GameConfigAuthoring : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject EnemyPrefab;

    [Header("Enemy Settings")]
    public float EnemyHealth = 100f;
    public float EnemySpeed = 3f;

    [Header("Wave Settings")]
    public int EnemiesPerWave = 5;
    public float SpawnInterval = 2f;
    public float TimeBetweenWaves = 5f;

    [Header("Base Settings")]
    public int BaseHealth = 20;
    public int StartingCurrency = 100;

    [Header("Enemy Path")]
    public Transform[] Waypoints;

    public class Baker : Baker<GameConfigAuthoring>
    {
        public override void Bake(GameConfigAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            Entity enemyPrefab = GetEntity(
                authoring.EnemyPrefab,
                TransformUsageFlags.Dynamic
            );

            AddComponent(entity, new GameConfig
            {
                EnemyPrefab = enemyPrefab,
                EnemyHealth = authoring.EnemyHealth,
                EnemySpeed = authoring.EnemySpeed,
                EnemiesPerWave = authoring.EnemiesPerWave,
                SpawnInterval = authoring.SpawnInterval,
                TimeBetweenWaves = authoring.TimeBetweenWaves,
                BaseHealth = authoring.BaseHealth,
                Currency = authoring.StartingCurrency,
                IsGameOver = false
            });

            AddComponent(entity, new WaveState
            {
                CurrentWave = 1,
                SpawnedThisWave = 0,
                SpawnTimer = 0f,
                IntermissionTimer = 0f,
                WaitingForNextWave = false
            });

            DynamicBuffer<WaypointElement> path =
                AddBuffer<WaypointElement>(entity);

            foreach (Transform waypoint in authoring.Waypoints)
            {
                if (waypoint == null)
                    continue;

                path.Add(new WaypointElement
                {
                    Position = waypoint.position
                });
            }
        }
    }
}