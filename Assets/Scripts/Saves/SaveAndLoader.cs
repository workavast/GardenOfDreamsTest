using System.IO;
using GameCode.Core;
using UnityEngine;

namespace GameCode.Saves
{
    public class SaveAndLoader
    {
        private static SaveAndLoader _instance;
        public static SaveAndLoader Instance => _instance ??= new SaveAndLoader();

        public bool Loaded { get; private set; }

        private const string SaveFileName = "Save";
        
        private static string SavePath => Path.Combine(Application.dataPath, SaveFileName);
        
        private SaveHolder _saveHolder = new SaveHolder();

        public IReadOnlySaveHolder SaveHolder => _saveHolder;
        
        public void UpdateInventoryData(Inventory inventory)
        {
            var newInventorySave = new InventorySave(inventory);
            _saveHolder.inventorySave = newInventorySave;
        }
        
        public void UpdatePlayerData(Player player)
        {
            var newPlayerSave = new PlayerSave(player);
            _saveHolder.playerSave = newPlayerSave;
        }
        
        public void Save()
        {
            BinarySerializer.Save(SavePath, _saveHolder);   
        }

        public void TryLoad()
        {
            if (File.Exists(SavePath))
            {
                _saveHolder = BinarySerializer.Load<SaveHolder>(SavePath);
                Loaded = true;
            }
        }

        public void DeleteSave()
        {
            File.Delete(SavePath);
        }
    }
}