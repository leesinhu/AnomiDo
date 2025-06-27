using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform _player;
    private Vector3 _shakeOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 basePos = new Vector3(_player.position.x, _player.position.y, -500);
        transform.position = basePos + _shakeOffset;
    }

    public IEnumerator ShakeCamera(float shakeDuration, float shakeMagnitudePos)
    {
        float passTime = 0.0f;
        while (passTime < shakeDuration)
        {
            Vector2 offset2D = Random.insideUnitCircle * shakeMagnitudePos;
            _shakeOffset = new Vector3(offset2D.x, offset2D.y, 0f);

            passTime += Time.deltaTime;
            yield return null;
        }
        _shakeOffset = Vector3.zero;
    }

    public IEnumerator WaveShake(float duration, float magnitude, float frequency)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Mathf.Sin(Time.time * frequency) * magnitude;
            float y = Mathf.Cos(Time.time * frequency) * magnitude;

            transform.position = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    public void StartWaveShake(float duration, float magnitude, float frequency = 20f)
    {
        StartCoroutine(WaveShake(duration, magnitude, frequency));
    }
}
