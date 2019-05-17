using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips",menuName = "AudioClips")]
public class AudioClips : ScriptableObject
{
    [Header("Music")]
    public AudioClip inGameMusic;
    public AudioClip transitionMusic;

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
    public AudioClip wagonHit, rollingWagon;
    public AudioClip speedUP;

    [Header("Support")]
    public AudioClip crowdCheer;
}