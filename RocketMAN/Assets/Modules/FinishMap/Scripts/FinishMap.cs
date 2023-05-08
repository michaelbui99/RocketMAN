using System;
using GameEvents.Map;
using Modules.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Modules.FinishMap.Scripts
{
    public class FinishMap : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private string nextScene;

        [SerializeField]
        private GameEvent mapFinishedEvent;

        [SerializeField]
        private bool lastMap;

        [SerializeField]
        private GameEvent allMapsCompletedEvent;

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            var activeScene = SceneManager.GetActiveScene();
            mapFinishedEvent.Raise(new MapEventData()
            {
                MapName = activeScene.name
            });

            if (lastMap)
            {
                allMapsCompletedEvent.Raise(GameEvent.NoData());
                SceneManager.LoadScene("End Screen");
                return;
            }

            SceneManager.LoadScene(nextScene);
        }
    }
}