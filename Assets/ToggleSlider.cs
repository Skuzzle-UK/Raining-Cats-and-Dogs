using UnityEngine;
using UnityEngine.UI;

public class ToggleSlider : MonoBehaviour
{
    private bool _on = false;
    private Slider _slider;

    public bool On { get { return _on; } }

    public void TurnOn()
    {
        _slider.value = _slider.maxValue;
        _on = true;
    }

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
    }
    public void Toggle()
    {
        _on = !_on;
        if(_on)
        {
            _slider.value = _slider.maxValue;
            return;
        }
        _slider.value = _slider.minValue;
    }
}
