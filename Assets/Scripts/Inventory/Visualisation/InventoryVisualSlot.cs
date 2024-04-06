using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode
{
    public class InventoryVisualSlot : MonoBehaviour
    {
        [SerializeField] private Image slotItemFill;
        [SerializeField] private TMP_Text itemCount;

        private int _slotIndex;

        /// <summary>
        /// return slot index
        /// </summary>
        public event Action<int> OnClick;
        
        public void InitSlotIndex(int slotIndex)
        {
            _slotIndex = slotIndex;
        }
        
        public void SetItem(InventorySlot inventorySlot)
        {
            if (inventorySlot.SlotIsEmpty)
            {
                slotItemFill.sprite = null;
                itemCount.text = "";
                return;
            }
            
            slotItemFill.sprite = inventorySlot.ItemConfig.Sprite;

            if (inventorySlot.ItemConfig.StackMaxValue <= 1)
                itemCount.text = "";
            else
                itemCount.text = inventorySlot.CurrentValue.ToString();
        }

        public void _Click()
            => OnClick?.Invoke(_slotIndex);
    }
}