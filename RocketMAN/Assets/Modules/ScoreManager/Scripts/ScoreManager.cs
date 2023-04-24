using System;
using Modules.Events;
using Modules.Events.GameEvents.Map;
using UnityEngine;

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
            
            _mapStartedObserver.RegisterCallback(OnMapStarted);
            _mapFinishedObserver.RegisterCallback(OnMapFinished);
            _allMapsFinishedObserver.RegisterCallback(OnGameFinished);
            
            OnMapStarted(new MapEventData{MapName = "TEST"});
        }

        private void OnDisable()
        {
            _mapStartedObserver.UnregisterCallback(OnMapStarted);
            _mapFinishedObserver.UnregisterCallback(OnMapFinished);
            _allMapsFinishedObserver.UnregisterCallback(OnGameFinished);
        }

        private void OnMapStarted(object mapEventData)
        {
            _timer.Reset();
            _timer.Start();
        }

        private void OnMapFinished(object mapEventData)
        {
            _timer.Stop();
            _timer.CaptureMoment(((MapEventData) mapEventData).MapName);
        }

        private void OnGameFinished(object data)
        {
            throw new NotImplementedException();
        }
    }
}