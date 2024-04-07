using UnityEngine;

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
            if (itemConfig == null)
            {
                Debug.LogWarning($"You try set null");
                return;
            }
            
            SlotIsEmpty = false;
            ItemConfig = itemConfig;
            CurrentValue = currentValue;
        }

        public void ChangeCurrentValue(int changeValue)
        {
            if (SlotIsEmpty)
            {
                Debug.LogWarning($"You try add items in empty slot. Info: {CurrentValue} | {changeValue}");
                return;
            }

            if(CurrentValue + changeValue > ItemConfig.StackMaxValue)
                Debug.LogWarning($"You try add more items than stack can have. Info: {ItemConfig} | {CurrentValue} | {changeValue}");
            
            CurrentValue = Mathf.Clamp(CurrentValue + changeValue, 0, ItemConfig.StackMaxValue);
        }

        public void Clean()
        {
            SlotIsEmpty = true;
            ItemConfig = null;
        }
    }
}