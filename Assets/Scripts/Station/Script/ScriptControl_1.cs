using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptControl_1 : ScriptControl
{
    UIController_Station uiController;

    GameObject playerObj;
    Image playerImg;
    public GameObject seniorObj;
    Image seniorImg;
    TutorialNPC senior;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] GameObject playerSpawnPoint;
    protected override void Awake()
    {
        base.Awake();
        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
        uiController = GameObject.Find("Canvas").GetComponent<UIController_Station>();

        playerObj = GameObject.Find("Canvas").transform.Find("InGameUI/img_player").gameObject;
        playerImg = playerObj.GetComponent<Image>();
        seniorImg = GameObject.Find("Canvas").transform.Find("InGameUI/img_senior").GetComponent<Image>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (uiController.GetIsMenuActiveSelf() == true) menuOpened = true;
        else menuOpened = false;

        base.Update();
    }

    protected override void AddTaskByText(int _sentenceIndex, int _listIndex)
    {
        if (_listIndex == 0)
        {
            switch (_sentenceIndex)
            {
                case 0:
                    sentenceSkipPermit = true;
                    ScriptWindow_Up();
                    text_name.text = "사수";
                    break;
                case 3:
                    FixSpeechEffect(0);

                    printDelayTime = 2;
                    playerObj.SetActive(true);
                    StartCoroutine(FadeInImg(playerImg, 1.5f, () => EmptyAction()));
                    break;
                case 4:
                    audio_tut.Play();
                    printDelayTime = 0;
                    break;
                case 5:
                    uiController.FadeTutImg(0, true);
                    break;
                case 6:
                    uiController.FadeTutImg(0, false);
                    break;
                case 7:
                    uiController.FadeTutImg(1, true);
                    break;
                case 8:
                    uiController.FadeTutImg(2, true);
                    break;
                case 9:
                    uiController.FadeTutImg(1, false);
                    uiController.FadeTutImg(2, false);
                    uiController.FadeTutImg(3, true);
                    break;
                case 10:
                    uiController.FadeTutImg(4, true);
                    uiController.FlickTutImg(8, true);
                    break;
                case 11:
                    uiController.FadeTutImg(3, false);
                    uiController.FadeTutImg(4, false);
                    uiController.FlickTutImg(8, false);
                    break;
                case 12:
                    FixSpeechEffect(0);
                    printSpeechEffect = false;

                    printDelayTime = 9f;
                    printSoundFlag = false;
                    sentenceSkipPermit = false; 

                    StartCoroutine(FadeOutImg(seniorImg, 1.0f, () => StartCoroutine(MovImgPos(playerObj, new Vector2(0, -539), 1.0f,
                        () => StartCoroutine(passManager.DoorSetting(2, ()=> StartCoroutine(SetNPC(seniorObj, () => EmptyAction()))))))));
                    ScriptWindow_Left_Guide(500, 200);
                    StartCoroutine(FreezeTutorial());
                    break;
                case 13:
                    speechEffect = seniorObj.transform.Find("speechEffect").gameObject;
                    FixSpeechEffect(1);
                    printSpeechEffect = true;

                    printDelayTime = 0;
                    printSoundFlag = true;
                    sentenceSkipPermit = true;
                    ScriptWindow_Left();
                    break;
                case 17:
                    FixSpeechEffect(0);
                    printSpeechEffect = false;

                    sentenceSkipPermit = false;
                    ScriptWindow_Left_Guide(500, 200);
                    StartCoroutine(OutTutorial());
                    break;
                case 18:
                    FixSpeechEffect(1);
                    printSpeechEffect = true;

                    sentenceSkipPermit = true;
                    ScriptWindow_Left();
                    break;
                case 20:
                    FixSpeechEffect(0);
                    printSpeechEffect = false;

                    sentenceSkipPermit = false;
                    ScriptWindow_Left_Guide(500, 300);
                    StartCoroutine(UnFreezeTutorial());
                    break;
                case 21:
                    FixSpeechEffect(1);
                    printSpeechEffect = true;

                    sentenceSkipPermit = true;
                    ScriptWindow_Left();
                    break;
                case 22:
                    FixSpeechEffect(0);
                    speechEffect = seniorImg.gameObject.transform.Find("speechEffect").gameObject;

                    printDelayTime = 1.8f;
                    ScriptWindow_Up();
                    StartCoroutine(uiController.FadeInAndOut_ConvertPlayerState(0.5f, 0.1f, 0.5f, true));
                    break;
                case 23:
                    printDelayTime = 0;
                    break;
            }
        }
        else if (_listIndex == 1)
        {
            switch (_sentenceIndex)
            {
                case 0:
                    sentenceSkipPermit = true;
                    ScriptWindow_Middle();
                    text_name.text = "사수";
                    uiController.FlickTutImg(9, true);
                    break;
                case 2:
                    uiController.FlickTutImg(9, false);
                    break;
                case 3:
                    ScriptWindow_Up();
                    break;
                case 4:
                    uiController.FadeTutImg(5, true);
                    break;
                case 5:
                    uiController.FadeTutImg(6, true);
                    break;
                case 6:
                    uiController.FadeTutImg(7, true);
                    break;
                case 8:
                    uiController.FadeTutImg(5, false);
                    uiController.FadeTutImg(6, false);
                    uiController.FadeTutImg(7, false);
                    break;
                case 10:
                    FixSpeechEffect(0);

                    printDelayTime = 2f;
                    StartCoroutine(FadeOutImg(playerImg, 1.0f, null));
                    ScriptWindow_Left();
                    charImg.SetActive(false);
                    break;
                case 11:
                    FixSpeechEffect(0);
                    printSpeechEffect = false;

                    printDelayTime = 0f;
                    sentenceSkipPermit = false;
                    ScriptWindow_Left_Guide(500, 200);
                    break;
            }
        }
        else if (_listIndex == 2 || _listIndex == 3 || _listIndex == 4)
        {
            switch (_sentenceIndex)
            {
                case 0:
                    ScriptWindow_Right();
                    text_name.text = "사수";
                    break;
            }
        }
    }

    public override IEnumerator StartFading()
    {
        sentenceList = scriptStore.stDictionary["scene1_0"];
        speechEffect = seniorImg.gameObject.transform.Find("speechEffect").gameObject;
        speechEffect.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        SetText(sentenceList);
        listIndex = 0;

        senior = seniorObj.GetComponent<TutorialNPC>();
        seniorObj.SetActive(false);

        StartCoroutine(uiController.FadeOut(0.5f, 0.5f));
        yield return new WaitForSeconds(1.5f);

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public override IEnumerator BeforeOpenTalk()
    {
        sentenceList = scriptStore.stDictionary["scene1_1"];
        SetText(sentenceList);
        listIndex = 1;

        yield return new WaitForSeconds(0.5f);

        speechEffect = seniorImg.gameObject.transform.Find("speechEffect").gameObject;
        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }


    public IEnumerator Info_Recall()
    {
        StopCoroutine(coroutine);

        audio_bell.Play();
        sentenceList = scriptStore.stDictionary["scene1_recall"];
        SetText(sentenceList);
        listIndex = 2;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = false;
        sentenceSkipPermit = false;

        yield return new WaitUntil(()=> !passManager.gateOpenSign);
        TextTrigger();
    }

    public void Info_Fail()
    {
        if (listIndex == 4) return;
        if (coroutine != null) StopCoroutine(coroutine);

        audio_bell.Play();
        sentenceList = scriptStore.stDictionary["scene1_fail"];
        SetText(sentenceList);
        listIndex = 4;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = false;
        sentenceSkipPermit = false;
    }

    IEnumerator FreezeTutorial()
    {
        missonActive = true;
        yield return new WaitForSeconds(printDelayTime);
        passManager.tut_freeze = true;
        yield return new WaitUntil(() => senior.state == TutNpcState.freeze);
        yield return new WaitForSeconds(1);
        passManager.tut_freeze = false;
        NextText();
        missonActive = false;
    }

    IEnumerator OutTutorial()
    {
        missonActive = true;
        yield return new WaitUntil(() => scriptState == ScriptState.isPrinting);
        yield return new WaitUntil(() => scriptState == ScriptState.isPrinted);
        passManager.tut_InOut = true;
        yield return new WaitUntil(() => passManager.playerState == PlayerState.OutSide);
        playerObj.SetActive(false);
        passManager.tut_InOut = false;
        yield return new WaitForSeconds(1.0f);
        NextText();
        missonActive = false;
    }

    IEnumerator UnFreezeTutorial()
    {
        missonActive = true;
        yield return new WaitUntil(() => scriptState == ScriptState.isPrinting);
        yield return new WaitUntil(() => scriptState == ScriptState.isPrinted);
        passManager.tut_playerAct = true;
        yield return new WaitUntil(() => senior.state == TutNpcState.idle);
        passManager.tut_playerAct = false;
        yield return new WaitForSeconds(1f);
        NextText();
        missonActive = false;
    }


    protected override void LastTaskByText(int _listIndex)
    {
        base.LastTaskByText(_listIndex);

        if (_listIndex == 0)
        {
            touchPermit = false;
            printDelayTime = 0;

            if (seniorObj.activeSelf)
            {
                seniorObj.GetComponent<TutorialNPC>().UnFreeze();
                seniorObj.SetActive(false);
            }
            passManager.SetPlayerToOutSide();
            passManager.StartStageSetting();
            StartCoroutine(GameManager.Instance.SoundFade(audio_tut, 5f, audio_tut.volume, 0));
            StartCoroutine(GameManager.Instance.SoundFade(audio_ambient, 3f, 0, audio_ambient.volume));
            audio_ambient.Play();

            uiController.FadeTutImg(0, false);
            uiController.FadeTutImg(1, false);
            uiController.FadeTutImg(2, false);
            uiController.FadeTutImg(3, false);
            uiController.FadeTutImg(4, false);
            uiController.FlickTutImg(8, false);
        }
        if(_listIndex == 1)
        {
            playerImg.color = new Color(playerImg.color.r, playerImg.color.g, playerImg.color.b, 0);
            passManager.playPermit = true;
            touchPermit = false;
            printDelayTime = 0;
            uiController.ShowOpenGuide();

            uiController.FadeTutImg(5, false);
            uiController.FadeTutImg(6, false);
            uiController.FadeTutImg(7, false);
            uiController.FlickTutImg(9, false);
        }
    }

    IEnumerator SetNPC(GameObject obj, System.Action onComplete)
    {
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(true);
        obj.transform.position = playerSpawnPoint.transform.position;
        yield return new WaitForSeconds(1.0f);
        onComplete?.Invoke();
    }

    IEnumerator MovImgPos(GameObject obj, Vector2 targetPos, float duration, System.Action onComplete)
    {
        yield return new WaitForSeconds(0.5f);
        Vector2 startPosition = obj.GetComponent<RectTransform>().anchoredPosition;
        float elapsedTime = 0f;

        obj.GetComponent<Image>().sprite = playerSprites[0];
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            obj.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPosition, targetPos, t);
            yield return null;
        }

        obj.GetComponent<RectTransform>().anchoredPosition = targetPos; // 최종 위치로 설정
        obj.GetComponent<Image>().sprite = playerSprites[1];
        obj.SetActive(false);
        passManager.convertController(0);
        seniorImg.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.5f);
        onComplete?.Invoke();
    }
}
