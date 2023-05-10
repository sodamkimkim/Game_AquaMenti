using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoundOptionManager : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }

    [SerializeField]
    private TextMeshProUGUI musicSliderText = null;
    [SerializeField]
    private TextMeshProUGUI soundEffectsSliderText = null;


    private void Start()
    {
        float musicVol = SoundManager.Instance.GetMixerVolume("Music");
        float soundEffectsVol = SoundManager.Instance.GetMixerVolume("Sound Effects");

        musicVol = MixerVolumeToVolume(musicVol);
        soundEffectsVol = MixerVolumeToVolume(soundEffectsVol);

        OnMusicSliderValueChange(musicVol);
        OnSoundEffectsSliderValueChange(soundEffectsVol);
    }


    public void OnMusicSliderValueChange(float _val)
    {
        musicVolume = _val;
        if (musicSliderText)
            musicSliderText.text = VolumeToString(_val);
        SoundManager.Instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float _val)
    {
        soundEffectsVolume = _val;
        if (soundEffectsSliderText)
            soundEffectsSliderText.text = VolumeToString(_val);
        SoundManager.Instance.UpdateMixerVolume();
    }

    private string VolumeToString(float _val)
    {
        return ((int)(_val * 100)).ToString();
    }
    private float MixerVolumeToVolume(float _val)
    {
        return Mathf.Pow(10, _val * 0.05f);
    }
}
