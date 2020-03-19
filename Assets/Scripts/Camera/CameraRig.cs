using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraRig : SingletonMonoBehaviour<CameraRig>
{
    public CinemachineBrain cinemachineBrain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCamera(CameraShakeProps cameraShakeProps)
    {
        var vCam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;

        var noiseSettings = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noiseSettings.m_AmplitudeGain = cameraShakeProps.camShakeStrength;
        noiseSettings.m_FrequencyGain = cameraShakeProps.camShakeVibrato;

        this.WaitAndExecute(() =>
        {
            noiseSettings.m_AmplitudeGain = 0;
            noiseSettings.m_FrequencyGain = 0;
        }, cameraShakeProps.camShakeDuration);
    }
}
