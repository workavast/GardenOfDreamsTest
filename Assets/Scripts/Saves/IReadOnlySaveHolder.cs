namespace GameCode.Saves
{
    public interface IReadOnlySaveHolder
    {
        public PlayerSave PlayerSave { get; }
        public InventorySave InventorySave { get; }
    }
}