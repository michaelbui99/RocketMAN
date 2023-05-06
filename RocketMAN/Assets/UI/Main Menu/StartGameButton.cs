using Modules.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.Main_Menu
{
    public class StartGameButton : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("Apprentice 1");
        }
    }
}
