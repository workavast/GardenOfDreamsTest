using System;

namespace GameCode.Saves
{
    [Serializable]
    public class GlobalSave
    {
        public PlayerSave playerSave;
        public InventorySave inventorySave;
    }
}