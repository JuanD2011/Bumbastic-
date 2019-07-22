using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips",menuName = "AudioClips")]
public class AudioClips : ScriptableObject
{
    [Header("Music")]
    public AudioClip inGameMusic;
    public AudioClip transitionMusic;

    [Header("Ambient")]
    public AudioClip desertAmbient;
    public AudioClip winterAmbient;

    [Header("SFx")]
    public AudioClip bumba;
    public AudioClip go;
    public AudioClip mrBumbasticIs;
    public AudioClip cLose, cWin, cTransmitter, cStun, cThrow, cSpeedUP;
    public AudioClip bomb;
    public AudioClip confettiBomb;
    public AudioClip bombThrow;
    public AudioClip bombReception;
    public AudioClip buttonDefault;
    public AudioClip buttonBack;
    public AudioClip buttonSelection;
    public AudioClip crow;
    public AudioClip powerUpBoxDropped;
    public AudioClip dropModule;
    public AudioClip anticipation;
    public AudioClip stun;
    public AudioClip wagonHit;
    public AudioClip rollingWagon;
    public AudioClip speedUP;
    public AudioClip dash;

    [Header("Support")]
    public AudioClip crowdCheer;
}