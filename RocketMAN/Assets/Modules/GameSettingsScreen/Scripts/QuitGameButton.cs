using UnityEngine;

namespace Scenes.Main_Menu.UI
{
    public class QuitGameButton : MonoBehaviour
    {
        public void OnQuitGame()
        {
            Application.Quit();
        }
    }
}