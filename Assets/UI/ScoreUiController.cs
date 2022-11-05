using TMPro;
using UnityEngine;

public class ScoreUiController : MonoBehaviour
{
    TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = $"Score : {GameManager.Instance.Score.ToString()}";
    }
}
