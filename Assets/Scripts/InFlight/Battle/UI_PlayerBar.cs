using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerBar : UI_Scene
{
    enum GameObjects
    {
        HPBar,
        StaminaBar,
    }

    PlayerStat _stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    private void Update()
    {
        //Transform parent = transform.parent;
        //transform.position = parent.position + new Vector3(0.0f, 2.6f, 0.0f);
        //transform.rotation = Camera.main.transform.rotation;

        float HPRatio = _stat.Hp / (float)_stat.MaxHp;
        float StaminaRatio = _stat.Stamina / _stat.MaxStamina;

        SetHpRatio(HPRatio);
        SetStaminaRatio(StaminaRatio);
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }

    public void SetStaminaRatio(float ratio)
    {
        GetObject((int)GameObjects.StaminaBar).GetComponent<Slider>().value = ratio;
    }
}
