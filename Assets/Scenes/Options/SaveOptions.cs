using UnityEngine;
using UnityEngine.UI;

public class SaveOptions : MonoBehaviour
{
    [SerializeField]
    private Slider _sfxSlider;
    [SerializeField]
    private Slider _musicSlider;

    public void SaveOptionsToPlayerPrefs()
    {
        PlayerPrefs.SetFloat("sfxVol", _sfxSlider.value);
        PlayerPrefs.SetFloat("musicVol", _musicSlider.value);
        PlayerPrefs.Save();
        LoadingData.LoadScene("MainMenu");
    }

    public void CancelOptions()
    {
        GameAudio.Instance.ResetToPrefs();
        LoadingData.LoadScene("MainMenu");
    }
}
