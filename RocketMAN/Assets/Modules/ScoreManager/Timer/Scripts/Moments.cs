using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.ScoreManager.Timer.Scripts
{
    [CreateAssetMenu(menuName = "Moments", fileName = "Moments")]
    public class Moments : ScriptableObject
    {
        public List<Moment> allMoments = new();

        public void AddMoment(Moment moment)
        {
            allMoments.Add(moment);
        }

        public void AddMoments(IEnumerable<Moment> moments)
        {
            allMoments.AddRange(moments);
        }

        public bool LabelExists(string label)
        {
            return allMoments.Any(m => m.Label == label);
        }

        public Dictionary<string, float> ToDictionary()
        {
            Dictionary<string, float> momentsDict = new();
            allMoments.ForEach(m => momentsDict.Add(m.Label, m.Time));

            return momentsDict;
        }
    }
}