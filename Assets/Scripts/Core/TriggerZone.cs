using System;
using UnityEngine;

namespace GameCode.Core
{
    public class TriggerZone : MonoBehaviour
    {
        public event Action<Collider2D> OnEnter;
        public event Action<Collider2D> OnExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnEnter?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnExit?.Invoke(other);
        }
    }
}
