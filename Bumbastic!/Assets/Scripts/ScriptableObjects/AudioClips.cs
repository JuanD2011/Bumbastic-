using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips",menuName = "AudioClips")]
public class AudioClips : ScriptableObject
{
    //Music
    public AudioClip inGameMusic;

    //SFx
    public AudioClip bomb, confettiBomb;
    public AudioClip buttonDefault, buttonBack, buttonSelection;
    public AudioClip crow;
    public AudioClip powerUpBoxDropped;
    public AudioClip dropModule, anticipation;

    /// <summary>
    /// Method called by buttons
    /// </summary>
    /// <param name="_isBack"></param>
    public void SoundButton(bool _isBack)
    {
        if (_isBack)
        {
            AudioManager.instance.PlaySFx(buttonBack);
        }
        else
            AudioManager.instance.PlaySFx(buttonDefault);
    }
}