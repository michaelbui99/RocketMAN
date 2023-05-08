using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Main_Menu.UI
{
    public class StartGameButton : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("Apprentice 1");
        }
    }
}
