using UnityEngine;
using Zenject;

namespace GameCode
{
    public class ItemsConfigsConverterInstaller : MonoInstaller
    {
        [SerializeField] private ItemsConfig itemsConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<ItemsConfigsConverter>().FromInstance(new ItemsConfigsConverter(itemsConfig)).AsSingle();
        }
    }
}