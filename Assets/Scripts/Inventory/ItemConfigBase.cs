using UnityEngine;

namespace GameCode
{
    public abstract class ItemConfigBase : ScriptableObject
    {
        [field: SerializeField] public ItemId ItemId { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] [field: Range(1, 32)] public int StackMaxValue { get; private set; } = 1;
    }
}