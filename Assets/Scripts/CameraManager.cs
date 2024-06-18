using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera cinemachine;
    CinemachineBasicMultiChannelPerlin noise;
    float timeSinceShake, maxShakeTime;
    float unScaledAmplitude;

    void Awake()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();

        noise = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceShake += Time.deltaTime;
        noise.m_AmplitudeGain = Mathf.Lerp(unScaledAmplitude, 0f, Mathf.Clamp(timeSinceShake / maxShakeTime, 0f, 1f));
    }
    public void CameraShake(float amplitude, float frequency, float time)
    {
        unScaledAmplitude = amplitude;
        noise.m_FrequencyGain = frequency;
        maxShakeTime = time;
        timeSinceShake = 0f;
    }
}
