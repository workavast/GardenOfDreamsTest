using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCode.Saves
{
    [Serializable]
    public class InventorySave
    {
        [SerializeField] private List<InventorySaveCell> saveCells;

        public IReadOnlyList<InventorySaveCell> SaveCells => saveCells;
        
        public InventorySave(Inventory inventory)
        {
            saveCells = new List<InventorySaveCell>();
            foreach (var inventorySlot in inventory.Items)
                saveCells.Add(new InventorySaveCell(inventorySlot));
        }
    }

    [Serializable]
    public class InventorySaveCell
    {
        [SerializeField] private bool isEmpty;
        [SerializeField] private ItemId itemId;
        [SerializeField] private int currentCount;

        public bool IsEmpty => isEmpty;
        public ItemId ItemId => itemId;
        public int CurrentCount => currentCount;

        public InventorySaveCell(InventorySlot inventorySlot)
        {
            isEmpty = inventorySlot.SlotIsEmpty;
            
            if (isEmpty)
            {
                itemId = default;
                currentCount = default;
            }
            else
            {
                itemId = inventorySlot.ItemConfig.ItemId;
                currentCount = inventorySlot.CurrentValue;
            }
        }
    }
}