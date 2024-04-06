using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameCode
{
    public class InventoryVisualization : MonoBehaviour
    {
        [SerializeField] private DeleteItemWindow deleteItemWindow;
        [SerializeField] private RectTransform slotsParent;
        [SerializeField] private InventoryVisualSlot visualSlotPrefab;
        
        [Inject] private readonly Inventory _inventory;
        
        private List<InventoryVisualSlot> _inventorySlots;

        private void Start()
        {
            _inventory.OnChange += UpdateVisualisation;
            InitBag();
            UpdateVisualisation();
        }
        
        private void InitBag()
        {
            _inventorySlots = new List<InventoryVisualSlot>(_inventory.Size);
            for (int i = 0; i < _inventory.Size; i++)
            {
                var slot = Instantiate(visualSlotPrefab, slotsParent);
                slot.InitSlotIndex(i);
                slot.OnClick += OpenDeleteWindow;
                _inventorySlots.Add(slot);
            }
        }
        
        public void ToggleActive()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
        
        private void UpdateVisualisation()
        {
            for (int i = 0; i < _inventory.Size; i++)
                _inventorySlots[i].SetItem(_inventory.Items[i]);
        }

        private void OpenDeleteWindow(int slotIndex)
        {
            if(!_inventory.Items[slotIndex].SlotIsEmpty)
                deleteItemWindow.OpenWindow(slotIndex);
        }
    }
}