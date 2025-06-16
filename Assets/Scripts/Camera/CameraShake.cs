using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake instance;
    [SerializeField]
    private CinemachineVirtualCamera vCam;
    [SerializeField]
    private float shakeTime;
    private CinemachineBasicMultiChannelPerlin perlin;

    private void Start()
    {
        perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        instance = this;
    }

    private void Update()
    {

        if (shakeTime > 0f)
        {
            shakeTime -= Time.deltaTime;
            if (shakeTime <= 0f)
            {
                perlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public void Shake(float duration, float intensity)
    {
        shakeTime = duration;
        perlin.m_AmplitudeGain = intensity;
    }
}
