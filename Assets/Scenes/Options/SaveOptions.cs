using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SaveOptions : MonoBehaviour
{
    [SerializeField]
    private Slider _sfxSlider;
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    private InputActionAsset actions;


    public void Start()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);
    }


    public void SaveOptionsToPlayerPrefs()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);

        PlayerPrefs.SetFloat("sfxVol", _sfxSlider.value);
        PlayerPrefs.SetFloat("musicVol", _musicSlider.value);
        PlayerPrefs.Save();
        LoadingData.LoadScene("MainMenu");
    }

    public void CancelOptions()
    {
        try
        {
            var rebinds = PlayerPrefs.GetString("rebinds");
            if (!string.IsNullOrEmpty(rebinds))
                actions.LoadBindingOverridesFromJson(rebinds);
        }
        catch { }

        GameAudio.Instance.ResetToPrefs();
        LoadingData.LoadScene("MainMenu");
    }
}
