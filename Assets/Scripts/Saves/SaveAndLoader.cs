using System.IO;
using GameCode.Core;
using UnityEngine;

namespace GameCode.Saves
{
    public class SaveAndLoader
    {
        private const string SaveFileName = "Save";
        
        private static string SavePath => Path.Combine(Application.dataPath, SaveFileName);
        
        private GlobalSave _globalSave;
        
        public void UpdateInventoryData(Inventory inventory)
        {
            var newInventorySave = new InventorySave(inventory);
            _globalSave.inventorySave = newInventorySave;
        }
        
        public void UpdatePlayerData(Player player)
        {
            var newPlayerSave = new PlayerSave(player);
            _globalSave.playerSave = newPlayerSave;
        }
        
        public void Save()
        {
            BinarySerializer.Save(SavePath, _globalSave);   
        }

        public void Load()
        {
            _globalSave = BinarySerializer.Load<GlobalSave>(SavePath);
        }
    }
}