using UnityEngine;
using Zenject;

namespace GameCode.Enemies
{
    public class EnemiesFactoryInstaller : MonoInstaller
    {
        [SerializeField] private EnemiesFactory factory;

        public override void InstallBindings()
        {
            Container.Bind<EnemiesFactory>().FromInstance(factory).AsSingle();
        }
    }
}