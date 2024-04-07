using GameCode.Core;
using UnityEngine;
using Zenject;

namespace GameCode.CameraMovement
{
    public class CameraMovement : MonoBehaviour
    {
        [Inject] private readonly Player _player;

        private void Update()
        {
            Vector3 newPosition = (Vector2)_player.transform.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }
}