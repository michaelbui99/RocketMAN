using System;
using Modules.Timer.Scripts;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerUI: MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TimerState gameTimer;

        private TMP_Text _timerDisplay;


        private void Awake()
        {
            _timerDisplay = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _timerDisplay.text = GetTimeFormatted();
        }

        private string GetTimeFormatted()
        {
            var time = TimeSpan.FromSeconds(gameTimer.Time);
            return time.ToString(@"mm\:ss\:fff");
        }
    }
}