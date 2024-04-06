using System.Collections.Generic;
using GameCode.Core;
using UnityEngine;
using Zenject;

namespace GameCode.Saves
{
    public class SceneSaveAndLoader : MonoBehaviour
    {
        [SerializeField] private Player _player;
        
        [Inject] private readonly Inventory _inventory;
        [Inject] private readonly ItemsConfigsConverter _itemsConfigsConverter;
        
        private void Start()
        {
            if (SaveAndLoader.Instance.Loaded)
            {
                _player.LoadSave(SaveAndLoader.Instance.GlobalSave.PlayerSave);
                _inventory.LoadSave(ConvertInventorySave(SaveAndLoader.Instance.GlobalSave.InventorySave));
            }
            else
            {
                SaveGame();
            }
            
            _inventory.OnChange += SaveGame;
            _player.HealthPoints.OnChange += SaveGame;
            _player.OnDeath += ResetInventory;
        }

        private IReadOnlyList<InventorySlot> ConvertInventorySave(InventorySave inventorySave)
        {
            List<InventorySlot> inventorySlots = new List<InventorySlot>(inventorySave.SaveCells.Count);
            foreach (var saveCell in inventorySave.SaveCells)
            {
                var newSlot = new InventorySlot();
                inventorySlots.Add(newSlot);
                if(!saveCell.IsEmpty)
                    newSlot.SetItem(_itemsConfigsConverter.ItemConfigs[saveCell.ItemId], saveCell.CurrentCount);
            }

            return inventorySlots;
        }

        private void SaveGame()
        {
            SaveAndLoader.Instance.UpdatePlayerData(_player);
            SaveAndLoader.Instance.UpdateInventoryData(_inventory);
            SaveAndLoader.Instance.Save();
        }

        private void ResetInventory()
        {
            foreach (var inventorySlot in _inventory.Items)
                inventorySlot.Clean();
            
            SaveAndLoader.Instance.UpdateInventoryData(_inventory);
        }
    }
}