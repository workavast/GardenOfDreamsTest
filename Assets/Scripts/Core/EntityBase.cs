using SomeStorages;
using UnityEngine;

namespace GameCode.Core
{
    public abstract class EntityBase : MonoBehaviour, IDamageable
    {
        [SerializeField] protected float healthPoints;
        [SerializeField] protected float moveSpeed;

        protected FloatStorage _healthPoints;

        public IReadOnlySomeStorage<float> HealthPoints => _healthPoints;
        
        public abstract void TakeDamage(float damage);
    }
}