using System.Collections.Generic;
using UnityEngine;

namespace GameCode
{
    [CreateAssetMenu(fileName = nameof(ItemsConfig), menuName = "Configs/" + nameof(ItemsConfig))]
    public class ItemsConfig : ScriptableObject
    {
        [SerializeField] private List<ItemConfigBase> items;
        
        public IReadOnlyList<ItemConfigBase> Items => items;
    }
}