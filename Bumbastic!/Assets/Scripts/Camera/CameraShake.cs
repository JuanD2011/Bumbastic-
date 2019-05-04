using System.Collections;
using UnityEngine;
using Cinemachine;
using System;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else {
            Destroy(this);
        }
    }

    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public delegate void DelCamera(float _duration, float _shakeAmplitude, float _shakeFrequency);
    public DelCamera OnShakeDuration;

    public Action<float, float> OnStartShake;
    public Action OnStopShake;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (virtualCamera != null)
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        OnShakeDuration += ShakeOnce;
        OnStartShake += StartShaking;
        OnStopShake += StopShaking;
    }

    /// <summary>
    /// It sets the amplitude and frequency, it does not stop.
    /// </summary>
    /// <param name="_shakeAmplitude"></param>
    /// <param name="_shakeFrequency"></param>
    private void StartShaking(float _shakeAmplitude, float _shakeFrequency)
    {
        virtualCameraNoise.m_AmplitudeGain = _shakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = _shakeFrequency;
    }

    private void ShakeOnce(float _duration, float _shakeAmplitude, float _shakeFrequency)
    {
        StartCoroutine(ShakeCamera(_duration, _shakeAmplitude, _shakeFrequency));
    }

    private IEnumerator ShakeCamera(float _duration, float _shakeAmplitude, float _shakeFrequency)
    {
        virtualCameraNoise.m_AmplitudeGain = _shakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = _shakeFrequency;
        yield return new WaitForSeconds(_duration);
        StopShaking();
    }

    private void StopShaking()
    {
        virtualCameraNoise.m_FrequencyGain = 0f;
        virtualCameraNoise.m_AmplitudeGain = 0f;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        StopShaking();
    }
}