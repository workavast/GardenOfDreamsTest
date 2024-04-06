namespace GameCode
{
    public class InventorySlot
    {
        public ItemConfigBase ItemConfig { get; private set; }
        public int CurrentValue { get; private set; }

        public bool HaveFreeSpace =>  CurrentValue < ItemConfig.StackMaxValue;
        public int FreeSpaceCount =>  ItemConfig.StackMaxValue - CurrentValue;
        public bool SlotIsEmpty { get; private set; } = true;
        
        public void SetItem(ItemConfigBase itemConfig, int currentValue)
        {
            if(itemConfig == null)
                return;
            
            SlotIsEmpty = false;
            ItemConfig = itemConfig;
            CurrentValue = currentValue;
        }

        public void ChangeCurrentValue(int changeValue)
        {
            CurrentValue += changeValue;
        }

        public void Clean()
        {
            SlotIsEmpty = true;
            ItemConfig = null;
        }
    }
}