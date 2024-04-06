using GameCode.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private EntityBase entityBase;
        [SerializeField] private Slider healthSlider;
        
        private void Start()
        {
            entityBase.HealthPoints.OnChange += UpdateHealthBar;
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            healthSlider.value = entityBase.HealthPoints.FillingPercentage;
        }
    }
}