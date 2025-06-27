using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangableUIObj : MonoBehaviour
{
    [SerializeField] Sprite sprite1, sprite2;
    Image img;
    UIController_Main uiController;

    private void Awake()
    {
        img = GetComponent<Image>();
        img.sprite = sprite1;

        uiController = GameObject.Find("Canvas").GetComponent<UIController_Main>();
    }

    private void Start()
    {
        uiController.OnHeatEvent += HeatEventListener;    
    }

    private void HeatEventListener(object sender, EventArgs eventArgs)
    {
        StartCoroutine(ConvertSpriteForSeconds(2.5f));
    }

    public IEnumerator ConvertSpriteForSeconds(float duration)
    {
        ConvertSprite(1);
        yield return new WaitForSeconds(duration);
        ConvertSprite(0);
    }

    void ConvertSprite(int i)
    {
        if(i == 0)
        {
            img.sprite = sprite1;
        }
        else //1
        {
            img.sprite = sprite2;
        }
    }
}
