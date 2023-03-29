using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraChaker : MonoBehaviour
{
    public static CameraChaker instance; 
    private CinemachineBasicMultiChannelPerlin _channelPerlin;
    private WaitForSeconds _wait = new WaitForSeconds(0.15f);
    public void StartShake(float amplitude = 0.5f, float frequency = 0.15f) => StartCoroutine(Shake(amplitude, frequency));
    private void Awake() => instance = this;
    private void Start() => _channelPerlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    private IEnumerator Shake(float amplitude, float frequency)
    {
        _channelPerlin.m_AmplitudeGain = amplitude;
        _channelPerlin.m_FrequencyGain = frequency;
        yield return _wait;
        _channelPerlin.m_AmplitudeGain = 0;
        _channelPerlin.m_FrequencyGain = 0;
    }
}
