using System;
using Modules.Events;
using Modules.Odm.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.InGame
{
    public class GasUI : MonoBehaviour
    {
        private GameEventObserver _gasStateObserver;
        private Slider _gasSlider;
        private TMP_Text _gasLiteral;

        private GasStateEvent _currentKnownGasState = new()
        {
            CurrentLevel = 1,
            ExpenditureRate = 1,
            MaxCapacity = 1
        };

        private void Awake()
        {
            _gasStateObserver = GetComponent<GameEventObserver>();
            _gasSlider = GetComponent<Slider>();

            _gasStateObserver.RegisterCallback(UpdateGasState);
        }

        private void Update()
        {
            _gasSlider.maxValue = _currentKnownGasState.MaxCapacity;
            _gasSlider.minValue = 0;
            _gasSlider.value = _currentKnownGasState.CurrentLevel;
        }

        private void OnDestroy()
        {
            _gasStateObserver.UnregisterCallback(UpdateGasState);
        }

        private void UpdateGasState(object newState)
        {
            _currentKnownGasState = newState.Cast<GasStateEvent>();
        }
    }
}