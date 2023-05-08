using System;
using System.Linq;
using System.Text;
using Modules.ScoreManager.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

namespace Scenes.End_Screen.UI
{
    public class DisplayResults : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject scoreManager;

        [SerializeField]
        private TMP_Text resultsDisplay;


        private IScoreManager _scoreManager;

        private void Awake()
        {
            _scoreManager = scoreManager.GetComponent<IScoreManager>();
        }

        private void Update()
        {
            SetResults();
        }

        void Start()
        {
            SetResults();
            _scoreManager.GetAllMapTimes().Keys.ToList().ForEach(Debug.Log);
        }

        private void SetResults()
        {
            var mapTimes = _scoreManager.GetAllMapTimes();
            var ammoUsages = _scoreManager.GetAmmoUsagePerMap();

            var results = mapTimes.Keys
                .Select(k => new Result(k, mapTimes[k], ammoUsages.ContainsKey(k) ? ammoUsages[k] : 0))
                .ToList();

            StringBuilder display = new();
            results.ForEach(r =>
            {
                display.AppendLine(
                    $"{r.map} - {r.finishTime.ToString("0.00")} - {ConvertToScore(r.finishTime, r.ammoUsage)}");
            });

            resultsDisplay.text = display.ToString();
        }

        private float ConvertToScore(float time, int usage)
        {
            return (time * usage) / 10;
        }

        private record Result(string map, float finishTime, int ammoUsage)
        {
            public float finishTime { get; } = finishTime;
            public string map { get; } = map;
            public int ammoUsage { get; } = ammoUsage;
        }
    }
}