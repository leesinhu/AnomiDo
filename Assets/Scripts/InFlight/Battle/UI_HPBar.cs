using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }

    Stat _stat;
    CanvasGroup _canvasGroup;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<Stat>();
        _canvasGroup = transform.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + new Vector3(0.0f, 2.6f, 0.0f);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = _stat.Hp / (float)_stat.MaxHp;

        if (_stat.Hp < 1)
        {
            gameObject.SetActive(false);
        }

        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }

    public void ShowBar()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void HideBar()
    {
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
