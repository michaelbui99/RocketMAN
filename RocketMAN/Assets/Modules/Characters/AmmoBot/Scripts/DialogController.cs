using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Characters.AmmoBot.Scripts
{
    public class DialogController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TMP_Text text;

        [Header("Settings")]
        [SerializeField]
        private float delayPerCharacter;

        [SerializeField]
        private string defaultDialog;

        private readonly Queue<IEnumerator> _routineQueue = new();


        private void Start()
        {
            StartCoroutine(ManageRoutines());
            Write(defaultDialog);
        }

        public void Write(string dialog)
        {
            _routineQueue.Enqueue(InternalWrite(dialog));
            _routineQueue.Enqueue(InternalWrite(defaultDialog));
        }

        private IEnumerator ManageRoutines()
        {
            while (true)
            {
                while (_routineQueue.Any())
                {
                    yield return StartCoroutine(_routineQueue.Dequeue());
                }

                yield return null;
            }
        }

        private IEnumerator InternalWrite(string dialog)
        {
            StringBuilder s = new();
            int index = 0;
            while (s.ToString() != dialog)
            {
                s.Append(dialog[index]);
                text.text = s.ToString();
                index++;
                yield return new WaitForSeconds(delayPerCharacter);
            }

            yield return new WaitForSeconds(5);
        }
    }
}