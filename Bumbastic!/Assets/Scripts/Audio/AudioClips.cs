using UnityEngine;
using UnityEngine.Audio;

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
    public AudioClip starObtained;
    public AudioClip crow;
    public AudioClip powerUpBoxDropped, powerUpBoxOpened, powerUpBubble, powerUpBoxExplosion;
    public AudioClip dropModule;
    public AudioClip anticipation;
    public AudioClip dropingWorld;
    public AudioClip stun;
    public AudioClip wagonHit;
    public AudioClip rollingWagon;
    public AudioClip powerUpSpeedUP, powerUpMagnet, dash, tangleExplosion;

    [Header("Support")]
    public AudioClip crowdCheer;

    [Header("Audio Mixer Snapshots")]
    public AudioMixerSnapshot normalSnapshot = null, tangledSnapshot = null;
}