using System;
using System.Collections;
using System.Collections.Generic;
using Modules.Events;
using UnityEngine;

public interface IPausedGameObserver
{
    bool GameIsPaused();
    bool GameIsResumed();

    delegate void Trigger();
    event Trigger OnPauseEvent;
    event Trigger OnResumeEvent;
}

public class PausedGameObserver : MonoBehaviour, IPausedGameObserver
{
    // Start is called before the first frame update
    private GameEventObserver _pauseGameObserver;
    private GameEventObserver _resumeGameObserver;

    private bool _paused = false;

    private void Awake()
    {
        _pauseGameObserver = GameObject.Find("PauseGameObserver").GetComponent<GameEventObserver>();
        _resumeGameObserver = GameObject.Find("ResumeGameObserver").GetComponent<GameEventObserver>();
        
        _pauseGameObserver.RegisterCallback(OnPause);
        _resumeGameObserver.RegisterCallback(OnResume);
    }

    private void OnDestroy()
    {
        _pauseGameObserver.UnregisterCallback(OnPause);
        _resumeGameObserver.UnregisterCallback(OnResume);
    }

    public bool GameIsPaused() => _paused;

    public bool GameIsResumed() => !GameIsPaused();
    public event IPausedGameObserver.Trigger OnPauseEvent;
    public event IPausedGameObserver.Trigger OnResumeEvent;

    private void OnPause(object _)
    {
        _paused = true;
        OnPauseEvent?.Invoke();
    }

    private void OnResume(object _)
    {
        _paused = false;
        OnResumeEvent?.Invoke();
    }
}