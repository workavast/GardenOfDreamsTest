using GameCode.UI;
using UnityEngine;
using Zenject;

namespace GameCode.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameplayMainScreen gameplayMainScreen;
        [SerializeField] private GameplayPlayerDeathScreen gameplayPlayerDeathScreen;

        [Inject] private Player _player;

        private void Awake()
        {
            Time.timeScale = 1;
            _player.OnDeath += OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            //need create something like GameStateSystem and change game state from gameplay to other. 
            Time.timeScale = 0;
            
            gameplayMainScreen.gameObject.SetActive(false);
            gameplayPlayerDeathScreen.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }
    }
}