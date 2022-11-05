using TMPro;
using UnityEngine;

public class TimerUiController : MonoBehaviour
{
    TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = $"{GameManager.Instance.TimeRemaining.ToString()} s : Time";
    }
}
