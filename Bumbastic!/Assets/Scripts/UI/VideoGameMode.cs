using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoGameMode : MonoBehaviour
{
    RawImage rawImage;
    VideoPlayer videoPlayer;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        if (GameModeDataBase.currentGameMode.VideoClip == null) yield break;
        videoPlayer.clip = GameModeDataBase.currentGameMode.VideoClip;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
}