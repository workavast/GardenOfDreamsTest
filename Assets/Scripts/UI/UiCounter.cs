using SomeStorages;
using TMPro;

namespace GameCode.UI
{
    public class UiCounter
    {
        private readonly TMP_Text _counterText;
        private readonly IReadOnlySomeStorage<int> _counter;
        
        public UiCounter(TMP_Text counterText, IReadOnlySomeStorage<int> counter)
        {
            _counterText = counterText;
            _counter = counter;

            _counter.OnChange += UpdateCounter;
        }

        private void UpdateCounter()
        {
            _counterText.text = _counter.CurrentValue + "/" + _counter.MaxValue;
        }
    }
}