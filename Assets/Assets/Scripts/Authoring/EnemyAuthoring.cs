using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    [Header("Enemy Settings")]
    [Min(1f)]
    public float MaxHealth = 100f;

    [Min(0f)]
    public float MoveSpeed = 3f;

    [Min(0)]
    public int Reward = 10;

    public class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<EnemyTag>(entity);

            AddComponent(entity, new EnemyHealth
            {
                Current = authoring.MaxHealth,
                Max = authoring.MaxHealth,
                Reward = authoring.Reward
            });

            AddComponent(entity, new EnemyMoveSpeed
            {
                Value = authoring.MoveSpeed
            });

            AddComponent(entity, new EnemyPathProgress
            {
                Index = 0
            });
        }
    }
}