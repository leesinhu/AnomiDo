using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptControl_2 : ScriptControl
{
    UiController_InFlight uiController;
    PlayerController playerController;

    [SerializeField] GameObject obj_senior, obj_red, obj_orange, obj_yellow;
    [SerializeField] GameObject obj_labber, obj_bed;
    SymbolAnimation mark_senior, mark_red, mark_orange, mark_yellow, mark_labber, mark_bed;

    [SerializeField] GameObject touchLimitPanel;

    protected override void Awake()
    {
        base.Awake();
        shipManager = GameObject.Find("ShipManager").GetComponent<ShipManager>();
        uiController = GameObject.Find("Canvas").GetComponent<UiController_InFlight>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        flowIndex = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        mark_senior = obj_senior.GetComponentInChildren<SymbolAnimation>();
        mark_red = obj_red.GetComponentInChildren<SymbolAnimation>();
        mark_orange = obj_orange.GetComponentInChildren<SymbolAnimation>();
        mark_yellow = obj_yellow.GetComponentInChildren<SymbolAnimation>();
        mark_labber = obj_labber.GetComponentInChildren<SymbolAnimation>();
        mark_bed = obj_bed.GetComponentInChildren<SymbolAnimation>();

        mark_red.ConvertMark(2);
        mark_orange.ConvertMark(2);
        mark_yellow.ConvertMark(2);

        mark_senior.SetMark(false);
        mark_red.SetMark(false);
        mark_orange.SetMark(false);
        mark_yellow.SetMark(false);
        mark_labber.SetMark(false);
        mark_bed.SetMark(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (uiController.GetIsMenuActiveSelf() == true) menuOpened = true;
        else menuOpened = false;

        base.Update();
    }

    public void OpenScript(string name)
    {
        switch (name)
        {
            case "Senior":
                if (mark_senior.symbolState == SymbolState.none)
                    return;

                mark_senior.SetMark(false);
                if(flowIndex == 1)
                {
                    sentenceList = scriptStore.stDictionary["scene2_senior"];
                    SetText(sentenceList);
                    listIndex = 1;
                }
                else if(flowIndex == 2)
                {
                    sentenceList = scriptStore.stDictionary["scene2_senior_2"];
                    SetText(sentenceList);
                    listIndex = 2;
                }
                else if(flowIndex == 4)
                {
                    sentenceList = scriptStore.stDictionary["scene2_3"];
                    SetText(sentenceList);
                    listIndex = 5;
                }
                else if(flowIndex == 5)
                {
                    sentenceList = scriptStore.stDictionary["scene2_senior_3"];
                    SetText(sentenceList);
                    listIndex = 9;
                }
                else if (flowIndex == 6)
                {
                    sentenceList = scriptStore.stDictionary["scene2_senior_4"];
                    SetText(sentenceList);
                    listIndex = 12;
                }
                else
                {
                    return;
                }
                speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                break;
            case "Red":
                if (mark_red.symbolState == SymbolState.none)
                    return;

                if (mark_red.symbolState == SymbolState.green)
                {
                    sentenceList = scriptStore.stDictionary["scene2_red"];
                    SetText(sentenceList);
                    listIndex = 6;
                }
                    
                speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                break;
            case "Orange":
                if (mark_orange.symbolState == SymbolState.none)
                    return;

                if (mark_orange.symbolState == SymbolState.green)
                {
                    sentenceList = scriptStore.stDictionary["scene2_orange"];
                    SetText(sentenceList);
                    listIndex = 7;
                }
                else //yellow mark
                {
                    sentenceList = scriptStore.stDictionary["scene2_orange2"];
                    SetText(sentenceList);
                    listIndex = 10;
                }

                speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;
                break;
            case "Yellow":
                if (mark_yellow.symbolState == SymbolState.none)
                    return;

                if (mark_yellow.symbolState == SymbolState.green)
                {
                    sentenceList = scriptStore.stDictionary["scene2_yellow"];
                    SetText(sentenceList);
                    listIndex = 8;
                }
                else //yellow mark
                {
                    sentenceList = scriptStore.stDictionary["scene2_yellow2"];
                    SetText(sentenceList);
                    listIndex = 11;
                }

                speechEffect = obj_yellow.transform.Find("SpeechEffect").gameObject;
                break;
        }

        shipManager.movePermit = false;
        shipManager.interactPermit = false;
        printSpeechEffect = true;
        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    protected override void AddTaskByText(int _sentenceIndex, int _listIndex)
    {
        switch (_listIndex)
        {
            case 0:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        break;
                    case 6:
                        uiController.FadeTutImg(0, true);
                        obj_senior.transform.position = new Vector2(-2, -4.25f);
                        FixSpeechEffect(0);
                        printSpeechEffect = false;

                        ScriptWindow_Left_Guide();
                        break;
                    case 7:
                        uiController.FadeTutImg(0, false);
                        uiController.FadeTutImg(1, true);
                        break;
                }
                break;
            case 1:
                switch (_sentenceIndex)
                {
                    case 0:
                        playerController.DeactiveNav();
                        obj_senior.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);

                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        break;
                    case 2:
                        FixSpeechEffect(0);
                        printSpeechEffect = false;

                        ScriptWindow_Left_Guide();
                        break;
                }
                break;
            case 2:
                switch (_sentenceIndex)
                {
                    case 0:
                        obj_senior.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 3:
                switch (_sentenceIndex)
                {
                    case 0:
                        mark_senior.SetMark(false);
                        sentenceSkipPermit = true;
                        printSpeechEffect = false;
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        break;
                    case 2:
                        uiController.FlickTutImg(2, true);
                        break;
                    case 3:
                        ScriptWindow_Up();
                        uiController.FlickTutImg(2, false);
                        uiController.FlickTutImg(3, true);
                        break;
                    case 4:
                        uiController.FlickTutImg(3, false);
                        break;
                    case 5:
                        ScriptWindow_Down();
                        uiController.FlickTutImg(4, true);
                        break;
                    case 7:
                        uiController.FlickTutImg(4, false);
                        break;
                    case 8:
                        ScriptWindow_Left_Guide();
                        break;
                }
                break;
            case 4:
                switch (_sentenceIndex)
                {
                    case 0:
                        mark_senior.SetMark(false);
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 5:
                switch (_sentenceIndex)
                {
                    case 0:
                        playerController.DeactiveNav();

                        mark_senior.SetMark(false);
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";

                        break;
                    case 1:
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        scriptWindow.GetComponent<Image>().sprite = windows[3];
                        text_name.text = "½Â°´ R";
                        break;
                    case 2:
                        FixSpeechEffect(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        scriptWindow.GetComponent<Image>().sprite = windows[0];
                        text_name.text = "»ç¼ö";
                        break;
                    case 3:
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        scriptWindow.GetComponent<Image>().sprite = windows[3];
                        text_name.text = "½Â°´ R";
                        break;
                    case 4:
                        FixSpeechEffect(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        scriptWindow.GetComponent<Image>().sprite = windows[0];
                        text_name.text = "»ç¼ö";
                        break;
                    case 5:
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        scriptWindow.GetComponent<Image>().sprite = windows[3];
                        text_name.text = "½Â°´ R";
                        break;
                    case 6:
                        FixSpeechEffect(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        scriptWindow.GetComponent<Image>().sprite = windows[0];
                        text_name.text = "»ç¼ö";
                        break;
                    case 7:
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        scriptWindow.GetComponent<Image>().sprite = windows[3];
                        text_name.text = "½Â°´ R";
                        break;
                    case 8:
                        FixSpeechEffect(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        scriptWindow.GetComponent<Image>().sprite = windows[0];
                        text_name.text = "»ç¼ö";
                        break;
                    case 9:
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        scriptWindow.GetComponent<Image>().sprite = windows[3];
                        text_name.text = "½Â°´ R";
                        break;
                    case 10:
                        FixSpeechEffect(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        printDelayTime = 0.75f;
                        scriptWindow.GetComponent<Image>().sprite = windows[0];
                        text_name.text = "»ç¼ö";
                        break;
                    case 11:
                        FixSpeechEffect(0);
                        printDelayTime = 0;
                        obj_senior.GetComponent<RotatableObj>().ConvertSprite(6);
                        break;
                }
                break;
            case 6:
                switch (_sentenceIndex)
                {
                    case 0:
                        obj_red.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        mark_red.SetMark(false);
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "½Â°´ R";
                        scriptWindow.GetComponent<Image>().sprite = windows[3];
                        break;
                    case 2:
                        FixSpeechEffect(0);
                        printDelayTime = 0.5f;
                        break;
                }
                break;
            case 7:
                switch (_sentenceIndex)
                {
                    case 0:
                        obj_orange.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        mark_orange.SetMark(false);
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "½Â°´ O";
                        scriptWindow.GetComponent<Image>().sprite = windows[4];
                        break;
                }
                break;
            case 8:
                switch (_sentenceIndex)
                {
                    case 0:
                        obj_yellow.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        mark_yellow.SetMark(false);
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "½Â°´ Y";
                        scriptWindow.GetComponent<Image>().sprite = windows[5];
                        break;
                }
                break;
            case 9:
                switch (_sentenceIndex)
                {
                    case 0:
                        shipManager.SetDoor(15, false);
                        mark_senior.SetMark(false);
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        scriptWindow.GetComponent<Image>().sprite = windows[0];
                        break;
                    case 1:
                        FixSpeechEffect(0);
                        printDelayTime = 0.75f;
                        break;
                    case 2:
                        printDelayTime = 0;
                        break;
                }
                break;
            case 10:
                switch (_sentenceIndex)
                {
                    case 0:
                        mark_orange.SetMark(false);
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "½Â°´ O";
                        scriptWindow.GetComponent<Image>().sprite = windows[4];
                        break;
                }
                break;
            case 11:
                switch (_sentenceIndex)
                {
                    case 0:
                        mark_yellow.SetMark(false);
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "½Â°´ Y";
                        scriptWindow.GetComponent<Image>().sprite = windows[5];
                        break;
                }
                break;
            case 12:
                switch (_sentenceIndex)
                {
                    case 0:
                        mark_senior.SetMark(false);
                        sentenceSkipPermit = true;
                        ScriptWindow_Left_Guide();
                        printSpeechEffect = false;
                        break;
                }
                break;
        }
    }

    protected override void LastTaskByText(int _listIndex)
    {
        base.LastTaskByText(_listIndex);
        
        switch (_listIndex)
        {
            case 0:
                touchPermit = false;
                printDelayTime = 0;
                obj_senior.transform.position = new Vector2(-3, -4.25f);
                playerController.ActivateNav(obj_senior.transform.position);
                mark_senior.SetMark(true);

                uiController.FadeTutImg(0, false);
                uiController.FadeTutImg(1, false);
                flowIndex = 1;
                break;
            case 1:
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(7);
                flowIndex = 2;
                mark_labber.SetMark(true);
                mark_senior.SetMark(true);
                mark_senior.ConvertMark(0);
                shipManager.SetDoor(11, true);
                break;
            case 2:
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(7);
                mark_senior.SetMark(true);
                break;
            case 3:
                touchLimitPanel.SetActive(false);
                mark_labber.ConvertMark(0);
                flowIndex = 3;

                uiController.FlickTutImg(2, false);
                uiController.FlickTutImg(3, false);
                uiController.FlickTutImg(4, false);
                shipManager.LastSentenceEnd(false, false);
                return;
            case 4:
                mark_senior.SetMark(true);
                mark_senior.ConvertMark(1);
                obj_senior.transform.position = new Vector2(-14, -3.5f);
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(4);
                playerController.ActivateNav(obj_senior.transform.position);
                flowIndex = 4;

                obj_red.SetActive(true);
                break;
            case 5:
                mark_senior.SetMark(true);
                mark_senior.ConvertMark(1);
                mark_red.SetMark(true);
                obj_senior.transform.position = new Vector2(26.48f, 6.71f);
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(0);
                flowIndex = 5;

                obj_orange.SetActive(true);
                mark_orange.SetMark(true);
                obj_yellow.SetActive(true);
                mark_yellow.SetMark(true);
                break;
            case 6:
                mark_red.SetMark(false);
                obj_red.transform.position = new Vector2(-1.55f, 17.29f);
                obj_red.GetComponent<RotatableObj>().ConvertSprite(6);
                StartCoroutine(uiController.AddCredit(1));
                break;
            case 7:
                mark_orange.SetMark(true);
                mark_orange.ConvertMark(0);
                obj_orange.GetComponent<RotatableObj>().ConvertSprite(6);
                StartCoroutine(uiController.AddCredit(3));
                break;
            case 8:
                mark_yellow.SetMark(true);
                mark_yellow.ConvertMark(0);
                obj_yellow.GetComponent<RotatableObj>().ConvertSprite(5);
                StartCoroutine(uiController.AddCredit(6));
                break;
            case 9:
                mark_senior.SetMark(true);
                mark_senior.ConvertMark(0);
                mark_bed.SetMark(true);
                flowIndex = 6;
                break;
            case 10:
                mark_orange.SetMark(true);
                obj_orange.GetComponent<RotatableObj>().ConvertSprite(6);
                break;
            case 11:
                mark_yellow.SetMark(true);
                obj_yellow.GetComponent<RotatableObj>().ConvertSprite(5);
                break;
            case 12:
                mark_senior.SetMark(true);
                break;
        }
        shipManager.LastSentenceEnd();
    }

    public override IEnumerator StartFading()
    {
        shipManager.interactPermit = false;
        shipManager.movePermit = false;

        sentenceList = scriptStore.stDictionary["scene2_0"];
        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
        playerController.SetDirectionByOther(new Vector2(-1, 1));

        SetText(sentenceList);
        listIndex = 0;

        StartCoroutine(uiController.FadeOut(1.5f, 0.5f));
        StartCoroutine(GameManager.Instance.SoundFade(shipManager.audio_main, 1.5f, 0, shipManager.audio_main.volume));
        yield return new WaitForSeconds(1.5f);

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public IEnumerator CoolingCannonTut()
    {
        touchLimitPanel.SetActive(true);
        obj_senior.transform.position = new Vector2(0, -4.5f);
        obj_senior.GetComponent<RotatableObj>().ConvertSprite(2);

        shipManager.interactPermit = false;
        shipManager.movePermit = false;

        sentenceList = scriptStore.stDictionary["scene2_1"];
        SetText(sentenceList);
        listIndex = 3;

        yield return new WaitForSeconds(1.2f);
        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public IEnumerator AfterCoolingCannonTut()
    {
        shipManager.interactPermit = false;
        shipManager.movePermit = false;
        playerController.SetDirectionByOther(new Vector2(0, -1));

        sentenceList = scriptStore.stDictionary["scene2_2"];
        SetText(sentenceList);
        listIndex = 4;

        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
        printSpeechEffect = true;

        yield return new WaitForSeconds(0.7f);
        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }
    IEnumerator SetNPC(GameObject obj, System.Action onComplete)
    {
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        onComplete?.Invoke();
    }

    IEnumerator MovImgPos(GameObject obj, Vector2 targetPos, float duration, System.Action onComplete)
    {
        yield return new WaitForSeconds(0.5f);
        Vector2 startPosition = obj.GetComponent<RectTransform>().anchoredPosition;
        float elapsedTime = 0f;

        obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/0");
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            obj.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPosition, targetPos, t);
            yield return null;
        }

        obj.GetComponent<RectTransform>().anchoredPosition = targetPos; // ÃÖÁ¾ À§Ä¡·Î ¼³Á¤
        obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/90");
        yield return new WaitForSeconds(0.5f);
        onComplete?.Invoke();
    }
}
