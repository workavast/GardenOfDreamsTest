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
        
        private GlobalSave _globalSave = new GlobalSave();

        public IReadOnlyGlobalSave GlobalSave => _globalSave;
        
        private SaveAndLoader()
        {
            TryLoad();
            Debug.Log($"SaveAndLoader Constructor");
        }
        
        public void UpdateInventoryData(Inventory inventory)
        {
            Debug.Log($"SaveAndLoader Constructor");
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

        public void TryLoad()
        {
            Debug.Log($"SaveAndLoader try load");
            if (File.Exists(SavePath))
            {
                Debug.Log($"SaveAndLoader loaded save");
                _globalSave = BinarySerializer.Load<GlobalSave>(SavePath);
                Loaded = true;
            }
        }
    }
}