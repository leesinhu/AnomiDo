using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class UIController_Station : MonoBehaviour
{
    //Cursor
    Image img_targetMark;

    //InGameUI
    Slider slider_remainTime;
    Image img_passCount_10, img_passCount_1;
    TextMeshProUGUI text_complete;
    [SerializeField]Sprite[] sprite_numbers; //inspector
    Image img_cameraFrame;
    Slider slider_bulletGage;
    TextMeshProUGUI text_bulletCount;
    [SerializeField] Image img_player; //inspector
    [SerializeField] Image img_senior; //inspector
    TextMeshProUGUI text_credit;
    TextMeshProUGUI text_openGuide;
    GameObject obj_interactGuide;
    HorizontalLayoutGroup layout_lines; //냉각포 게이지 선
    List<Image> imges_tut = new List<Image>();
    GameObject[] objs_cameraPoint = new GameObject[4];

    //EndUI
    TextMeshProUGUI text_gameover, text_gameclear;

    //Effect
    GameObject blurEffect;
    Image img_fade;
    RawImage videoTexture;
    public VideoPlayer videoPlayer;

    //Panel
    GameObject obj_pausePanel;
    GameObject obj_gameoverPanel;

    //Etc
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject backGround_heat;
    AudioSource audio_button1, audio_button3, audio_button4;

    PassManager passManager;
    GameObject wayLine;
    Coroutine coroutine = null;

    private void Awake()
    {
        img_targetMark = transform.Find("img_targetMark").GetComponent<Image>();

        slider_remainTime = transform.Find("InGameUI/slider_remainTime").GetComponent<Slider>();
        img_passCount_10 = transform.Find("InGameUI/img_passCount/passCount_10").GetComponent<Image>();
        img_passCount_1 = transform.Find("InGameUI/img_passCount/passCount_1").GetComponent<Image>();
        text_complete = transform.Find("InGameUI/img_passCount/text_complete").GetComponent<TextMeshProUGUI>();
        img_cameraFrame = transform.Find("InGameUI/img_cameraframe").GetComponent<Image>();
        slider_bulletGage = transform.Find("InGameUI/img_cameraframe/slider_bulletGage").GetComponent<Slider>();
        text_bulletCount = transform.Find("InGameUI/img_cameraframe/text_bulletCount").GetComponent<TextMeshProUGUI>();
        text_credit = transform.Find("InGameUI/text_credit").GetComponent<TextMeshProUGUI>();
        text_openGuide = transform.Find("InGameUI/text_openGuide").GetComponent<TextMeshProUGUI>();
        obj_interactGuide = transform.Find("InGameUI/InteractGuide").gameObject;
        layout_lines = transform.Find("InGameUI/img_cameraframe/slider_bulletGage/LineLayout").GetComponent<HorizontalLayoutGroup>();
        audio_button1 = GameObject.Find("Main Camera").transform.Find("Audio_Button1").GetComponent<AudioSource>();
        audio_button3 = GameObject.Find("Main Camera").transform.Find("Audio_Button3").GetComponent<AudioSource>();
        audio_button4 = GameObject.Find("Main Camera").transform.Find("Audio_Button4").GetComponent<AudioSource>();

        Transform parent_tutImg = transform.Find("InGameUI/img_tut");
        foreach (Transform child in parent_tutImg)
        {
            Image img = child.GetComponent<Image>();
            imges_tut.Add(img);
        }

        Transform parent_cameraPoint = transform.Find("InGameUI/img_cameraPoints");
        for(int i=0; i<objs_cameraPoint.Length; i++)
        {
            objs_cameraPoint[i] = parent_cameraPoint.GetChild(i).gameObject;
        }

        text_gameover = transform.Find("EndUI/text_GameOver").GetComponent<TextMeshProUGUI>();
        text_gameover.gameObject.SetActive(false);
        text_gameclear = transform.Find("EndUI/text_GameClear").GetComponent<TextMeshProUGUI>();
        text_gameclear.gameObject.SetActive(false);

        blurEffect = transform.Find("BlurEffect").gameObject;
        img_fade = transform.Find("Effect/img_fade").GetComponent<Image>();
        img_fade.gameObject.SetActive(true);
        img_fade.color = new Color(0, 0, 0, 255);
        videoTexture = transform.Find("Effect/obj_video").GetComponent<RawImage>();
        videoTexture.color = new Color(255, 255, 255, 0);

        obj_pausePanel = transform.Find("Panel/obj_pausePanel").gameObject;
        obj_gameoverPanel = transform.Find("Panel/obj_gameoverPanel").gameObject;

        wayLine = GameObject.Find("WayLine");
    }

    private void Start()
    {
        Debug.Assert(img_targetMark != null, "img_targetMark가 할당되지 않았습니다!");
        Debug.Assert(slider_remainTime != null, "slider_remainTime이 할당되지 않았습니다!");
        Debug.Assert(img_passCount_10 != null, "img_passCount_10이 할당되지 않았습니다!");
        Debug.Assert(img_passCount_1 != null, "img_passCount_1이 할당되지 않았습니다!");
        Debug.Assert(img_cameraFrame != null, "img_cameraFrame이 할당되지 않았습니다!");
        Debug.Assert(slider_bulletGage != null, "slider_bulletGage가 할당되지 않았습니다!");
        Debug.Assert(text_bulletCount != null, "text_bulletCount가 할당되지 않았습니다!");
        Debug.Assert(text_credit != null, "text_credit이 할당되지 않았습니다!");
        Debug.Assert(text_openGuide != null, "text_openGuide가 할당되지 않았습니다!");
        Debug.Assert(text_gameover != null, "text_gameover가 할당되지 않았습니다!");
        Debug.Assert(text_gameclear != null, "text_gameclear가 할당되지 않았습니다!");
        Debug.Assert(img_fade != null, "img_fade가 할당되지 않았습니다!");
        Debug.Assert(videoTexture != null, "videoTexture가 할당되지 않았습니다!");
        Debug.Assert(videoPlayer != null, "videoPlayer가 할당되지 않았습니다!");
        Debug.Assert(obj_pausePanel != null, "obj_pausePanel이 할당되지 않았습니다!");
        Debug.Assert(obj_gameoverPanel != null, "obj_gameoverPanel이 할당되지 않았습니다!");

        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
        HidePassComplete();

        int bulletMaxCount = (int)GameManager.Instance.GetUpgradeElement("Max") / 4;
        layout_lines.padding.left = 200/ bulletMaxCount;
        for(int i=0; i<layout_lines.transform.childCount ; i++)
        {
            if (i < bulletMaxCount - 1)
                layout_lines.transform.GetChild(i).gameObject.SetActive(true);
            else
                layout_lines.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        slider_remainTime.value = (int)passManager.gameTimer / passManager.GameLimitTime;
        img_passCount_10.sprite = sprite_numbers[(passManager.PassGoalCount - passManager.PassCount) / 10];
        img_passCount_1.sprite = sprite_numbers[(passManager.PassGoalCount - passManager.PassCount) % 10];

        slider_bulletGage.value = passManager.GetBulletGage()[0] / passManager.GetBulletGage()[1];
        text_bulletCount.text = "X" + passManager.GetBulletNum().ToString();

        text_credit.text = "Credit : " + passManager.credit.ToString();

        switch (passManager.stageState)
        {
            case StationStageState.Tutorial:
                break;
            case StationStageState.StageSetting:
                Cursor.visible = true;
                img_targetMark.gameObject.SetActive(false);
                break;
            case StationStageState.BeforeOpen:
                break;
            case StationStageState.AfterOpen:
                text_openGuide.gameObject.SetActive(false);
                break;
            case StationStageState.GameClear:
                Cursor.visible = true;
                img_targetMark.gameObject.SetActive(false);
                text_gameclear.gameObject.SetActive(true);
                break;
            case StationStageState.GameOver:
                Cursor.visible = true;
                img_targetMark.gameObject.SetActive(false);
                text_gameover.gameObject.SetActive(true);
                break;
        }

        switch (passManager.playerState)
        {  
            case PlayerState.inSide:
                blurEffect.gameObject.SetActive(true);
                img_cameraFrame.gameObject.SetActive(true);
                if(passManager.stageState != StationStageState.Tutorial && img_senior != null)
                    img_senior.gameObject.SetActive(true);

                Vector2 mousePos = Input.mousePosition;
                if ((mousePos.x >= 0 && mousePos.x <= Screen.width && mousePos.y >= 0 && mousePos.y <= Screen.height)
                    && (passManager.stageState == StationStageState.AfterOpen || passManager.tut_freeze))
                {
                    Cursor.visible = false;
                    img_targetMark.gameObject.SetActive(true);
                    img_targetMark.rectTransform.position = mousePos;
                }
                else
                {
                    Cursor.visible = true;
                    img_targetMark.gameObject.SetActive(false);
                }

                backGround_heat.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                backGround.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);

                wayLine.SetActive(false);

                break;
            case PlayerState.OutSide:
                Cursor.visible = true;
                blurEffect.gameObject.SetActive(false);
                img_targetMark.gameObject.SetActive(false);
                img_cameraFrame.gameObject.SetActive(false);
                if (img_senior != null)
                    img_senior.gameObject.SetActive(false);

                backGround_heat.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
                backGround.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

                wayLine.SetActive(true);

                break;
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    public void GoToMainScreen()
    {
        OpenMenu(false);
        GameManager.Instance.Initialize(); //게임 초기화
        GameManager.Instance.LoadSceneByIndex(0);
    }

    public void Retry()
    {
        OpenGameOverWindow();
    }

    public void OpenMenu(bool b = true)
    {
        if(!obj_pausePanel.gameObject.activeSelf && b)
        {
            audio_button1.Play();
            Cursor.visible = true;
            obj_pausePanel.gameObject.SetActive(true);
        }
        else if(!obj_gameoverPanel.gameObject.activeSelf)
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

    public void CloseOpenGuide()
    {
        text_openGuide.gameObject.SetActive(false);
    }

    public void OpenGameOverWindow()
    {
        if(obj_gameoverPanel != null)
        {
            audio_button3.Play();
            obj_gameoverPanel.gameObject.SetActive(true);
        }
            
    }

    public void CloseGameOverWindow()
    {
        if (obj_gameoverPanel != null)
        {
            obj_gameoverPanel.gameObject.SetActive(false);
            audio_button4.Play();
        }
            
    }

    public void CloseGameOverWindowAndMenu()
    {
        if (obj_gameoverPanel != null)
            obj_gameoverPanel.gameObject.SetActive(false);
        OpenMenu(false);
    }

    public void SetCameraPoints(int index, bool b)
    {
        objs_cameraPoint[index].SetActive(b);
    }

    public IEnumerator FadeInAndOut_ConvertPlayerState(float inTime, float waitTime1, float outTime, bool convertPState = false)
    {
        //ScriptControl_1에서 사용
        coroutine = StartCoroutine(FadeIn(inTime));
        yield return new WaitUntil(() => coroutine == null);

        if(convertPState)
        {
            ConvertPlayerState();
            StartCoroutine(passManager.DoorSetting(3, null));
        }

        yield return new WaitForSeconds(waitTime1);

        coroutine = StartCoroutine(FadeOut(outTime));
        yield return new WaitUntil(() => coroutine == null);
    }

    public IEnumerator FadeInAndOut_ConvertStageState(float inTime, float waitTime1, float outTime, StationStageState state, bool foregroundOpen,
        System.Action onComplete)
    {
        if (inTime > 0)
        {
            coroutine = StartCoroutine(FadeIn(inTime));
            yield return new WaitUntil(() => coroutine == null);
        }

        yield return new WaitForSeconds(waitTime1/2);

        if (foregroundOpen)
        {
            if (obj_pausePanel.activeSelf) OpenMenu(false);

            videoPlayer.Prepare();
            yield return new WaitUntil(() => videoPlayer.isPrepared);
            videoPlayer.Play();
            videoTexture.color = new Color(255, 255, 255, 255);
        }
        else
        {
            if(videoPlayer.isPlaying)
                videoPlayer.Stop();
            videoTexture.color = new Color(255, 255, 255, 0);
        }

        if (passManager.playerState == PlayerState.OutSide)
        {
            ConvertPlayerState();    
        }

        if (state == StationStageState.StageSetting)
        {
            if (img_player != null)
                img_player.color = new Color(1, 1, 1, 1);

            passManager.stageState = state;

            passManager.SetMainCameraPos();
        }
        else if(state == StationStageState.BeforeOpen)
        {
            passManager.stageState = state;
        }

        yield return new WaitForSeconds(waitTime1/2);

        coroutine = StartCoroutine(FadeOut(outTime));
        yield return new WaitUntil(() => coroutine == null);
        onComplete?.Invoke();
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
        if(!b)
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
        while(img.gameObject.activeSelf)
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

        if(coroutine != null)
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
        passManager.playPermit = false;
        coroutine = StartCoroutine(FadeIn(fadeTime));
        StartCoroutine(GameManager.Instance.SoundFade(audio, fadeTime, audio.volume, 0));
        yield return new WaitUntil(() => coroutine == null);

        passManager.enabled = false; //OnDisable 호출 위함
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.LoadSceneByIndex(0);
    }

    public IEnumerator FadeToRestartScene(float fadeTime = 0.5f, AudioSource audio = null)
    {
        passManager.playPermit = false;
        coroutine = StartCoroutine(FadeIn(fadeTime));
        StartCoroutine(GameManager.Instance.SoundFade(audio, fadeTime, audio.volume, 0));
        yield return new WaitUntil(() => coroutine == null);

        passManager.enabled = false; //제대로 종료하기 위함
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.LoadSceneByIndex(GameManager.Instance.GetCurrentSceneIndex());
    }

    public void ConvertPlayerState()
    {
        if (passManager.playerState == PlayerState.inSide)
            passManager.playerState = PlayerState.OutSide;
        else
        {
            passManager.playerState = PlayerState.inSide;
            passManager.SetPlayerToOutSide();

            GameObject senior = GameObject.Find("Senior");
            if (senior != null)
                senior.SetActive(false);

            GameObject playerUiObj = GameObject.Find("Canvas").transform.Find("InGameUI/img_player").gameObject;
            playerUiObj.SetActive(true);
            playerUiObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(-289, -539, 0);
            playerUiObj.GetComponent<Image>().color = new Color(255, 255, 255, 255);

            GameObject seniorUiObj = GameObject.Find("Canvas").transform.Find("InGameUI/img_senior").gameObject;
            seniorUiObj.SetActive(true);
            seniorUiObj.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
    }

    public void ShowOpenGuide()
    {
        text_openGuide.gameObject.SetActive(true);
    }

    public void ShowInteractGuide(bool b)
    {
        if(b)
        {
            obj_interactGuide.SetActive(true);
        }
        else
        {
            obj_interactGuide.SetActive(false);
        }
    }
    
    public void ShowPassComplete()
    {
        Color tempColor = text_complete.color;
        tempColor.a = 1f;  // 알파값 1로 설정 (완전 불투명)
        text_complete.color = tempColor;

        img_passCount_10.color = new Color(1, 1, 1, 0);
        img_passCount_1.color = new Color(1, 1, 1, 0);
    }

    public void HidePassComplete()
    {
        Color tempColor = text_complete.color;
        tempColor.a = 0f;  // 알파값 1로 설정 (완전 불투명)
        text_complete.color = tempColor;

        img_passCount_10.color = new Color(1, 1, 1, 1);
        img_passCount_1.color = new Color(1, 1, 1, 1);
    }
}
