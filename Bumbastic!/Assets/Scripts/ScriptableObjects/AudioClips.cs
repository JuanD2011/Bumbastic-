using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips",menuName = "AudioClips")]
public class AudioClips : ScriptableObject
{
    //Music
    public AudioClip inGameMusic;

    //Ambient
    public AudioClip desertAmbient, winterAmbient;

    //SFx
    public AudioClip bomb, confettiBomb;
    public AudioClip bombThrow, bombReception;
    public AudioClip buttonDefault, buttonBack, buttonSelection;
    public AudioClip crow;
    public AudioClip powerUpBoxDropped;
    public AudioClip dropModule, anticipation;
    public AudioClip stun;
    public AudioClip speedUP;

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