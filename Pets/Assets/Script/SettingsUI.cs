using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Dino.Utility.Audio;
using UnityEngine.Rendering;
using AudioType = Dino.Utility.Audio.AudioType;

public class SettingsUI : UIWindow
{
    [Header("SettingsUI")]
    [SerializeField] private Slider _volumeSFXSlider;
    [SerializeField] private Slider _volumeMusicSlider;


    public override void Initialized()
    {
        base.Initialized();
        _volumeMusicSlider.onValueChanged.AddListener(OnVolumeMusicChanges);
        _volumeSFXSlider.onValueChanged.AddListener(OnVolumeSFXChanged);

    }

    private void OnVolumeMusicChanges(float volume)
    {
        Debug.Log($"Volume changed SFX to: {volume}");
        AudioManager.Instance.UpdateAudioMixerGroupVolume(volume, AudioType.Music);
        
    }

    private void OnVolumeSFXChanged(float value)
    {
        Debug.Log($"Sound changed Muisc to: {value}");
        AudioManager.Instance.UpdateAudioMixerGroupVolume(value, AudioType.SFX);
    }

    [Button("Test Show")]
    private void TestShow()
    {
        Show();
    }

    [Button("Test Hide")]
    private void TestHide()
    {
        Hide();
    }

}
