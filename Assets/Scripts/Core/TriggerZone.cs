using System;
using UnityEngine;

namespace GameCode.Core
{
    public class TriggerZone : MonoBehaviour
    {
        public event Action<Collider2D> OnColliderEnter;
        public event Action<Collider2D> OnColliderExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnColliderEnter?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnColliderExit?.Invoke(other);
        }
    }
}
