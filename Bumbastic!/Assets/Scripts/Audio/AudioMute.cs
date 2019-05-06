using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMute : MonoBehaviour
{
    public AudioType mType;

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Settings settings;

    float volume;
    float mutedVolume = -80f; //Volume for the group that is going to be muted

    [SerializeField] Color disabledColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] Color enableColor = new Color();
    [SerializeField] Image image;
    [SerializeField] Slider mVolumeSlider;

    public void Init()
    {
        switch (mType)
        {
            case AudioType.Music:
                if (!settings.isMusicActive)
                {
                    image.color = disabledColor;
                    audioMixer.SetFloat("MusicVol", mutedVolume);
                }
                mVolumeSlider.value = settings.musicSlider;
                break;
            case AudioType.SFx:
                if (!settings.isSfxActive)
                {
                    image.color = disabledColor;
                    audioMixer.SetFloat("SFxVol", mutedVolume);
                }
                mVolumeSlider.value = settings.sFxSlider;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Slider to control the volume, only works if the audio is active.
    /// </summary>
    /// <param name="_vol"></param>
    public void Slider(float _vol)
    {
        switch (mType)  
        {
            case AudioType.Music:
                if (settings.isMusicActive)
                {
                    audioMixer.SetFloat("MusicVol", _vol);
                }
                settings.musicSlider = _vol;
                break;
            case AudioType.SFx:
                if (settings.isSfxActive)
                {
                    audioMixer.SetFloat("SFxVol", _vol);
                }
                settings.sFxSlider = _vol;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Function to mute audio
    /// </summary>
    public void MuteAudios()
    {
        float value = 0f;
        switch (mType)   
        {
            case AudioType.Music:
                audioMixer.GetFloat("MusicVol", out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat("MusicVol", mutedVolume);
                    image.color = disabledColor;
                    settings.isMusicActive = false;
                }
                else if (value <= mutedVolume)
                {
                    audioMixer.SetFloat("MusicVol", settings.musicSlider);
                    image.color = enableColor;
                    settings.isMusicActive = true;
                }
                break;
            case AudioType.SFx:
                audioMixer.GetFloat("SFxVol", out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat("SFxVol", mutedVolume);
                    image.color = disabledColor;
                    settings.isSfxActive = false;
                }
                else if (value <= mutedVolume)
                {
                    audioMixer.SetFloat("SFxVol",settings.sFxSlider);
                    image.color = enableColor;
                    settings.isSfxActive = true;
                }
                break;
            default:
                break;
        }
    }
}