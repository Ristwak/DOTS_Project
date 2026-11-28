using Unity.Entities;
using UnityEngine;

public class TowerAuthoring : MonoBehaviour
{
    [Header("Tower Combat")]
    [Min(0.1f)]
    public float Range = 10f;

    [Min(0f)]
    public float Damage = 20f;

    [Tooltip("Attacks per second")]
    [Min(0.1f)]
    public float FireRate = 1f;

    public class Baker : Baker<TowerAuthoring>
    {
        public override void Bake(TowerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new TowerData
            {
                Range = authoring.Range,
                Damage = authoring.Damage,
                FireRate = authoring.FireRate,
                Cooldown = 0f
            });
        }
    }
}