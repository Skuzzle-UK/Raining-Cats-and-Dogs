using UnityEngine;
using UnityEngine.UI;

public class WeaponChargeUiController : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetValue(float val)
    {
        slider.value = val;
    }

    public void SetRange(float min, float max)
    {
        slider.minValue = min;
        slider.maxValue = max;
    }
}
