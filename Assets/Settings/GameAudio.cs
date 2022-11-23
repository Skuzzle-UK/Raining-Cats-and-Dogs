using UnityEngine;
using UnityEngine.Audio;

public class GameAudio : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    private float _musicVol = 8f;
    private float _sfxVol = 0f;

    public float MusicVolume { get { return _musicVol; } }
    public float SFXVolume { get { return _sfxVol; } }


    public static GameAudio Instance;

    private void Awake()
    {
        // If Instance is not null (any time after the first time)
        // AND
        // If Instance is not 'this' (after the first time)
        if (Instance != null && Instance != this)
        {
            // ...then destroy the game object this script component is attached to.
            Destroy(gameObject);
        }
        else
        {
            // Tell Unity not to destory the GameObject this
            //  is attached to between scenes.
            DontDestroyOnLoad(gameObject);
            // Save an internal reference to the first instance of this class
            Instance = this;
        }

        ResetToPrefs();
    }

    public void SetSFXVolume(float level)
    {
        //@TODO check vol level is legit
        Instance._audioMixer.SetFloat("sfxVol", level);
    }

    public void SetMusicVolume(float level)
    {
        //@TODO check vol level is legit
        Instance._audioMixer.SetFloat("musicVol", level);
    }

    public void MuteSFX()
    {
        Instance._audioMixer.SetFloat("sfxVol", -80f);
    }
    public void UnMuteSFX()
    {
        Instance._audioMixer.SetFloat("sfxVol", Instance._sfxVol);
    }

    public void ResetToPrefs()
    {
        Instance._sfxVol = PlayerPrefs.GetFloat("sfxVol", 0f);
        Instance._musicVol = PlayerPrefs.GetFloat("musicVol", 8f);
        SetMusicVolume(Instance._musicVol);
        SetSFXVolume(Instance._sfxVol);
    }
}
