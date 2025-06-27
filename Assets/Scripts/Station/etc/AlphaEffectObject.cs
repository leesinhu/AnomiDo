using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaEffectObject : MonoBehaviour
{
    SpriteRenderer spRend;
    [SerializeField] bool playAwake = false;
    [SerializeField] float duration = 0;
    [SerializeField] float maxAlpha = 1;
    [SerializeField] float minAlpha = 0;

    // Start is called before the first frame update
    void Start()
    {
        spRend = GetComponent<SpriteRenderer>();

        if(playAwake)
        {
            StartCoroutine(AlphaInAndOutEffect_Persist(duration, maxAlpha, minAlpha));
        }
    }

    IEnumerator AlphaEffect(float duration, float max, float min, bool b, System.Action onComplete)
    {
        float elapsedTime = 0;
        float startAlpha, endAlpha;
        if(b == true)
        {
            startAlpha = min;
            endAlpha = max;
        }
        else
        {
            startAlpha = max;
            endAlpha = min;
        }

        while(elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration); // 투명도 계산
            spRend.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        spRend.color = new Color(1, 1, 1, endAlpha);
        onComplete?.Invoke();
    }

    public void AlphaInAndOutEffect(float duration, float max, float min)
    {
        StartCoroutine(AlphaEffect(duration/2, max, min, true, ()=>StartCoroutine(AlphaEffect(duration/2, max, min, false, null))));
    }

    public IEnumerator AlphaInAndOutEffect_Persist(float duration, float max, float min)
    {
        while (true)
        {
            yield return StartCoroutine(AlphaEffect(duration / 2, max, min, true, null));
            yield return StartCoroutine(AlphaEffect(duration / 2, max, min, false, null));
        }
    }
}
