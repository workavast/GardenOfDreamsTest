using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCode
{
    public class DeleteItemWindow : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemCount;

        [Inject] private readonly Inventory _inventory;
        
        private int _slotIndex;
        
        public void OpenWindow(int slotIndex)
        {
            _slotIndex = slotIndex;
            var inventorySlot = _inventory.Items[slotIndex];
            itemImage.sprite = inventorySlot.ItemConfig.Sprite;
            
            if (inventorySlot.ItemConfig.StackMaxValue <= 1)
                itemCount.text = "";
            else
                itemCount.text = inventorySlot.CurrentValue.ToString();
            
            gameObject.SetActive(true);
        }
        
        public void _DeleteItem()
        {
            _inventory.CleanSlot(_slotIndex);
            _CloseWindow();
        }
        
        public void _CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}