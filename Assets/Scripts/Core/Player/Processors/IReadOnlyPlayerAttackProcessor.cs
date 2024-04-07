using SomeStorages;

namespace GameCode.Core
{
    public interface IReadOnlyPlayerAttackProcessor
    {
        public IReadOnlySomeStorage<int> MagazineCounter { get; }
    }
}