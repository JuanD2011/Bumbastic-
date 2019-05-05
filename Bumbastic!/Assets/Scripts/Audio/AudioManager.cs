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
        PlayMusic(audioClips.inGameMusic, 0.6f, 1f, 2f);

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

    #region SFx methods.
    public void PlaySFx(AudioClip _clipToPlay)
    {
        currentAudioSource = GetAudioSource(AudioType.SFx);

        if (currentAudioSource != null && _clipToPlay != null)
        {
            currentAudioSource.PlayOneShot(_clipToPlay);
        }
    }

    public void PlaySFx(AudioClip _clipToPlay, float _volume)
    {
        currentAudioSource = GetAudioSource(AudioType.SFx);

        if (currentAudioSource != null && _clipToPlay != null)
        {
            currentAudioSource.PlayOneShot(_clipToPlay, _volume);
        }
    }

    /// <summary>
    /// This is only for loops, you don't know when it is going to finish.
    /// </summary>
    /// <param name="_clipToPlay"></param>
    /// <param name="_play"></param>
    /// <param name="_volume"></param>
    public void PlaySFx(AudioClip _clipToPlay, bool _play, float _volume)
    {
        if (!_play)
        {
            currentAudioSource = GetAudioSource(AudioType.SFx);
        }
        else
        {
            //currentAudioSource = GetAudioSource(AudioType.SFx, true);
        }

        if (currentAudioSource != null && _clipToPlay != null)
        {
            currentAudioSource.clip = _clipToPlay;
            currentAudioSource.Play();
            currentAudioSource.loop = _play;
        }
    }
    #endregion

    #region Music methods.
    /// <summary>
    /// Time to Fade out will only works if there's another music playing.
    /// </summary>
    /// <param name="_clipToPlay"></param>
    /// <param name="_audioType"></param>
    /// <param name="_volume"></param>
    /// <param name="_timeToFadeOut"></param>
    /// <param name="_timeToFadeIn"></param>
    public void PlayMusic(AudioClip _clipToPlay, float _volume, float _timeToFadeOut, float _timeToFadeIn)
    {
        currentAudioSource = GetAudioSource(AudioType.Music);

        if (currentAudioSource != null && _clipToPlay != null)
        {
            if (!currentAudioSource.isPlaying)
            {
                currentAudioSource.clip = _clipToPlay;
                StartCoroutine(MusicTrack(currentAudioSource, _volume , _timeToFadeIn));
            }
            else
            {
                StartCoroutine(ChangeMusicTracks(currentAudioSource, _clipToPlay, _volume, _timeToFadeOut, _timeToFadeIn));
            }
        }
    }

    /// <summary>
    /// Init the music with a Time to fade in
    /// </summary>
    /// <param name="_currentAudioSource"></param>
    /// <returns></returns>
    private IEnumerator MusicTrack(AudioSource _currentAudioSource, float _volume, float _timeToFadeIn)
    {
        _currentAudioSource.volume = 0f;
        _currentAudioSource.Play();

        float elapsedTime = 0f;
        float currentVolume = _currentAudioSource.volume;

        while (elapsedTime < _timeToFadeIn)
        {
            _currentAudioSource.volume = Mathf.Lerp(currentVolume, _volume, elapsedTime / _timeToFadeIn);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _currentAudioSource.volume = _volume;
    }

    private IEnumerator ChangeMusicTracks(AudioSource _currentAudioSource, AudioClip _newMusicTrack, float _volume, float _timeToFadeOut, float _timeToFadeIn)
    {
        float elapsedTime = 0f;
        float currentVolume = _currentAudioSource.volume;

        while (elapsedTime < _timeToFadeOut)
        {
            _currentAudioSource.volume = Mathf.Lerp(currentVolume, 0f, elapsedTime / _timeToFadeOut);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _currentAudioSource.clip = _newMusicTrack;

        currentVolume = 0f;
        elapsedTime = 0f;

        while (elapsedTime < _timeToFadeIn)
        {
            _currentAudioSource.volume = Mathf.Lerp(currentVolume, _volume, elapsedTime / _timeToFadeIn);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _currentAudioSource.volume = _volume;
    }
    #endregion

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