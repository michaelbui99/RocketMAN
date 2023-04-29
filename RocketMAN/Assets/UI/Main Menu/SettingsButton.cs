using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main_Menu
{
    public class SettingsButton : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("Scenes/Main Menu/Settings");
        }
    }
}
