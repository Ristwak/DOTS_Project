using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTSProject.Systems
{
    public partial struct TowerAttackSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<GameConfig>())
                return;

            if (SystemAPI.GetSingleton<GameConfig>().IsGameOver)
                return;

            EntityQuery enemyQuery = state.GetEntityQuery(
                ComponentType.ReadOnly<EnemyTag>(),
                ComponentType.ReadWrite<EnemyHealth>(),
                ComponentType.ReadOnly<LocalTransform>()
            );

            NativeArray<Entity> enemies =
                enemyQuery.ToEntityArray(Allocator.Temp);

            var transforms =
                SystemAPI.GetComponentLookup<LocalTransform>(true);

            var health =
                SystemAPI.GetComponentLookup<EnemyHealth>(false);

            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (tower, transform) in
                SystemAPI.Query<
                    RefRW<TowerData>,
                    RefRO<LocalTransform>>())
            {
                tower.ValueRW.Cooldown -= deltaTime;

                if (tower.ValueRO.Cooldown > 0f)
                    continue;

                Entity target = Entity.Null;
                float closestDistance = tower.ValueRO.Range;

                float3 towerPosition = transform.ValueRO.Position;

                // Find the nearest enemy inside tower range.
                foreach (Entity enemy in enemies)
                {
                    if (!transforms.HasComponent(enemy) ||
                        !health.HasComponent(enemy))
                        continue;

                    if (health[enemy].Current <= 0f)
                        continue;

                    float distance = math.distance(
                        towerPosition,
                        transforms[enemy].Position
                    );

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        target = enemy;
                    }
                }

                if (target == Entity.Null)
                    continue;

                // Apply damage to the selected enemy.
                EnemyHealth enemyHealth = health[target];

                enemyHealth.Current -= tower.ValueRO.Damage;

                health[target] = enemyHealth;

                // Reset attack cooldown.
                tower.ValueRW.Cooldown =
                    1f / math.max(tower.ValueRO.FireRate, 0.01f);
            }

            enemies.Dispose();
        }
    }
}