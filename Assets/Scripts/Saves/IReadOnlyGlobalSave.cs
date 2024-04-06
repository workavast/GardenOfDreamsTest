namespace GameCode.Saves
{
    public interface IReadOnlyGlobalSave
    {
        public PlayerSave PlayerSave { get; }
        public InventorySave InventorySave { get; }
    }
}