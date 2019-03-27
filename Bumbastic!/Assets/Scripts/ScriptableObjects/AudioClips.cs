using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips",menuName = "AudioClips")]
public class AudioClips : ScriptableObject
{
    //Music
    public AudioClip inGameMusic;

    //SFx
    public AudioClip bomb;
    public AudioClip buttonDefault, buttonBack, buttonSelection;

    /// <summary>
    /// Method called by buttons
    /// </summary>
    /// <param name="_isBack"></param>
    public void SoundButton(bool _isBack)
    {
        if (_isBack)
        {
            AudioManager.instance.PlayAudio(buttonBack, AudioType.SFx);
        }
        else
            AudioManager.instance.PlayAudio(buttonDefault, AudioType.SFx);
    }
}