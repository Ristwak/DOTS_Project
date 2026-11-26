using Unity.Entities;

namespace DOTSProject.Systems
{
    [UpdateAfter(typeof(TowerAttackSystem))]
    public partial struct EnemyDeathSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<GameConfig>())
                return;

            RefRW<GameConfig> config =
                SystemAPI.GetSingletonRW<GameConfig>();

            var ecb = SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (health, entity) in
                SystemAPI.Query<RefRO<EnemyHealth>>()
                .WithAll<EnemyTag>()
                .WithEntityAccess())
            {
                if (health.ValueRO.Current > 0f)
                    continue;

                // Reward the player for killing an enemy.
                config.ValueRW.Currency += health.ValueRO.Reward;

                // Remove the dead enemy.
                ecb.DestroyEntity(entity);
            }
        }
    }
}