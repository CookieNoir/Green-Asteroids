using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Asteroids.UI
{
    public class ObstaclesCounter : MonoBehaviour
    {
        [SerializeField] private string _outputFormat;
        [SerializeField] private TMP_Text _countField;
        public UnityEvent OnValueChanged;

        public void SetCountField(int current, int max)
        {
            _countField.text = string.Format(_outputFormat, current, max);
            OnValueChanged.Invoke();
        }
    }
}