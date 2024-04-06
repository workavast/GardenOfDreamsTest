using System;
using GameCode.Core;

namespace GameCode.Saves
{
    [Serializable]
    public class PlayerSave
    {
        public float healthPintsFillingPercentage;

        public PlayerSave(Player player)
        {
            healthPintsFillingPercentage = player.HealthPoints.FillingPercentage;
        }
    }
}