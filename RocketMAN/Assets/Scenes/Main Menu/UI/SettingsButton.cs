using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Main_Menu.UI
{
    public class SettingsButton : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("Scenes/Main Menu/Settings");
        }
    }
}
