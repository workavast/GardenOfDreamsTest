using GameCode.Core;
using TMPro;
using UnityEngine;

namespace GameCode.UI
{
    public class GameplayMainScreen : ScreenBase
    {
        [SerializeField] private Player player;
        [SerializeField] private Joystick joystick;
        [SerializeField] private InventoryVisualization inventoryVisualization;
        [SerializeField] private TMP_Text magazineCounterText;
        
        private Counter _magazineCounter;
        
        private void Awake()
        {
            joystick.OnInputEnter += InputEnter;
            joystick.OnInputExit += InputExit;
        }

        private void Start()
        {
            _magazineCounter = new Counter(magazineCounterText, player.AttackProcessor.MagazineCounter);
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
