using System;
using Modules.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Scenes.Main_Menu.UI
{
    public class OkButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameEvent resumeGameEvent;

        [Header("Settings")]
        [SerializeField]
        private GameContext context;


        public void OnClick()
        {
            Action action = context switch
            {
                GameContext.InGame => () => resumeGameEvent.Raise(GameEvent.NoData()),
                GameContext.MainMenu => () => SceneManager.LoadScene("MainMenu"),
                _ => SwitchUtil.Unreachable
            };
            
            action.Invoke();
        }

        private enum GameContext
        {
            InGame,
            MainMenu
        }
    }
}