using System;
using GameCode.Core;
using UnityEngine;

namespace GameCode.Saves
{
    [Serializable]
    public class PlayerSave
    {
        [SerializeField] private float healthPointsFillingPercentage;

        public float HealthPointsFillingPercentage => healthPointsFillingPercentage;

        public PlayerSave(Player player)
        {
            healthPointsFillingPercentage = player.HealthPoints.FillingPercentage;
        }
    }
}