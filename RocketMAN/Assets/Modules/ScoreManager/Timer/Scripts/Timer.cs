using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.ScoreManager.Timer.Scripts
{
    public interface ITimer
    {
        void Reset();
        void Start();
        void Stop();
        void CaptureMoment(string label);
        bool TryGetMoment(string label, out KeyValuePair<string, float> moment);
        Dictionary<string, float> GetAllMoments();
    }

    public class Timer : MonoBehaviour, ITimer
    {
        [Header("References")]
        [SerializeField]
        private TimerState timerState;

        [SerializeField]
        private Moments moments;

        private readonly Dictionary<string, float> _internalMoments = new();
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
            _internalMoments.Clear();
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

            if (_internalMoments.ContainsKey(label))
            {
                _internalMoments[label] = moment;
                CommitNewMoments();
                return;
            }

            _internalMoments.Add(label, moment);
            CommitNewMoments();
        }

        public bool TryGetMoment(string label, out KeyValuePair<string, float> moment)
        {
            if (!_internalMoments.TryGetValue(label, out float momentTime))
            {
                moment = default;
                return false;
            }

            moment = KeyValuePair.Create(label, momentTime);
            return true;
        }

        private void CommitNewMoments()
        {
            var newMoments = _internalMoments.Select(m => new Moment()
                {
                    Label = m.Key,
                    Time = m.Value
                })
                .Where(m => !moments.LabelExists(m.Label))
                .ToList();
            moments.AddMoments(newMoments);
        }

        public Dictionary<string, float> GetAllMoments() => moments.ToDictionary();
    }
}