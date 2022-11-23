using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderChange : MonoBehaviour
{
    public enum SoundGroup
    {
        SFX,
        Music
    }

    [SerializeField]
    private AudioClip _audioClip;
    private AudioSource _audioSource;
    [SerializeField]
    private SoundGroup _soundGroup;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _audioSource = GetComponent<AudioSource>();
        if (_soundGroup == SoundGroup.SFX) _slider.value = GameAudio.Instance.SFXVolume;
        if (_soundGroup == SoundGroup.Music) _slider.value = GameAudio.Instance.MusicVolume;
    }

    public void PlayClip()
    {
        if (_soundGroup == SoundGroup.SFX)
        {
            GameAudio.Instance.SetSFXVolume(_slider.value);
            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(_audioClip);
            }
        }
        else
        {
            GameAudio.Instance.SetMusicVolume(_slider.value);
        }
    }
}
