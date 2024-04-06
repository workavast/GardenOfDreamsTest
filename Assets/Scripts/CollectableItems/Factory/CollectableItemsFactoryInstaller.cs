using UnityEngine;
using Zenject;

namespace GameCode.CollectableItems.Factory
{
    public class CollectableItemsFactoryInstaller : MonoInstaller
    {
        [SerializeField] private CollectableItemsFactory factory;

        public override void InstallBindings()
        {
            Container.Bind<CollectableItemsFactory>().FromInstance(factory).AsSingle();
        }
    }
}