using System;
using System.Collections.Generic;
using System.Linq;
using GameEvents.Map;
using Modules.Events;
using Modules.ScoreManager.Timer.Scripts;
using Modules.Weapons.WeaponManager.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Modules.ScoreManager.Scripts
{
    public interface IScoreManager
    {
        Dictionary<string, float> GetAllMapTimes();
        Dictionary<string, int> GetAmmoUsagePerMap();
    }

    public class ScoreManager : MonoBehaviour, IScoreManager
    {
        private GameEventObserver _mapFinishedObserver;
        private GameEventObserver _allMapsCompletedObserver;
        private GameEventObserver _weaponStateObserver;
        private GameEventObserver _pauseGameObserver;
        private GameEventObserver _resumeGameObserver;

        private IPausedGameObserver _pausedGameObserver;

        private readonly Dictionary<string, int> _ammoUsagePerMap = new();

        private ITimer _timer;

        private void OnEnable()
        {
            _mapFinishedObserver = GameObject.Find("mapFinishedObserver").GetComponent<GameEventObserver>();
            _allMapsCompletedObserver = GameObject.Find("allMapsCompletedObserver").GetComponent<GameEventObserver>();
            _weaponStateObserver = GameObject.Find("weaponStateObserver").GetComponent<GameEventObserver>();
            _pausedGameObserver = GetComponentInChildren<IPausedGameObserver>();

            _mapFinishedObserver.RegisterCallback(OnMapFinished);
            _allMapsCompletedObserver.RegisterCallback(OnGameFinished);
            _weaponStateObserver.RegisterCallback(OnFireWeaponEvent);
            _pausedGameObserver.OnPauseEvent += PauseTimer;
            _pausedGameObserver.OnResumeEvent += ResumeTimer;
        }

        private void Awake()
        {
            _timer = GetComponent<ITimer>();
            var alreadyInstantiated = GameObject.FindGameObjectsWithTag("Score").Length > 1;

            if (alreadyInstantiated)
            {
                Destroy(gameObject);
            }

            _timer.Reset();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _timer.Start();
        }

        private void OnDisable()
        {
            _mapFinishedObserver.InvokeActionIfNotNull(() =>
                _mapFinishedObserver.UnregisterCallback(OnMapFinished));


            _allMapsCompletedObserver.InvokeActionIfNotNull(() =>
                _allMapsCompletedObserver.UnregisterCallback(OnGameFinished));


            _weaponStateObserver.InvokeActionIfNotNull(() =>
                _weaponStateObserver.UnregisterCallback(OnFireWeaponEvent));

            _pausedGameObserver.OnPauseEvent -= PauseTimer;
            _pausedGameObserver.OnResumeEvent -= ResumeTimer;
        }

        private void OnMapFinished(object mapEventData)
        {
            _timer.CaptureMoment(mapEventData.Cast<MapEventData>().MapName);
        }

        private void OnGameFinished(object data)
        {
            _timer.Stop();
            _timer.CaptureMoment("FINISH_TIME");
        }

        private void OnFireWeaponEvent(object weaponStateEvent)
        {
            if (weaponStateEvent.Cast<WeaponStateEvent>().EventType is not WeaponStateEventType.FireWeapon)
            {
                return;
            }
            
            string currentMap = SceneManager.GetActiveScene().name;
            if (!_ammoUsagePerMap.TryGetValue(currentMap, out int usage))
            {
                usage = 0;
                _ammoUsagePerMap.Add(currentMap, usage);
            }

            _ammoUsagePerMap[currentMap] = usage + 1;
        }

        private void PauseTimer()
        {
            _timer.Stop();
        }

        private void ResumeTimer()
        {
            _timer.Start();
        }

        public Dictionary<string, float> GetAllMapTimes() => _timer.GetAllMoments();

        public Dictionary<string, int> GetAmmoUsagePerMap() => _ammoUsagePerMap;
    }
}