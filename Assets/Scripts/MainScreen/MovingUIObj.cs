using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovingUIObj : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] RectTransform pos1, pos2;
    [SerializeField] bool autoPlay = true;
    RectTransform rect;
    Vector2 dir;
    Coroutine coroutine = null;

    Image img;
    CanvasGroup group;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        group = GetComponent<CanvasGroup>();

        if (autoPlay)
        {
            coroutine = StartCoroutine(MoveUI(15));
        }
    }

    private void Update()
    {
        if(coroutine == null && autoPlay)
        {
            coroutine = StartCoroutine(MoveUI(15));
        }
    }

    public IEnumerator MoveUI(float duration, bool doAlphaConvert = false)
    {
        if (group != null)
            group.alpha = 1;
        else
            img.color = new Color(1, 1, 1, 1);


        Vector2 startPos = pos1.anchoredPosition;
        Vector2 endPos = pos2.anchoredPosition;
        float elapsedtime = 0;

        while (elapsedtime < duration)
        {
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedtime / duration);
            if(doAlphaConvert)
            {
                if(group != null)
                    group.alpha = Mathf.Lerp(1, 0, elapsedtime / duration);
                else
                {
                    float alpha = Mathf.Lerp(1, 0, elapsedtime / duration); // 투명도 계산
                    img.color = new Color(1, 1, 1, alpha);
                }
            }
            elapsedtime += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = endPos;

        if (group != null)
            group.alpha = 0;
        else
            img.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(3);
        rect.anchoredPosition = startPos;
        coroutine = null;
    }
}
