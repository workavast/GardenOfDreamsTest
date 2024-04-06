using Zenject;

namespace GameCode
{
    public class InventoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Inventory>().FromInstance(new Inventory()).AsSingle();
        }
    }
}