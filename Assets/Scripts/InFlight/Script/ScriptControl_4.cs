using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptControl_4 : ScriptControl
{
    UiController_InFlight uiController;
    PlayerController playerController;

    [SerializeField] GameObject obj_senior, obj_red, obj_orange, obj_yellow, obj_green, obj_white;
    [SerializeField] GameObject obj_labber, obj_bed, obj_door, obj_cargo;
    SymbolAnimation mark_senior, mark_red, mark_orange, mark_yellow, mark_green, mark_white,
        mark_labber, mark_bed, mark_door, mark_cargo;

    [SerializeField] GameObject touchLimitPanel;
    [SerializeField] AudioSource audio_box;
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
        mark_green = obj_green.GetComponentInChildren<SymbolAnimation>();
        mark_white = obj_white.GetComponentInChildren<SymbolAnimation>();
        mark_labber = obj_labber.GetComponentInChildren<SymbolAnimation>();
        mark_bed = obj_bed.GetComponentInChildren<SymbolAnimation>();
        mark_door = obj_door.GetComponentInChildren<SymbolAnimation>();
        mark_cargo = obj_cargo.GetComponentInChildren<SymbolAnimation>();

        mark_red.ConvertMark(2);
        mark_orange.ConvertMark(2);
        mark_yellow.ConvertMark(2);
        mark_green.ConvertMark(2);
        mark_white.ConvertMark(2);

        mark_senior.SetMark(true);
        mark_red.SetMark(false);
        mark_orange.SetMark(false);
        mark_yellow.SetMark(false);
        mark_green.SetMark(false);
        mark_white.SetMark(false);
        mark_labber.SetMark(false);
        mark_bed.SetMark(false);
        mark_door.SetMark(false);
        mark_cargo.SetMark(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (uiController.GetIsMenuActiveSelf() == true) menuOpened = true;
        else menuOpened = false;

        base.Update();
        if(flowIndex == 4)
        {
            if(playerController.transform.position.x < 19)
            {
                StartCoroutine(FightTut());
                flowIndex = 5;
            }
        }
        else if(flowIndex == 5)
        {
            if (shipManager.enemyRemainCount == 6)
            {
                FightTut2();
                flowIndex = 6;
            }
        }
        else if(flowIndex == 6)
        {
            if (shipManager.enemyRemainCount == 0)
            {
                AfterFight();
                flowIndex = 7;
            }
        }
    }

    public void OpenScript(string name)
    {
        switch (name)
        {
            case "Senior":
                if (mark_senior.symbolState == SymbolState.none)
                    return;

                mark_senior.SetMark(false);
                if(flowIndex == 0)
                {
                    obj_senior.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene4_senior"];
                    SetText(sentenceList);
                    listIndex = 0;
                }
                else if(flowIndex == 1)
                {
                    obj_senior.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene4_senior_2"];
                    SetText(sentenceList);
                    listIndex = 3;
                }
                else if (flowIndex == 2)
                {
                    sentenceList = scriptStore.stDictionary["scene4_senior_3"];
                    SetText(sentenceList);
                    listIndex = 8;
                }
                else if (flowIndex == 3)
                {
                    sentenceList = scriptStore.stDictionary["scene4_senior_4"];
                    SetText(sentenceList);
                    listIndex = 9;
                }
                else if(flowIndex == 6)
                {
                    obj_senior.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene4_senior_7"];
                    SetText(sentenceList);
                    listIndex = 12;
                }
                else if(flowIndex == 7)
                {
                    obj_senior.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene4_senior_8"];
                    SetText(sentenceList);
                    listIndex = 13;
                }
                else
                {
                    return;
                }
                speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                break;
            case "Green":   
                obj_green.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                scriptWindow.GetComponent<Image>().sprite = windows[6];
                speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;

                sentenceList = scriptStore.stDictionary["scene4_green"];
                SetText(sentenceList);
                listIndex = 4;

                mark_green.SetMark(false);
                break;
            case "Orange":               
                scriptWindow.GetComponent<Image>().sprite = windows[4];
                speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;

                sentenceList = scriptStore.stDictionary["scene4_orange"];
                SetText(sentenceList);
                listIndex = 5;

                mark_orange.SetMark(false);
                break;
            case "White":     
                scriptWindow.GetComponent<Image>().sprite = windows[7];
                speechEffect = obj_white.transform.Find("SpeechEffect").gameObject;

                if (mark_white.symbolState == SymbolState.green)
                {
                    obj_white.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene4_white"];
                    SetText(sentenceList);
                    listIndex = 6;
                }
                else //yellow mark
                {
                    sentenceList = scriptStore.stDictionary["scene4_white2"];
                    SetText(sentenceList);
                    listIndex = 7;
                }
                mark_white.SetMark(false);
                break;
            case "Red":
                scriptWindow.GetComponent<Image>().sprite = windows[3];
                speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;

                if (mark_red.symbolState == SymbolState.green)
                {
                    obj_red.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene4_red"];
                    SetText(sentenceList);
                    listIndex = 14;
                }
                else //yellow mark
                {
                    sentenceList = scriptStore.stDictionary["scene4_red2"];
                    SetText(sentenceList);
                    listIndex = 15;
                }
                mark_red.SetMark(false);
                break;
            case "Yellow":
                scriptWindow.GetComponent<Image>().sprite = windows[5];
                speechEffect = obj_yellow.transform.Find("SpeechEffect").gameObject;

                if (mark_yellow.symbolState == SymbolState.green)
                {
                    obj_yellow.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene4_yellow"];
                    SetText(sentenceList);
                    listIndex = 16;
                }
                else //yellow mark
                {
                    sentenceList = scriptStore.stDictionary["scene4_yellow2"];
                    SetText(sentenceList);
                    listIndex = 17;
                }
                mark_yellow.SetMark(false);
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
            case 0: //Ã³À½. »ç¼ö¿ÍÀÇ ´ëÈ­
                switch (_sentenceIndex)
                {
                    case 0: 
                        sentenceSkipPermit = true;
                        scriptWindow.GetComponent<Image>().sprite = windows[0];
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 1: //Ã¢°í ¹® ¿­±â
                switch (_sentenceIndex)
                {
                    case 0: 
                        playerController.DeactiveNav();
                        sentenceSkipPermit = true;
                        ScriptWindow_Right(false);

                        mark_door.SetMark(false);
                        shipManager.SetDoor(14, true);
                        break;
                }
                break;
            case 2: //Ã¢°í¿¡¼­ ¹°Ç° ²¨³»±â
                switch (_sentenceIndex)
                {
                    case 0:
                        audio_box.Play();
                        sentenceSkipPermit = true;
                        ScriptWindow_Right(false);

                        mark_cargo.SetMark(false);
                        break;
                }
                break;
            case 3: //³Ã°¢Æ÷ ¾Õ ´ëÈ­
                switch (_sentenceIndex)
                {
                    case 0:
                        playerController.DeactiveNav();
                        sentenceSkipPermit = true;
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);

                        break;
                    case 1:
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        printSpeechEffect = true;
                        ScriptWindow_Down();
                        break;
                    case 2:
                        printDelayTime = 1.75f;
                        FixSpeechEffect(0);
                        StartCoroutine(uiController.FadeInAndOut(0.5f, 0.2f, 0.5f, null));
                        break;
                    case 3:
                        printDelayTime = 0;
                        break;
                }
                break;
            case 4:
                switch (_sentenceIndex)
                {
                    case 0:
                        text_name.text = "½Â°´ G";
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(6);
                        break;
                    case 3:
                        FixSpeechEffect(0);
                        speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ O";
                        ScriptWindow_Down(4);
                        break;
                    case 4:
                        FixSpeechEffect(0);
                        speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;
                        obj_green.GetComponent<RotatableObj>().ConvertSprite(3);
                        text_name.text = "½Â°´ G";
                        ScriptWindow_Down(6);
                        break;
                    case 6:
                        FixSpeechEffect(0);
                        speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ O";
                        ScriptWindow_Down(4);
                        break;
                    case 7:
                        FixSpeechEffect(0);
                        speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ G";
                        ScriptWindow_Down(6);
                        break;
                    case 8:
                        FixSpeechEffect(0);
                        speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ O";
                        ScriptWindow_Down(4);
                        break;
                    case 9:
                        FixSpeechEffect(0);
                        speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ G";
                        ScriptWindow_Down(6);
                        break;
                }
                break;
            case 6:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(7);
                        text_name.text = "½Â°´ W";
                        break;
                }
                break;
            case 7:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(7);
                        text_name.text = "½Â°´ W";
                        break;
                }
                break;
            case 8: //Àáµé ±â Àü »ç¼ö¿Í ´ëÈ­
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        scriptWindow.GetComponent<Image>().sprite = windows[0];
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";

                        break;
                }
                break;
            case 9:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Right(false);
                        break;
                }
                break;
            case 10:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 11:
                switch(_sentenceIndex)
                {
                    case 0:
                        playerController.DeactiveNav();
                        obj_senior.GetComponent<RotatableObj>().ConvertSprite(0);

                        ScriptWindow_Down(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "»ç¼ö";
                        break;
                    case 2:
                        uiController.FadeTutImg(0, true);
                        break;
                    case 3:
                        uiController.FadeTutImg(1, true);
                        break;
                    case 4:
                        uiController.FadeTutImg(0, false);
                        uiController.FadeTutImg(1, false);
                        break;
                    case 5:
                        uiController.FadeTutImg(2, true);
                        break;
                    case 6:
                        uiController.FadeTutImg(3, true);
                        break;
                    case 7:
                        uiController.FadeTutImg(4, true);
                        break;
                    case 8:
                        uiController.FadeTutImg(2, false);
                        uiController.FadeTutImg(3, false);
                        uiController.FadeTutImg(4, false);
                        uiController.FlickTutImg(5, true);
                        break;
                    case 10:
                        uiController.FlickTutImg(5, false);
                        break;
                    case 11:
                        obj_senior.transform.position = new Vector2(14.32f, -35f); //È­¸é¹Ù±ùÀ¸·Î(ÀüÅõ ½ÃÀÛ)
                        sentenceSkipPermit = false;

                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        ScriptWindow_Left_Guide();
                        break;
                }
                break;
            case 12:
                switch (_sentenceIndex)
                {
                    case 0:
                        playerController.DeactiveNav();
                        ScriptWindow_Down(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 13:
                switch (_sentenceIndex)
                {
                    case 0:
                        playerController.DeactiveNav();
                        ScriptWindow_Down(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 14:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(3);
                        text_name.text = "½Â°´ R";
                        break;
                }
                break;
            case 15:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(3);
                        text_name.text = "½Â°´ R";
                        break;
                }
                break;
            case 16:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(5);
                        text_name.text = "½Â°´ Y";
                        break;
                }
                break;
            case 17:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(5);
                        text_name.text = "½Â°´ Y";
                        break;
                }
                break;
        }
    }

    protected override void LastTaskByText(int _listIndex)
    {
        base.LastTaskByText(_listIndex);
        printDelayTime = 0;

        switch (_listIndex)
        {
            case 0:
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(7);
                obj_senior.transform.position = new Vector2(-3, -4.25f);    

                playerController.ActivateNav(obj_door.transform.position);
                mark_door.SetMark(true);
                break;
            case 1:
                mark_cargo.SetMark(true);
                break;
            case 2:
                shipManager.SetDoor(12, true);
                shipManager.SetDoor(13, true);
                shipManager.SetDoor(15, true);

                playerController.ActivateNav(obj_senior.transform.position);
                mark_senior.SetMark(true);

                flowIndex = 1;
                break;
            case 3:
                shipManager.SetDoor(11, true);
                mark_labber.SetMark(true);
                
                obj_senior.transform.position = new Vector2(26.48f, 6.71f);
                mark_labber.ConvertMark(0);
                mark_senior.SetMark(true);

                mark_red.SetMark(true);
                mark_yellow.SetMark(true);
                mark_green.SetMark(true);
                mark_white.SetMark(true);

                flowIndex = 2;
                break;
            case 4:
                mark_orange.SetMark(true);
                mark_orange.ConvertMark(0);

                mark_green.SetMark(false);
                obj_green.transform.position = new Vector2(2, -4.5f);
                StartCoroutine(uiController.AddCredit(3));
                break;
            case 5:
                mark_orange.SetMark(true);
                mark_orange.ConvertMark(0);
                break;
            case 6:
                obj_white.GetComponent<RotatableObj>().ConvertSprite(6);
                mark_white.SetMark(true);
                mark_white.ConvertMark(0);
                StartCoroutine(uiController.AddCredit(5));
                break;
            case 7:
                mark_white.SetMark(true);
                mark_white.ConvertMark(0);
                break;
            case 8:
                mark_senior.SetMark(true);
                mark_senior.ConvertMark(0);
                mark_bed.SetMark(true);
                flowIndex = 3;

                shipManager.SetDoor(13, false);
                shipManager.SetDoor(14, false);
                shipManager.SetDoor(15, false);
                break;
            case 9:
                mark_senior.SetMark(true);
                mark_senior.ConvertMark(0);
                break;
            case 10:
                obj_senior.transform.position = new Vector2(14.32f, -14.09f);
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(2);
                mark_senior.SetMark(true);
                playerController.ActivateNav(obj_senior.transform.position);
                flowIndex = 4;

                //ÀüÅõ BGM
                StartCoroutine(GameManager.Instance.SoundFade(shipManager.audio_fight, 1, 0, shipManager.volume_fight));
                break;
            case 11:
                uiController.FadeTutImg(0, false);
                uiController.FadeTutImg(1, false);
                uiController.FadeTutImg(2, false);
                uiController.FadeTutImg(3, false);
                uiController.FadeTutImg(4, false);
                uiController.FlickTutImg(5, false);
                SetObjOutside(obj_senior); //È­¸é¹Ù±ùÀ¸·Î(ÀüÅõ ½ÃÀÛ)
                break;
            case 12:
                SetObjOutside(obj_senior);
                shipManager.SetDoor(12, true);
                break;
            case 13:
                shipManager.Sleep();
                return;
            case 14:
                obj_red.GetComponent<RotatableObj>().ConvertSprite(0);
                mark_red.SetMark(true);
                mark_red.ConvertMark(0);
                StartCoroutine(uiController.AddCredit(2));
                break;
            case 15:
                mark_red.SetMark(true);
                mark_red.ConvertMark(0);
                break;
            case 16:
                obj_yellow.GetComponent<RotatableObj>().ConvertSprite(6);
                mark_yellow.SetMark(true);
                mark_yellow.ConvertMark(0);
                StartCoroutine(uiController.AddCredit(4));
                break;
            case 17:
                mark_yellow.SetMark(true);
                mark_yellow.ConvertMark(0);
                break;
        }
        shipManager.LastSentenceEnd();
    }

    public override IEnumerator StartFading()
    {
        shipManager.interactPermit = false;
        shipManager.movePermit = false;

        StartCoroutine(uiController.FadeOut(1.5f, 0.5f));
        StartCoroutine(GameManager.Instance.SoundFade(shipManager.audio_main, 1.5f, 0, shipManager.audio_main.volume));
        yield return new WaitForSeconds(1.5f);

        shipManager.interactPermit = true;
        shipManager.movePermit = true;
        touchPermit = true;
    }

    public IEnumerator StartFading_Fight()
    {
        shipManager.interactPermit = false;
        shipManager.movePermit = false;

        obj_red.SetActive(false);
        obj_orange.SetActive(false);
        obj_yellow.SetActive(false);
        obj_green.SetActive(false);
        obj_white.SetActive(false);

        mark_senior.SetMark(false);
        sentenceList = scriptStore.stDictionary["scene4_senior_5"];
        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
        playerController.transform.position = new Vector2(26.62f, -6.57f);
        obj_senior.transform.position = new Vector2(28.4f, -6.57f);
        obj_senior.GetComponent<RotatableObj>().ConvertSprite(4);

        SetText(sentenceList);
        listIndex = 10;

        StartCoroutine(uiController.FadeOut(1.5f, 0.5f));
        yield return new WaitForSeconds(1.5f);

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    IEnumerator FightTut()
    {
        shipManager.interactPermit = false;
        shipManager.movePermit = false;

        StartCoroutine(uiController.FadeIn(0.3f));
        yield return new WaitForSeconds(0.3f);

        yield return new WaitForSeconds(0.1f);
        playerController.transform.position = new Vector2(16.32f, -14.09f);
        mark_senior.SetMark(false);

        StartCoroutine(uiController.FadeOut(0.3f));
        yield return new WaitForSeconds(1f);

        sentenceList = scriptStore.stDictionary["scene4_senior_6"];
        SetText(sentenceList);
        listIndex = 11;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    void FightTut2()
    {
        obj_senior.transform.position = new Vector2(9, 9.5f);
        obj_senior.GetComponent<RotatableObj>().ConvertSprite(7);
        mark_senior.SetMark(true);
        playerController.ActivateNav(obj_senior.transform.position);
    }

    void AfterFight()
    {
        obj_senior.transform.position = new Vector2(21, 10);
        obj_senior.GetComponent<RotatableObj>().ConvertSprite(6);
        mark_senior.SetMark(true);
        playerController.ActivateNav(obj_senior.transform.position);
    }

    public void OpenDoor()
    {
        audio_openDoor.Play();

        sentenceList = scriptStore.stDictionary["scene4_0"];
        SetText(sentenceList);
        listIndex = 1;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public void OpenCargo()
    {
        sentenceList = scriptStore.stDictionary["scene4_1"];
        SetText(sentenceList);
        listIndex = 2;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    void SetObjOutside(GameObject obj)
    {
        obj.transform.position = new Vector2(0, -35f);
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
