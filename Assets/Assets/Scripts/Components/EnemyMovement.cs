using Unity.Entities;
using Unity.Mathematics;

namespace DOTSProject.Gameplay
{
    public struct EnemyMovement : IComponentData
    {
        public float MoveSpeed;
        public float3 Direction;
    }
}