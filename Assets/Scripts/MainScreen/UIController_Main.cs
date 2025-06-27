using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController_Main : MonoBehaviour
{
    ScriptControl_0 sc0;
    [SerializeField] AudioSource audio_main, audio_wind, audio_keyboard, audio_button1, audio_button2, audio_button3, audio_button4, 
        audio_ShipGo, audio_ShipStop;
    [SerializeField] Image panel_stageSelect, img_fade, img_end;
    [SerializeField] GameObject obj_blur;
    [SerializeField] GameObject obj_miniShip;
    [SerializeField] MovingUIObj obj_bigShip;
    [SerializeField] Button button_GameStart, button_Info, button_GameQuit;
    [SerializeField] GameObject[] image_prologue;
    [SerializeField] GameObject[] image_ending;
    [SerializeField] Transform panel_guide;
    List<GameObject> guideImage = new List<GameObject>();
    int guidePage = 0;
    Button button_miniShip;
    Animator anim_miniShip;
    GameObject mark_miniShip;
    Coroutine coroutine;

    float volume_main = 0.5f;
    float volume_wind = 0.5f;

    public event EventHandler OnHeatEvent; //열화상 전환 이벤트
    float delay_heatEvent;
    [SerializeField] float term_heatEvent;

    private void Awake()
    {
        sc0 = GameObject.Find("ScriptManager").GetComponent<ScriptControl_0>();
        button_miniShip = obj_miniShip.GetComponent<Button>();
        anim_miniShip = obj_miniShip.GetComponent<Animator>();
        mark_miniShip = panel_stageSelect.transform.Find("ShipMark").gameObject;
        mark_miniShip.SetActive(false);

        if (GameManager.Instance.gameState == GameState.gamePlaying)
        {
            img_fade.color = new Color(0, 0, 0, 1);
            if (GameManager.Instance.ActiveStageNum < 11)
            {
                audio_main.mute = true;
                Open_WorldMap();
            }
        }
        else
        {
            panel_stageSelect.gameObject.SetActive(false);
        }

        for(int i=0; i<panel_guide.childCount; i++)
        {
            guideImage.Add(panel_guide.GetChild(i).gameObject);
        }

        delay_heatEvent = 0;
        button_GameStart.enabled = true;
        button_Info.enabled = true;
        button_GameQuit.enabled = true;

        
    }

    private void Start()
    {
        if (GameManager.Instance.gameState == GameState.gamePlaying)
        {
            button_GameStart.enabled = false;
            button_Info.enabled = false;
            button_GameQuit.enabled = false;
        }

        if (GameManager.Instance.ActiveStageNum == 11 && GameManager.Instance.gameState == GameState.gamePlaying)
        {
            StartCoroutine(sc0.Ending());
            StartCoroutine(GameManager.Instance.SoundFade(audio_main, 5.0f, 0, 0.2f));
        }
    }

    private void Update()
    {
        delay_heatEvent += Time.deltaTime;
        if(obj_blur.activeSelf == true && delay_heatEvent > 2.5f)
        {
            obj_blur.SetActive(false);
            if(GameManager.Instance.gameState == GameState.idle)
                audio_keyboard.Play();
        }

        if(delay_heatEvent > term_heatEvent)
        {
            OnHeatEvent?.Invoke(this, EventArgs.Empty);
            delay_heatEvent = 0;
            obj_blur.SetActive(true);
            if (GameManager.Instance.gameState == GameState.idle)
                audio_keyboard.Play();
        }

        if(GameManager.Instance.gameState == GameState.ending && Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.Initialize(); //게임 초기화
            GameManager.Instance.LoadSceneByIndex(0);
        }
    }


    public void Open_WorldMap()
    {
        StartCoroutine(FadeOut_SetPanel(0, 0.5f, 0.5f, panel_stageSelect, ()=>StartCoroutine(PlayMiniShipAnim(0.8f))));
    }

    public void Prologue()
    {
        button_GameStart.enabled = false;
        audio_button1.Play();
        GameManager.Instance.gameState = GameState.gamePlaying;
        StartCoroutine(sc0.StartFading());
    }

    public void Open_WorldMap_New()
    {
        StartCoroutine(FadeOut_SetPanel(1.0f, 0.5f, 1.5f, panel_stageSelect, () => StartCoroutine(PlayMiniShipAnim(0.8f))));
    }

    public void Quit()
    {
        if (GameManager.Instance.gameState == GameState.idle)
        {
            button_GameQuit.enabled = false;
            audio_button1.Play();
            #if UNITY_EDITOR
            // 에디터에서 종료를 위해 UnityEditor.EditorApplication.isPlaying을 false로 설정
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            // 빌드된 애플리케이션 종료
            Application.Quit();
            #endif
        }
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

    public IEnumerator FadeToLoadScene(int index, float fadeTime = 0.5f)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime); // 투명도 계산
            img_fade.color = new Color(0, 0, 0, alpha);

            audio_wind.volume = Mathf.Lerp(volume_wind, 0, elapsedTime / fadeTime);
            yield return null;
        }


        img_fade.color = new Color(0, 0, 0, 1); // 완전 불투명
        audio_wind.volume = 0;

        if (coroutine != null)
            coroutine = null;

        term_heatEvent += 10; //오류 방지
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.LoadSceneByIndex(index);
    }

    public IEnumerator FadeToEnding(float fadeTime = 1.0f)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime); // 투명도 계산
            img_fade.color = new Color(0, 0, 0, alpha);

            audio_wind.volume = Mathf.Lerp(volume_wind, 0, elapsedTime / fadeTime);
            yield return null;
        }


        img_fade.color = new Color(0, 0, 0, 1); // 완전 불투명
        audio_wind.volume = 0;

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;

        img_end.gameObject.SetActive(true);
        TextMeshProUGUI tempText = img_end.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime); // 투명도 계산
            img_end.color = new Color(0, 0, 0, alpha);
            tempText.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        img_end.color = new Color(0, 0, 0, 1);
        tempText.color = new Color(1, 1, 1, 1);
        GameManager.Instance.gameState = GameState.ending;
    }

    public IEnumerator FadeOut_SetPanel(float audioFadeTime, float waitTime1, float outTime, Image panel, System.Action onComplete)
    {
        if(audioFadeTime > 0)
            StartCoroutine(GameManager.Instance.SoundFade(audio_main, audioFadeTime, volume_main, 0));

        if (panel != null)
        {
            panel_stageSelect.gameObject.SetActive(true);
            button_miniShip.enabled = false;
            StartCoroutine(PlayMiniShipAnim(0));
        }

        yield return new WaitForSeconds(waitTime1);

        coroutine = StartCoroutine(FadeOut(outTime));
        StartCoroutine(GameManager.Instance.SoundFade(audio_wind, outTime, 0, volume_wind));
        yield return new WaitUntil(() => coroutine == null);

        onComplete?.Invoke();
    }

    public IEnumerator PlayMiniShipAnim(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        switch(GameManager.Instance.ActiveStageNum)
        {
            case 1:
                anim_miniShip.Play("MiniShipMov 1");
                break;
            case 2:
                anim_miniShip.Play("MiniShipMov 1_5");
                break;
            case 3:
                anim_miniShip.Play("MiniShipMov 2");
                break;
            case 4:
                anim_miniShip.Play("MiniShipMov 2_3");
                break;
            case 5:
                anim_miniShip.Play("MiniShipMov 2_6");
                break;
            case 6:
                anim_miniShip.Play("MiniShipMov 3");
                break;
            case 7:
                anim_miniShip.Play("MiniShipMov 3_3");
                break;
            case 8:
                anim_miniShip.Play("MiniShipMov 3_6");
                break;
            case 9:
                anim_miniShip.Play("MiniShipMov 4");
                break;
            case 10:
                anim_miniShip.Play("MiniShipMov 4_5");
                break;
            case 11:
                audio_ShipGo.volume = 1;
                audio_ShipStop.volume = 1;
                anim_miniShip.Play("MiniShipMov 5");
                break;
        }

        if (waitTime == 0)
            anim_miniShip.speed = 0;
        else
        {
            anim_miniShip.speed = 1;
            audio_ShipGo.Play();
        }
    }

    public void ActivateMiniShipButton()
    {
        button_miniShip.enabled = true;
        mark_miniShip.SetActive(true);
    }

    public void LoadStageByButton()
    {
        button_miniShip.enabled = false;
        audio_button2.Play();
        int nextSceneIndex;
        switch(GameManager.Instance.ActiveStageNum)
        {
            case 1:
                nextSceneIndex = 1;
                break;
            case 2:
                nextSceneIndex = 2;
                break;
            case 3:
                nextSceneIndex = 3;
                break;
            case 4:
                nextSceneIndex = 4;
                break;
            case 5:
                nextSceneIndex = 4; //전투
                break;
            case 6:
                nextSceneIndex = 5;
                break;
            case 7:
                nextSceneIndex = 6;
                break;
            case 8:
                nextSceneIndex = 6; //전투
                break;
            case 9:
                nextSceneIndex = 7;
                break;
            case 10:
                nextSceneIndex = 8;
                break;
            case 11:
                nextSceneIndex = 8; //전투
                break;
            default:
                nextSceneIndex = 0;
                break;
        }
        StartCoroutine(FadeToLoadScene(nextSceneIndex));
    }

    public IEnumerator LoadStage()
    {
        yield return new WaitForSeconds(1);
        int nextSceneIndex;
        switch (GameManager.Instance.ActiveStageNum)
        {
            case 1:
                nextSceneIndex = 1;
                break;
            case 2:
                nextSceneIndex = 2;
                break;
            case 3:
                nextSceneIndex = 3;
                break;
            case 4:
                nextSceneIndex = 4;
                break;
            case 5:
                nextSceneIndex = 4; //전투
                break;
            case 6:
                nextSceneIndex = 5;
                break;
            case 7:
                nextSceneIndex = 6;
                break;
            case 8:
                nextSceneIndex = 6; //전투
                break;
            case 9:
                nextSceneIndex = 7;
                break;
            case 10:
                nextSceneIndex = 8;
                break;
            case 11:
                StartCoroutine(FadeToEnding(1.0f));
                yield break;
            default:
                nextSceneIndex = 0; 
                break;
        }
        StartCoroutine(FadeToLoadScene(nextSceneIndex));
    }

    public void SetPrologueImage(int index, bool b)
    {
        if (index >= image_prologue.Length) return;
        image_prologue[index].SetActive(b);
    }

    public void SetEndingImage(int index, bool b)
    {
        if (index >= image_ending.Length) return;
        image_ending[index].SetActive(b);
    }

    public IEnumerator SetEndingImage_Fade(int index, bool b, float fadeTime = 0.5f)
    {
        if (index >= image_ending.Length) yield break;
        float startAlpha, endAlpha;
        if(b)
        {
            image_ending[index].SetActive(true);
            startAlpha = 0;
            endAlpha = 1;
        }
        else
        {
            startAlpha = 1;
            endAlpha = 0;
        }

        float elapsedTime = 0f;

        Image tempImg = image_ending[index].GetComponent<Image>();
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeTime);
            tempImg.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        tempImg.color = new Color(1, 1, 1, endAlpha);
    }

    public void SetEndingImage_Scale(int index, Vector2 targetSize, float duration = 1.5f)
    {
        StartCoroutine(ScaleUI(image_ending[index].GetComponent<RectTransform>(), targetSize, duration));
    }

    IEnumerator ScaleUI(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            target.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            yield return null;
        }

        target.localScale = targetScale;
    }


    public void OpenGuide()
    {
        if(GameManager.Instance.gameState == GameState.idle)
        {
            audio_button1.Play();
            guidePage = 0;
            guideImage[guidePage].SetActive(true);
        }
    }

    public void NextGuide()
    {
        if(guidePage < guideImage.Count - 1)
        {
            audio_button3.Play();
            guideImage[guidePage].SetActive(false);
            guidePage += 1;
            guideImage[guidePage].SetActive(true);
        }
    }

    public void BeforeGuide()
    {
        if (guidePage > 0)
        {
            audio_button3.Play();
            guideImage[guidePage].SetActive(false);
            guidePage -= 1;
            guideImage[guidePage].SetActive(true);
        }
    }

    public void CloseGuide()
    {
        audio_button4.Play();
        guideImage[guidePage].SetActive(false);
    }
}
