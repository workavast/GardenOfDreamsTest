using UnityEngine;

namespace GameCode.CollectableItems
{
    public class CollectableItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public ItemConfigBase ItemConfig { get; private set; }
        public int ItemCount { get; private set; }
        
        public void Init(ItemConfigBase newItemConfig, int itemCount)
        {
            ItemConfig = newItemConfig;
            ItemCount = itemCount;
            spriteRenderer.sprite = newItemConfig.Sprite;
        }

        public ItemConfigBase PickUp(int itemCount)
        {
            ItemCount -= itemCount;

            if (ItemCount <= 0)
                Destroy(gameObject);//need use pooling
            
            Debug.Log("PickUped item");
            return ItemConfig;
        }
    }
}