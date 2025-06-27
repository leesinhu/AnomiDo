using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UiController_InFlight : MonoBehaviour
{
    [SerializeField] GameObject UpgradePanel, SleepPanel;

    [SerializeField] TextMeshProUGUI text_Level_Max, text_Level_Speed, text_Level_LossAmount;
    [SerializeField] TextMeshProUGUI text_UpgradeCost_Max, text_UpgradeCost_Speed, text_UpgradeCost_LossAmount;
    [SerializeField] TextMeshProUGUI text_credit;
    [SerializeField] GameObject obj_pausePanel, obj_GameOver;
    [SerializeField] Button button_upgrade_max, button_upgrade_speed, button_upgrade_lossAmount;
    [SerializeField] Sprite[] sprite_numbers;

    [SerializeField] GameObject obj_interactGuide, obj_addCredit;
    TextMeshProUGUI text_interact, text_addCredit;
    List<Image> imges_tut = new List<Image>();

    //Effect
    [SerializeField] GameObject blurEffect;
    [SerializeField] Image img_fade, img_blind;
    [SerializeField] AudioSource audio_get, audio_upgrade, audio_laberUp, audio_laberDown;
    AudioSource audio_button1, audio_button4;

    //Common
    ShipManager shipManager;
    Coroutine coroutine;

    // Start is called before the first frame update
    void Awake()
    {
        shipManager = GameObject.Find("ShipManager").GetComponent<ShipManager>();
        UpgradePanel.SetActive(false);
        img_fade.color = new Color(0, 0, 0, 1);

        text_interact = obj_interactGuide.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text_addCredit = obj_addCredit.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        obj_interactGuide.SetActive(false);

        Transform parent_tutImg = transform.Find("img_tut");
        foreach (Transform child in parent_tutImg)
        {
            Image img = child.GetComponent<Image>();
            imges_tut.Add(img);
        }

        audio_button1 = GameObject.Find("MainCamera").transform.Find("Audio_Button1").GetComponent<AudioSource>();
        audio_button4 = GameObject.Find("MainCamera").transform.Find("Audio_Button4").GetComponent<AudioSource>();
    }

    private void Start()
    {
        text_credit.transform.parent.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        text_Level_Max.text = "Lv. " + GameManager.Instance.level_Max + " 저장량 : " + GameManager.Instance.GetUpgradeElement("Max");
        text_Level_Speed.text = "Lv. " + GameManager.Instance.level_Speed + " 충전 속도 : " + GameManager.Instance.GetUpgradeElement("Speed");
        text_Level_LossAmount.text = "Lv. " + GameManager.Instance.level_LossAmount + " 체온 감소량 : " + GameManager.Instance.GetUpgradeElement("LossAmount");

        text_UpgradeCost_Max.text = "X " + GameManager.Instance.GetCreditCost("Max");
        text_UpgradeCost_Speed.text = "X " + GameManager.Instance.GetCreditCost("Speed");
        text_UpgradeCost_LossAmount.text = "X " + GameManager.Instance.GetCreditCost("LossAmount");

        text_credit.text = GameManager.Instance.credit.ToString();

        if (GameManager.Instance.credit < GameManager.Instance.GetCreditCost("Max"))
        {
            button_upgrade_max.interactable = false;
        }
        else if (GameManager.Instance.level_Max >= 5)
        {
            button_upgrade_max.interactable = false;
            text_UpgradeCost_Max.text = "최대\n강화";
        }
        else
        {
            button_upgrade_max.interactable = true;
        }

        if (GameManager.Instance.credit < GameManager.Instance.GetCreditCost("Speed"))
        {
            button_upgrade_speed.interactable = false;
        }
        else if (GameManager.Instance.level_Speed >= 5)
        {
            text_UpgradeCost_Speed.text = "최대\n강화";
            button_upgrade_speed.interactable = false;
        }
        else
        {
            button_upgrade_speed.interactable = true;
        }

        if (GameManager.Instance.credit < GameManager.Instance.GetCreditCost("LossAmount"))
        {
            button_upgrade_lossAmount.interactable = false;
        }
        else if (GameManager.Instance.level_LossAmount >= 5)
        {
            text_UpgradeCost_LossAmount.text = "최대\n강화";
            button_upgrade_lossAmount.interactable = false;
        }
        else
        {
            button_upgrade_lossAmount.interactable = true;
        }

        if(!shipManager.interactPermit)
        {
            obj_interactGuide.SetActive(false);
        }

        switch (shipManager.stageState)
        {
            case InFlightStageState.Idle:
                //IdleStageSetting();
                break;
            case InFlightStageState.Fight:
                break;
            case InFlightStageState.StageEnd:
                obj_GameOver.SetActive(true);
                break;
        }
    }

    public void OpenUpgradePanel()
    {
        audio_laberUp.Play();
        UpgradePanel.SetActive(true);
    }

    public void CloseUpgradePanel()
    {
        audio_laberDown.Play();
        UpgradePanel.gameObject.SetActive(false);
        shipManager.CloseUpgradePanel();
        /*if (shipManager.stageState < InFlightStageState.Fight)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            else
            {
                shipManager.ConvertStageState(InFlightStageState.Fight);
            }
        }*/
    }

    public void OpenSleepPanel()
    {
        SleepPanel.SetActive(true);
    }

    public void CloseSleepPanel(bool b = false)
    {
        SleepPanel.SetActive(false);
        audio_upgrade.PlayOneShot(audio_upgrade.clip);
        if (!b)
        {
            shipManager.movePermit = true;
            StartCoroutine(shipManager.DelayToInteractPermit(0.5f));
        }
    }

    public void SleepInBed()
    {
        audio_upgrade.PlayOneShot(audio_upgrade.clip);
        shipManager.Sleep();
    }

    public void SetInterActText(bool on, string sentence = null)
    {
        if(shipManager.interactPermit)
        {
            if (on)
            {
                obj_interactGuide.SetActive(true);
                text_interact.text = "SPACE BAR - " + sentence;
            }
            else
            {
                obj_interactGuide.SetActive(false);
            }
        }
    }

    public void GoToMainScreen()
    {
        OpenMenu(false);
        GameManager.Instance.Initialize(); //게임 초기화
        GameManager.Instance.LoadSceneByIndex(0);
    }

    public void Retry()
    {
        shipManager.movePermit = false;
        shipManager.interactPermit = false;
        OpenMenu(false);
        if (shipManager.stageState == InFlightStageState.Idle)
            StartCoroutine(FadeToRestartScene(0.5f, shipManager.audio_main));
        else
            StartCoroutine(FadeToRestartScene(0.5f, shipManager.audio_fight));
    }

    public void OpenMenu(bool b = true)
    {
        if (!obj_pausePanel.gameObject.activeSelf && b)
        {
            audio_button1.Play();
            obj_pausePanel.gameObject.SetActive(true);
        }
        else
        {
            audio_button4.Play();
            obj_pausePanel.gameObject.SetActive(false);
        }
    }

    public bool GetIsMenuActiveSelf()
    {
        bool b = obj_pausePanel.gameObject.activeSelf;
        return b;
    }

    public void Upgrade_Max()
    {
        audio_upgrade.PlayOneShot(audio_upgrade.clip);
        GameManager.Instance.Upgrade("Max");
    }

    public void Upgrade_Speed()
    {
        audio_upgrade.PlayOneShot(audio_upgrade.clip);
        GameManager.Instance.Upgrade("Speed");
    }

    public void Upgrade_LossAmount()
    {
        audio_upgrade.PlayOneShot(audio_upgrade.clip);
        GameManager.Instance.Upgrade("LossAmount");
    }

    public void FadeTutImg(int index, bool b)
    {
        StartCoroutine(FadeImg(imges_tut[index], b));
    }

    public void FlickTutImg(int index, bool b)
    {
        if (b)
            StartCoroutine(FlickImg(imges_tut[index]));
        else
            imges_tut[index].gameObject.SetActive(false);
    }

    IEnumerator FadeImg(Image img, bool b, float fadeTime = 0.25f)
    {
        float elapsedTime = 0;
        float startAlpha = 0, endAlpha = 1;
        if (!b)
        {
            startAlpha = img.color.a;
            endAlpha = 0;
        }

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeTime); // 투명도 계산
            img.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        img.color = new Color(1, 1, 1, endAlpha); // 완전 투명
    }

    IEnumerator FlickImg(Image img)
    {
        float elapsedTime = 0;
        float fadeTime = 0.5f;
        while (img.gameObject.activeSelf)
        {
            elapsedTime = 0;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime); // 투명도 계산
                img.color = new Color(1, 1, 1, alpha);
                yield return null;
            }
            elapsedTime = 0;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime); // 투명도 계산
                img.color = new Color(1, 1, 1, alpha);
                yield return null;
            }
        }
    }

    public IEnumerator FadeInAndOut_ConvertStageState(float inTime, float waitTime1, float outTime, StationStageState state, bool foregroundOpen,
        System.Action onComplete)
    {
        if (inTime > 0)
        {
            coroutine = StartCoroutine(FadeIn(inTime));
            yield return new WaitUntil(() => coroutine == null);
        }

        /*if (passManager.playerState == PlayerState.OutSide)
        {
            ConvertPlayerState();
        }*/

        yield return new WaitForSeconds(waitTime1);

        coroutine = StartCoroutine(FadeOut(outTime));
        yield return new WaitUntil(() => coroutine == null);
        onComplete?.Invoke();
    }

    public IEnumerator FadeInAndOut(float inTime, float waitTime1, float outTime, System.Action onComplete)
    {
        if (inTime > 0)
        {
            coroutine = StartCoroutine(FadeIn(inTime));
            yield return new WaitUntil(() => coroutine == null);
        }

        yield return new WaitForSeconds(waitTime1);

        coroutine = StartCoroutine(FadeOut(outTime));
        yield return new WaitUntil(() => coroutine == null);
        onComplete?.Invoke();
    }

    public IEnumerator FadeInAndOut_SetPlayer(float inTime, float waitTime1, float outTime, Vector2 pos, Vector2 dir, AudioSource audio, System.Action onComplete)
    {
        if (inTime > 0)
        {
            coroutine = StartCoroutine(FadeIn(inTime));
            StartCoroutine(GameManager.Instance.SoundFade(audio, inTime, audio.volume, 0));
            yield return new WaitUntil(() => coroutine == null);
        }

        yield return new WaitForSeconds(waitTime1/2);
        shipManager.SetPlayer(pos, dir);
        yield return new WaitForSeconds(waitTime1 / 2);

        coroutine = StartCoroutine(FadeOut(outTime));
        yield return new WaitUntil(() => coroutine == null);
        onComplete?.Invoke();
    }

    public IEnumerator FadeOut(float fadeTime = 0.5f, float waitTime = 0)
    {
        float elapsedTime = 0f;
        yield return new WaitForSeconds(waitTime);

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime); // 투명도 계산
            img_fade.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        img_fade.color = new Color(0, 0, 0, 0); // 완전 투명

        if (coroutine != null)
            coroutine = null;
    }

    public IEnumerator FadeIn(float fadeTime = 0.5f, float waitTime = 0)
    {
        float elapsedTime = 0f;
        yield return new WaitForSeconds(waitTime);

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime); // 투명도 계산
            img_fade.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        img_fade.color = new Color(0, 0, 0, 1); // 완전 불투명

        if (coroutine != null)
            coroutine = null;
    }

    public IEnumerator FadeToEndScene(float fadeTime = 0.5f, AudioSource audio = null)
    {
        coroutine = StartCoroutine(FadeIn(fadeTime));
        StartCoroutine(GameManager.Instance.SoundFade(audio, fadeTime, audio.volume, 0));
        yield return new WaitUntil(() => coroutine == null);

        shipManager.enabled = false; //제대로 종료하기 위함
        obj_interactGuide.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.LoadSceneByIndex(0);
    }

    public IEnumerator FadeToRestartScene(float fadeTime = 0.5f, AudioSource audio = null)
    {
        coroutine = StartCoroutine(FadeIn(fadeTime));
        StartCoroutine(GameManager.Instance.SoundFade(audio, fadeTime, audio.volume, 0));
        yield return new WaitUntil(() => coroutine == null);

        shipManager.enabled = false; //제대로 종료하기 위함
        obj_interactGuide.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.LoadSceneByIndex(GameManager.Instance.GetCurrentSceneIndex());
    }

    public IEnumerator AddCredit(int getCredit = 0)
    {
        text_addCredit.text = "크레딧 + " + getCredit;
        StartCoroutine(obj_addCredit.GetComponent<MovingUIObj>().MoveUI(1f, true));
        yield return new WaitForSeconds(1f);
        GameManager.Instance.credit += getCredit;
        if(getCredit != 0)
            audio_get.Play();
    }

    public void SetBlind(bool b)
    {
        if (b)
            img_blind.color = new Color(1, 1, 1, 1);
        else
            img_blind.color = new Color(1, 1, 1, 0);
    }
}
