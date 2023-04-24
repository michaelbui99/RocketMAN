using System;
using Modules.Events;
using Modules.Events.GameEvents.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.FinishMap.Scripts
{
    public class FinishMap : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private string nextScene;

        [SerializeField]
        private GameEvent mapFinishedEvent;

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

            SceneManager.LoadScene(nextScene);
        }
    }
}