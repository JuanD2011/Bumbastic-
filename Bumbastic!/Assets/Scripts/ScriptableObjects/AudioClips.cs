using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips",menuName = "AudioClips")]
public class AudioClips : ScriptableObject
{
    [Header("Music")]
    public AudioClip inGameMusic;

    [Header("Ambient")]
    public AudioClip desertAmbient, winterAmbient;

    [Header("SFx")]
    public AudioClip bomb, confettiBomb;
    public AudioClip bombThrow, bombReception;
    public AudioClip buttonDefault, buttonBack, buttonSelection;
    public AudioClip crow;
    public AudioClip powerUpBoxDropped;
    public AudioClip dropModule, anticipation;
    public AudioClip stun;
    public AudioClip speedUP;

    [Header("Support")]
    public AudioClip crowdCheer;

    /// <summary>
    /// Method called by buttons
    /// </summary>
    /// <param name="_isBack"></param>
    public void SoundButton(bool _isBack)
    {
        if (_isBack)
            AudioManager.instance.PlaySFx(buttonBack);
        else
            AudioManager.instance.PlaySFx(buttonDefault);
    }
}