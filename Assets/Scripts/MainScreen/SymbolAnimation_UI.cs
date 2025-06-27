using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolAnimation_UI : MonoBehaviour
{
    [SerializeField] float floatStrength = 0.2f;  // 위아래로 움직이는 범위
    [SerializeField] float floatSpeed = 3f;     // 움직이는 속도
    [SerializeField] float heightOffset = 0;
    [SerializeField] float startDelay = 0;
    [SerializeField] RectTransform targetRect;
    Image img_Symbol;
    RectTransform thisRect, parentRect;

    private Vector2 startPos;

    private static readonly Color Transparent = new Color(1, 1, 1, 0);
    private static readonly Color Opaque = new Color(1, 1, 1, 1);

    void Start()
    {
        thisRect = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        img_Symbol = GetComponent<Image>();

        if (targetRect != null)
        {
            startPos = new Vector2(targetRect.anchoredPosition.x, targetRect.anchoredPosition.y + heightOffset);
        }
        else
        {
            startPos = new Vector2(thisRect.anchoredPosition.x, thisRect.anchoredPosition.y + heightOffset);
        }
        
        StartCoroutine(FloatUpDown(startDelay));
    }

    private void Update()
    {
        if (targetRect != null)
        {
            startPos = new Vector2(targetRect.anchoredPosition.x, targetRect.anchoredPosition.y + heightOffset);
        }
    }

    IEnumerator FloatUpDown(float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            float time = 0f;
            while (time < Mathf.PI * 2) // 한 주기 (360도)
            {
                float newY = startPos.y + Mathf.Sin(time) * floatStrength;
                thisRect.anchoredPosition = new Vector3(startPos.x, newY + heightOffset);
                time += Time.deltaTime * floatSpeed;
                yield return null;
            }
        }
    }

    public void SetSymbol(bool isVisible)
    {
        if (isVisible)
        {
            img_Symbol.color = Opaque;
            StartCoroutine(FloatUpDown());
        }
        else
        {
            img_Symbol.color = Transparent;
        }
    }
}
