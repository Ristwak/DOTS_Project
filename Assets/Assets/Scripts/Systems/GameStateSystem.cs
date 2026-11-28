using Unity.Entities;
using UnityEngine;

namespace DOTSProject.Systems
{
    public partial struct GameStateSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<GameConfig>())
                return;

            RefRW<GameConfig> config =
                SystemAPI.GetSingletonRW<GameConfig>();

            if (config.ValueRO.BaseHealth <= 0 &&
                !config.ValueRO.IsGameOver)
            {
                config.ValueRW.BaseHealth = 0;
                config.ValueRW.IsGameOver = true;

                Debug.Log("GAME OVER! Your base has been destroyed.");
            }
        }
    }
}