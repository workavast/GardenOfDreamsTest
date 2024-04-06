using System;
using System.Collections.Generic;

namespace GameCode.Saves
{
    [Serializable]
    public class InventorySave
    {
        public List<InventorySaveCell> saveCells;

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
        public bool isEmpty;
        public ItemId itemId;
        public int currentCount;

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