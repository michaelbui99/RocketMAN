using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.ScoreManager.Timer.Scripts
{
    public class Timer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TimerState timerState;

        private readonly Dictionary<string, float> _moments = new();
        private bool _active;

        private void Update()
        {
            if (!_active)
            {
                return;
            }

            timerState.Time += Time.deltaTime;
        }


        public void Reset()
        {
            Stop();
            timerState.Time = 0;
        }

        public void Start()
        {
            _active = true;
        }

        public void Stop()
        {
            _active = false;
        }

        public void CaptureMoment(string label)
        {
            float moment = timerState.Time;

            if (_moments.ContainsKey(label))
            {
                _moments[label] = moment;
                return;
            }

            _moments.Add(label, moment);
        }

        public bool TryGetMoment(string label, out KeyValuePair<string, float> moment)
        {
            if (!_moments.TryGetValue(label, out float momentTime))
            {
                moment = default;
                return false;
            }

            moment = KeyValuePair.Create(label, momentTime);
            return true;
        }

        public IEnumerable<KeyValuePair<string, float>> GetAllMoments()
        {
            return _moments.Keys
                .Select(label => KeyValuePair.Create(label, _moments[label]))
                .ToList();
        }
    }
}