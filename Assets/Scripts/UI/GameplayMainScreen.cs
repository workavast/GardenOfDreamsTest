using GameCode.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCode.UI
{
    public class GameplayMainScreen : ScreenBase
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private InventoryVisualization inventoryVisualization;
        [SerializeField] private TMP_Text magazineCounterText;

        [Inject] private readonly Player _player;

        private UiCounter _magazineCounter;
        
        private void Awake()
        {
            joystick.OnInputEnter += InputEnter;
            joystick.OnInputExit += InputExit;
        }

        private void Start()
        {
            _magazineCounter = new UiCounter(magazineCounterText, _player.AttackProcessor.MagazineCounter);
        }

        private void InputEnter()
        {
            var direction = Vector3.up * joystick.Vertical + Vector3.right * joystick.Horizontal;
            _player.SetInput(direction);
        }
        
        private void InputExit()
        {
            _player.ResetInput();
        }
        
        public void _PlayerAttack(bool attack)
        {
            _player.SetAttack(attack);
        }

        public void _ToggleActiveInventory()
            => inventoryVisualization.ToggleActive();
    }
}
