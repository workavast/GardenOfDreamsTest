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
}