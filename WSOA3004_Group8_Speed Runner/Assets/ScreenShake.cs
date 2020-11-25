using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public float shakeDuration;
    public float shakeAmplitude;
    public float shakeFrequency;
    private float shakeElapsedTime = 0f;

    public CinemachineVirtualCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin virtualCamNoise;


    // Start is called before the first frame update
    void Start()
    {
        if (cinemachineCam != null)
        {
            virtualCamNoise = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cinemachineCam != null || virtualCamNoise != null)
        {
            if (shakeElapsedTime > 0)
            {
                virtualCamNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCamNoise.m_FrequencyGain = shakeFrequency;
                shakeElapsedTime -= Time.deltaTime;

            }
            else
            {
                virtualCamNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0;
            }
        }
    }


    public void PlayerHasDIed()
    {
        shakeElapsedTime = shakeDuration;
    }
}
