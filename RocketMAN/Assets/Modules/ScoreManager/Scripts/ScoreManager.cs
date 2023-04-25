using System;
using Modules.Events;
using Modules.Events.GameEvents.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.ScoreManager.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float scorePenaltyPerSecond = 1f;

        [SerializeField]
        private float BaseFinishScore = 1000;

        private GameEventObserver _mapStartedObserver;
        private GameEventObserver _mapFinishedObserver;
        private GameEventObserver _allMapsFinishedObserver;

        private Timer.Scripts.Timer _timer;

        private void OnEnable()
        {
            _timer = GetComponent<Timer.Scripts.Timer>();

            var mapStartedObserverObject = GameObject.Find("mapStartedObserver");
            var mapFinishedObserverObject = GameObject.Find("mapFinishedObserver");
            var allMapsFinishedObserverObject = GameObject.Find("allMapsFinishedObserver");

            _mapStartedObserver = mapStartedObserverObject.GetComponent<GameEventObserver>();
            _mapFinishedObserver = mapFinishedObserverObject.GetComponent<GameEventObserver>();
            _allMapsFinishedObserver = allMapsFinishedObserverObject.GetComponent<GameEventObserver>();

            _mapFinishedObserver.RegisterCallback(OnMapFinished);
            _allMapsFinishedObserver.RegisterCallback(OnGameFinished);
        }

        private void Awake()
        {
            var alreadyInstantiated = GameObject.FindGameObjectsWithTag("Score").Length > 1;

            if (alreadyInstantiated)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _timer.Start();
        }

        private void OnDisable()
        {
            if (_mapFinishedObserver != null)
            {
                _mapFinishedObserver.UnregisterCallback(OnMapFinished);
            }

            if (_allMapsFinishedObserver != null)
            {
                _allMapsFinishedObserver.UnregisterCallback(OnGameFinished);
            }
        }

        private void OnMapFinished(object mapEventData)
        {
            _timer.CaptureMoment(((MapEventData) mapEventData).MapName);
        }

        private void OnGameFinished(object data)
        {
            _timer.Stop();
        }
    }
}