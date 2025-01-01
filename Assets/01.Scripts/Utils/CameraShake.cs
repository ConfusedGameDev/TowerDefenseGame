using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class CameraShake : SingletonComponent<CameraShake>
{
    public float shakeDuration;
    public bool isShaking;
    public float shakeSpeed;
    public AnimationCurve shakeAmplitude;
    public float shakeIntensity = 1f;
    public bool movementShake;
    public bool rotationShake;
    public Vector3 positionOffsetDirection;
    public Vector3 rotationOffsetDirection;
    public int perShakeDuration = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    [Button]
    public void Shake()
    {
        StartCoroutine(onShake());
    }
    IEnumerator onShake()
    {
        isShaking = true;
        var originalPosition = transform.position;
        var originalRotation = transform.rotation;
        var delta = 0f;
        bool addValue = true;
        while(delta<shakeDuration)
        {
            for (int i = 0; i < perShakeDuration; i++)
            {
                yield return new WaitForEndOfFrame();
                delta += Time.deltaTime;
            }
            if(movementShake)
            transform.position += positionOffsetDirection.normalized *shakeIntensity*shakeAmplitude.Evaluate(delta/shakeDuration) *( addValue ? 1f : -1f); 
            if(rotationShake)
                transform.rotation*=  Quaternion.Euler(rotationOffsetDirection * shakeIntensity * shakeAmplitude.Evaluate(delta / shakeDuration) * (addValue ? 1f : -1f));
            addValue = !addValue;
            
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isShaking=false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
