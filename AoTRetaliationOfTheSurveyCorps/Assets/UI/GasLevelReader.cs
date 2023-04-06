using OdmGear.Gas;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GasLevelReader : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("References")]
        [SerializeField]
        private GasController gasController;

        [SerializeField]
        private TMP_Text text;

        private void Update()
        {
            text.text = FormatGasLevel(gasController.GetCurrentGasLevel(), gasController.GetMaxGasCapacity());
        }

        private string FormatGasLevel(float current, float maxCapacity)
        {
            return $"Gas: {current:0.00}/{maxCapacity}";
        }
    }
}