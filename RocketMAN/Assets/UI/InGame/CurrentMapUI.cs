using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.InGame
{
    public class CurrentMapUI : MonoBehaviour
    {
        private TMP_Text _currentMap;

        private void Awake()
        {
            _currentMap = GetComponent<TMP_Text>();
            _currentMap.text = SceneManager.GetActiveScene().name;
        }
    }
}