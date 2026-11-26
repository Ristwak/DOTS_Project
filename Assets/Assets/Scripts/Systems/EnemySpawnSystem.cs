using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace DOTSProject.Systems
{
    public partial struct EnemySpawnSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<GameConfig>() ||
                !SystemAPI.HasSingleton<WaveState>())
                return;

            RefRW<GameConfig> config =
                SystemAPI.GetSingletonRW<GameConfig>();

            RefRW<WaveState> wave =
                SystemAPI.GetSingletonRW<WaveState>();

            if (config.ValueRO.IsGameOver)
                return;

            DynamicBuffer<WaypointElement> path =
                SystemAPI.GetSingletonBuffer<WaypointElement>();

            if (path.Length == 0)
                return;

            EntityQuery enemies = SystemAPI.QueryBuilder()
                .WithAll<EnemyTag>()
                .Build();

            float deltaTime = SystemAPI.Time.DeltaTime;

            // Wait until all enemies from the current wave are gone.
            if (wave.ValueRO.SpawnedThisWave >=
                config.ValueRO.EnemiesPerWave)
            {
                if (enemies.CalculateEntityCount() == 0)
                {
                    wave.ValueRW.WaitingForNextWave = true;
                    wave.ValueRW.IntermissionTimer += deltaTime;

                    if (wave.ValueRO.IntermissionTimer >=
                        config.ValueRO.TimeBetweenWaves)
                    {
                        wave.ValueRW.CurrentWave++;
                        wave.ValueRW.SpawnedThisWave = 0;
                        wave.ValueRW.SpawnTimer = 0f;
                        wave.ValueRW.IntermissionTimer = 0f;
                        wave.ValueRW.WaitingForNextWave = false;
                    }
                }

                return;
            }

            wave.ValueRW.SpawnTimer += deltaTime;

            if (wave.ValueRO.SpawnTimer <
                config.ValueRO.SpawnInterval)
                return;

            wave.ValueRW.SpawnTimer = 0f;

            Entity enemy = state.EntityManager.Instantiate(
                config.ValueRO.EnemyPrefab
            );

            float3 spawnPosition = path[0].Position;

            state.EntityManager.SetComponentData(
                enemy,
                LocalTransform.FromPosition(spawnPosition)
            );

            state.EntityManager.SetComponentData(
                enemy,
                new EnemyPathProgress { Index = 1 }
            );

            // Increase enemy difficulty with each wave.
            float health = config.ValueRO.EnemyHealth +
                (wave.ValueRO.CurrentWave - 1) * 15f;

            float speed = config.ValueRO.EnemySpeed +
                (wave.ValueRO.CurrentWave - 1) * 0.15f;

            state.EntityManager.SetComponentData(
                enemy,
                new EnemyHealth
                {
                    Current = health,
                    Max = health,
                    Reward = 10
                }
            );

            state.EntityManager.SetComponentData(
                enemy,
                new EnemyMoveSpeed { Value = speed }
            );

            wave.ValueRW.SpawnedThisWave++;
        }
    }
}