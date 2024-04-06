using SomeStorages;

namespace GameCode.Core
{
    public interface IReadOnlyAttackProcessor
    {
        public IReadOnlySomeStorage<int> MagazineCounter { get; }
    }
}