using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips",menuName = "AudioClips")]
public class AudioClips : ScriptableObject
{
    //Music
    public AudioClip inGameMusic;

    //SFx
    public AudioClip bomb;

    public void Button(bool _bool)
    {
        AudioManager.instance.PlayAudio(AudioManager.instance.audioClips.bomb, AudioType.SFx);
    }
}