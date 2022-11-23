using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    private int _dotCount = 0;
    private IEnumerator _setDot;
    private bool _quit = false;
    // Start is called before the first frame update
    void Awake()
    {
        _setDot = SetDot();
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        StartCoroutine(_setDot);
    }

    private IEnumerator SetDot()
    {
        while (!_quit)
        {
            yield return new WaitForSeconds(0.5f);
            if (_dotCount < 5)
            {
                _dotCount++;
            }
            else
            {
                _dotCount = 0;
            }

            _textMeshProUGUI.text = "Loading";

            if (_dotCount > 0)
            {
                for (int i = 0; i < _dotCount; i++)
                {
                    _textMeshProUGUI.text += ".";
                }
            }
        }
    }

    private void OnDestroy()
    {
        _quit = true;
    }
}
