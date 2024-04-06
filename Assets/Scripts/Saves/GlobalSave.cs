using System;

namespace GameCode.Saves
{
    [Serializable]
    public class GlobalSave : IReadOnlyGlobalSave
    {
        public PlayerSave playerSave;
        public InventorySave inventorySave;
        
        public PlayerSave PlayerSave => playerSave;
        public InventorySave InventorySave => inventorySave;
    }
}