using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Dino.Utility.Audio;
using Dino.Utility.Audio;
using UnityEngine.Rendering;
using AudioType = Dino.Utility.Audio.AudioType;

public class SettingsUI : UIWindow
{
    [Header("SettingsUI")]
    [SerializeField] private Slider _volumeSFXSlider;
    [SerializeField] private Slider _volumeMusicSlider;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";


    public override void Initialized()
    {
        base.Initialized();

        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

        _volumeMusicSlider.onValueChanged.RemoveListener(OnVolumeMusicChanges);
        _volumeSFXSlider.onValueChanged.RemoveListener(OnVolumeSFXChanged);

        _volumeMusicSlider.SetValueWithoutNotify(musicVolume);
        _volumeSFXSlider.SetValueWithoutNotify(sfxVolume);

        AudioManager.Instance.UpdateAudioMixerGroupVolume(musicVolume, AudioType.Music);
        AudioManager.Instance.UpdateAudioMixerGroupVolume(sfxVolume, AudioType.SFX);

        _volumeMusicSlider.onValueChanged.AddListener(OnVolumeMusicChanges);
        _volumeSFXSlider.onValueChanged.AddListener(OnVolumeSFXChanged);

        Debug.Log("Slider Music: " + _volumeMusicSlider.value);
        Debug.Log("Slider SFX: " + _volumeSFXSlider.value);

    }

    public void OnVolumeMusicChanges(float volume)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();

        AudioManager.Instance.UpdateAudioMixerGroupVolume(volume, AudioType.Music);
    }

    public void OnVolumeSFXChanged(float value)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
        PlayerPrefs.Save();

        AudioManager.Instance.UpdateAudioMixerGroupVolume(value, AudioType.SFX);
    }

    [Button("Test Show")]
    public void TestShow()
    {
        Show();
        AudioManager.Instance.PlaySound("Button");
    }

    [Button("Test Hide")]
    public void TestHide()
    {
        Hide();
        AudioManager.Instance.PlaySound("Button");
    }

}
