using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCode.UI
{
    public class ScreenBase : MonoBehaviour
    {
        public void _LoadScene(int sceneIndex)
            => SceneManager.LoadScene(sceneIndex);
        
        public void _Quit()
            => Application.Quit();
    }
}