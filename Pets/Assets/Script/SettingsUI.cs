using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class SettingsUI : UIWindow
{
    [Header("SettingsUI")]
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _soundSlider;


    public override void Initialized()
    {
        base.Initialized();
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        _soundSlider.onValueChanged.AddListener(OnSoundChanged);

    }

    private void OnVolumeChanged(float value)
    {
        Debug.Log($"Volume changed to: {value}");
    }

    private void OnSoundChanged(float value)
    {
        Debug.Log($"Sound changed to: {value}");
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
