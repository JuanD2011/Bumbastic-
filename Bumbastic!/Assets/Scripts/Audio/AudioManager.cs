using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Tooltip("ScriptableObject with all audioclips")]
    public AudioClips audioClips;

    [Header("Number of AudioSources")]
    [Tooltip("Number of AudioSource to be instantiated")]
    [Range(2, 10)]
    [SerializeField] int audioSourcesAmount = 2;

    [SerializeField] GameObject audioSourceTemplate;
    [SerializeField] AudioMixer audioMixer;

    List<AudioSource> audioSources;

    AudioSource currentAudioSource;

    [SerializeField] AudioMute[] audioMutes;

    public AudioSource CurrentAudioSource
    {
        get { return currentAudioSource; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        CreateAudioSources(audioSourcesAmount);
    }

    private void Start()
    {
        PlayAudio(audioClips.inGameMusic, AudioType.Music);

        if (audioMutes != null)
        {
            foreach (AudioMute item in audioMutes)
            {
                item.Init();
            } 
        }
    }

    private void CreateAudioSources(int audioSourcesAmount)
    {
        audioSources = new List<AudioSource>();

        for (int i = 0; i < audioSourcesAmount; i++)
        {
            if (i == 0)
            {
                GameObject gameObject = Instantiate(audioSourceTemplate);
                gameObject.name = string.Format("{0} AudioSource_{1}", AudioType.Music.ToString(), i);
                AudioSource audioSourceCreated = gameObject.GetComponent<AudioSource>();
                audioSourceCreated.outputAudioMixerGroup = audioMixer.FindMatchingGroups(AudioType.Music.ToString())[0];
                audioSourceCreated.loop = true;

                if (audioSourceCreated != null)
                {
                    audioSources.Add(audioSourceCreated);
                }
            }
            else
            {
                GameObject gameObject = Instantiate(audioSourceTemplate);
                gameObject.name = string.Format("{0} AudioSource_{1}", AudioType.SFx.ToString(), i);
                AudioSource audioSourceCreated = gameObject.GetComponent<AudioSource>();
                audioSourceCreated.outputAudioMixerGroup = audioMixer.FindMatchingGroups(AudioType.SFx.ToString())[0];

                if (audioSourceCreated != null)
                {
                    audioSources.Add(audioSourceCreated);
                }
            }
        }
    }

    public void PlayAudio(AudioClip _clipToPlay, AudioType _audioType)
    {
        if (GetAudioSource(_audioType) != null && _clipToPlay != null)
        {
            currentAudioSource = GetAudioSource(_audioType);

            switch (_audioType) 
            {
                case AudioType.Music:
                    if (!currentAudioSource.isPlaying)
                    {
                        currentAudioSource.clip = _clipToPlay;
                        StartCoroutine(MusicTrack(currentAudioSource, 0.7f));
                    }
                    else
                    {
                        StartCoroutine(MusicTrack(currentAudioSource, _clipToPlay, 0.7f));
                    }
                    break;
                case AudioType.SFx:
                    currentAudioSource.PlayOneShot(_clipToPlay);
                    break;
                default:
                    currentAudioSource.PlayOneShot(_clipToPlay);
                    break;
            }
        }
    }

    /// <summary>
    /// Init the music with fade in
    /// </summary>
    /// <param name="_currentAudioSource"></param>
    /// <returns></returns>
    private IEnumerator MusicTrack(AudioSource _currentAudioSource, float _Volume)
    {
        _currentAudioSource.volume = 0f;
        _currentAudioSource.Play();

        while (_currentAudioSource.volume < _Volume)
        {
            _currentAudioSource.volume += Time.deltaTime;
            yield return null;
        }

        _currentAudioSource.volume = _Volume;
    }

    /// <summary>
    /// Change the current music to a new one
    /// </summary>
    /// <param name="_currentAudioSource"></param>
    /// <param name="_newMusicTrack"></param>
    /// <returns></returns>
    private IEnumerator MusicTrack(AudioSource _currentAudioSource, AudioClip _newMusicTrack, float _Volume)
    {
        while (_currentAudioSource.volume > 0.05f)
        {
            _currentAudioSource.volume -= Time.deltaTime;
            yield return null;
        }

        _currentAudioSource.volume = 0f;
        _currentAudioSource.clip = _newMusicTrack;

        while (_currentAudioSource.volume < _Volume)
        {
            _currentAudioSource.volume += Time.deltaTime;
            yield return null;
        }
        _currentAudioSource.volume = _Volume;
    }

    private AudioSource GetAudioSource(AudioType _audioType)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            switch (_audioType) 
            {
                case AudioType.Music:
                    if (audioSources[i].outputAudioMixerGroup == audioMixer.FindMatchingGroups(_audioType.ToString())[0])
                    {
                        return audioSources[i];
                        break;
                    }
                    break;
                case AudioType.SFx:
                    if (audioSources[i].volume == 1 && audioSources[i].pitch == 1)
                    {
                        if (audioSources[i].outputAudioMixerGroup == audioMixer.FindMatchingGroups(_audioType.ToString())[0])
                        {
                            return audioSources[i];
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        GameObject gameObject = Instantiate(audioSourceTemplate);
        gameObject.name = string.Format("{0} AudioSource_{1}", _audioType.ToString(), audioSources.Count);
        AudioSource audioSourceCreated = gameObject.GetComponent<AudioSource>();
        audioSourceCreated.outputAudioMixerGroup = audioMixer.FindMatchingGroups(_audioType.ToString())[0];

        if (audioSourceCreated != null)
        {
            audioSources.Add(audioSourceCreated);
        }

        return audioSourceCreated;
    }
}