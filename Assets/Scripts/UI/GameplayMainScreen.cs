using System;
using GameCode.Core;
using UnityEngine;

namespace GameCode
{
    public class GameplayMainScreen : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Joystick joystick;
        [SerializeField] private InventoryVisualization inventoryVisualization;
        
        private void Awake()
        {
            joystick.OnInputEnter += InputEnter;
            joystick.OnInputExit += InputExit;
        }
        
        private void InputEnter()
        {
            var direction = Vector3.up * joystick.Vertical + Vector3.right * joystick.Horizontal;
            player.SetInput(direction);
        }
        
        private void InputExit()
        {
            player.ResetInput();
        }

        
        public void _PlayerAttack(bool attack)
        {
            player.SetAttack(attack);
        }

        public void _ToggleActiveInventory()
            => inventoryVisualization.ToggleActive();
    }
}
