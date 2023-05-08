using System;
using Modules.Events;
using UnityEngine;
using Utility;

namespace Modules.GameSettingsScreen.Scripts
{
    public class InGameSettings : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Canvas playerCanvas;

        [SerializeField]
        private Canvas settingsCanvas;

        private IPausedGameObserver _pausedGameObserver;
        private bool _paused = false;

        private void Start()
        {
            _pausedGameObserver = GetComponentInChildren<IPausedGameObserver>();

            _pausedGameObserver.OnPauseEvent += OnPauseGame;
            _pausedGameObserver.OnResumeEvent += OnResumeGame;
        }

        private void OnDestroy()
        {
            _pausedGameObserver.InvokeActionIfNotNull(() => _pausedGameObserver.OnPauseEvent -= OnPauseGame);
            _pausedGameObserver.InvokeActionIfNotNull(() => _pausedGameObserver.OnResumeEvent -= OnResumeGame);
        }

        private void OnPauseGame()
        {
            _paused = true;
            playerCanvas.gameObject.SetActive(false);
            settingsCanvas.gameObject.SetActive(true);
        }

        private void OnResumeGame()
        {
            _paused = false;
            playerCanvas.gameObject.SetActive(true);
            settingsCanvas.gameObject.SetActive(false);
        }
    }
}