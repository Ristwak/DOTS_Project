using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTSProject.Systems
{
    [BurstCompile]
    public partial struct EnemyMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<GameConfig>())
                return;

            RefRW<GameConfig> config =
                SystemAPI.GetSingletonRW<GameConfig>();

            DynamicBuffer<WaypointElement> path =
                SystemAPI.GetSingletonBuffer<WaypointElement>();

            if (path.Length == 0 || config.ValueRO.IsGameOver)
                return;

            float deltaTime = SystemAPI.Time.DeltaTime;

            var ecb = SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (transform, progress, speed, entity) in
                SystemAPI.Query<
                    RefRW<LocalTransform>,
                    RefRW<EnemyPathProgress>,
                    RefRO<EnemyMoveSpeed>>()
                .WithAll<EnemyTag>()
                .WithEntityAccess())
            {
                int index = progress.ValueRO.Index;

                // Enemy has reached the base.
                if (index >= path.Length)
                {
                    config.ValueRW.BaseHealth--;
                    ecb.DestroyEntity(entity);
                    continue;
                }

                float3 target = path[index].Position;
                float3 current = transform.ValueRO.Position;

                float3 difference = target - current;
                float distance = math.length(difference);

                float movement = speed.ValueRO.Value * deltaTime;

                if (distance <= movement || distance < 0.01f)
                {
                    transform.ValueRW.Position = target;
                    progress.ValueRW.Index++;
                }
                else
                {
                    float3 direction = math.normalize(difference);

                    transform.ValueRW.Position +=
                        direction * movement;

                    transform.ValueRW.Rotation =
                        quaternion.LookRotationSafe(
                            direction,
                            math.up()
                        );
                }
            }
        }
    }
}