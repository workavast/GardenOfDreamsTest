using SomeStorages;
using UnityEngine;

namespace GameCode.Core
{
    public class EntityBase : MonoBehaviour
    {
        [SerializeField] protected float healthPoints;
        [SerializeField] protected float moveSpeed;

        protected FloatStorage _healthPoints;

        public IReadOnlySomeStorage<float> HealthPoints => _healthPoints;
    }
}