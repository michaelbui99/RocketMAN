using Modules.GameSettings.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu
{
    public class MusicSlider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TMP_Text value;

        [SerializeField]
        private Slider slider;

        private GameSettingsIO _gameSettingsIO;

        private void Awake()
        {
            _gameSettingsIO = GetComponent<GameSettingsIO>();
            slider.value = _gameSettingsIO.AudioSettings.MusicVolume;
        }

        void Update()
        {
            value.text = $"{slider.value.ToString("0.00")}";
            // NOTE: (mibui 2023-05-05) Snippet from here https://johnleonardfrench.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
            _gameSettingsIO.AudioSettings.MusicVolume = Mathf.Log10(slider.value) * 20;
        }
    }
}