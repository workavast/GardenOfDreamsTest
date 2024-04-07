using UnityEngine;

namespace GameCode.Core
{
    public interface ITarget
    {
        public Transform AimPoint { get; }
    }
}