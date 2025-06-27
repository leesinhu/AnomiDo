using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptControl_6 : ScriptControl
{
    UiController_InFlight uiController;
    PlayerController playerController;

    [SerializeField] GameObject obj_senior, obj_red, obj_orange, obj_yellow, obj_green, obj_white, obj_sky, obj_blue;
    [SerializeField] GameObject obj_labber, obj_bed, obj_elect, obj_controller, obj_door;
    SymbolAnimation mark_senior, mark_red, mark_orange, mark_yellow, mark_green, mark_white, mark_sky, mark_blue,
        mark_labber, mark_bed, mark_elect, mark_controller, mark_door;

    [SerializeField] GameObject touchLimitPanel;
    [SerializeField] Sprite sprite_fall_senior, sprite_fall_red;

    [SerializeField] AudioSource audio_elect, audio_doorBreak;

    int flowIndex_yellow = 0;

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
        mark_sky = obj_sky.GetComponentInChildren<SymbolAnimation>();
        mark_blue = obj_blue.GetComponentInChildren<SymbolAnimation>();
        mark_labber = obj_labber.GetComponentInChildren<SymbolAnimation>();
        mark_bed = obj_bed.GetComponentInChildren<SymbolAnimation>();
        mark_elect = obj_elect.GetComponentInChildren<SymbolAnimation>();
        mark_controller = obj_controller.GetComponentInChildren<SymbolAnimation>();
        mark_door = obj_door.GetComponentInChildren<SymbolAnimation>();

        mark_red.ConvertMark(2);
        mark_orange.ConvertMark(2);
        mark_yellow.ConvertMark(2);
        mark_green.ConvertMark(2);
        mark_white.ConvertMark(2);
        mark_sky.ConvertMark(2);
        mark_blue.ConvertMark(2);

        mark_senior.SetMark(true);
        mark_red.SetMark(false);
        mark_orange.SetMark(false);
        mark_yellow.SetMark(false);
        mark_green.SetMark(false);
        mark_white.SetMark(false);
        mark_sky.SetMark(false);
        mark_blue.SetMark(false);
        mark_labber.SetMark(false);
        mark_bed.SetMark(false);
        mark_elect.SetMark(false);
        mark_controller.SetMark(false);
        mark_door.SetMark(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (uiController.GetIsMenuActiveSelf() == true) menuOpened = true;
        else menuOpened = false;

        base.Update();
        if (flowIndex == 5)
        {
            if(shipManager.enemyRemainCount == 15)
            {
                shipManager.SetDoor(13, true);
                obj_senior.transform.position = new Vector2(19.75f, -10);
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(6);
                flowIndex = 6; //Àû ¼ÒÅÁ ÈÄ »ç¼ö ÃâÇö
            }
        }
        else if(flowIndex == 7)
        {
            if (playerController.transform.position.x < 6 && playerController.transform.position.y >= 8.5f)
            {
                audio_doorBreak.Play();
                shipManager.DestroyDoor(3);
                shipManager.DestroyDoor(9);
                shipManager.DestroyDoor(10);
                shipManager.SetEnemyToChase(5, 13);
                flowIndex = 8;
            }
        }
        else if(flowIndex == 8)
        {
            if (shipManager.enemyRemainCount == 6)
            {
                shipManager.SetDoor(1, true);
                shipManager.SetDoor(2, true);
                shipManager.DeActivateEnemy(14, 19);
                obj_red.SetActive(true);
                playerController.ActivateNav(obj_red.transform.position);
                flowIndex = 9; 
            }
        }
        else if(flowIndex == 9)
        {
            if((playerController.transform.position - obj_red.transform.position).sqrMagnitude < 25)
            {
                StartCoroutine(SeekToSenior());
                mark_door.SetMark(true);
                flowIndex = 10;
            }
        }
        else if(flowIndex == 11)
        {
            if (shipManager.enemyRemainCount == 0)
            {
                StartCoroutine(BackToControlRoom());
                mark_controller.SetMark(true);

                flowIndex = 12;
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
                    sentenceList = scriptStore.stDictionary["scene6_senior"];
                    SetText(sentenceList);
                    listIndex = 0;
                }
                else if(flowIndex == 1)
                {
                    obj_senior.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene6_senior_2"];
                    SetText(sentenceList);
                    listIndex = 1;
                }
                else if (flowIndex == 3)
                {
                    sentenceList = scriptStore.stDictionary["scene6_senior_3"];
                    SetText(sentenceList);
                    listIndex = 3;
                }
                else if (flowIndex == 4)
                {
                    sentenceList = scriptStore.stDictionary["scene6_senior_4"];
                    SetText(sentenceList);
                    listIndex = 10;
                }
                else if(flowIndex == 6)
                {
                    obj_senior.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene6_senior_5"];
                    SetText(sentenceList);
                    listIndex = 11;
                    flowIndex = 6;
                }
                else if(flowIndex == 7)
                {
                    obj_senior.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene6_senior_8"];
                    SetText(sentenceList);
                    listIndex = 13;
                }
                else if(flowIndex == 10)
                {
                    sentenceList = scriptStore.stDictionary["scene6_2"];
                    SetText(sentenceList);
                    listIndex = 14;
                }
                else
                {
                    return;
                }
                speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                break;
            case "Orange":   
                scriptWindow.GetComponent<Image>().sprite = windows[4];
                speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;

                if (mark_orange.symbolState == SymbolState.green)
                {
                    obj_orange.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene6_orange"];
                    SetText(sentenceList);
                    listIndex = 4;
                }
                else //yellow mark
                {
                    sentenceList = scriptStore.stDictionary["scene6_orange_2"];
                    SetText(sentenceList);
                    listIndex = 5;
                }
                mark_orange.SetMark(false);
                break;
            case "Blue":               
                scriptWindow.GetComponent<Image>().sprite = windows[9];
                speechEffect = obj_blue.transform.Find("SpeechEffect").gameObject;

                if (mark_blue.symbolState == SymbolState.green)
                {
                    obj_blue.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene6_blue"];
                    SetText(sentenceList);
                    listIndex = 6;
                }
                else
                {
                    sentenceList = scriptStore.stDictionary["scene6_blue_2"];
                    SetText(sentenceList);
                    listIndex = 7;
                }
                mark_blue.SetMark(false);
                break;
            case "Sky":     
                scriptWindow.GetComponent<Image>().sprite = windows[8];
                speechEffect = obj_sky.transform.Find("SpeechEffect").gameObject;

                if (mark_sky.symbolState == SymbolState.green)
                {
                    obj_sky.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene6_sky"];
                    SetText(sentenceList);
                    listIndex = 8;
                }
                else //yellow mark
                {
                    sentenceList = scriptStore.stDictionary["scene6_sky_2"];
                    SetText(sentenceList);
                    listIndex = 9;
                }
                mark_sky.SetMark(false);
                break;
            case "Red":
                scriptWindow.GetComponent<Image>().sprite = windows[3];
                speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                obj_red.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);

                if (mark_red.symbolState == SymbolState.green)
                {
                    obj_red.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene6_red2"];
                    SetText(sentenceList);
                    listIndex = 17;
                }
                else //yellow mark
                {
                    sentenceList = scriptStore.stDictionary["scene6_red"];
                    SetText(sentenceList);
                    listIndex = 12;
                }
                mark_red.SetMark(false);
                break;
            case "Yellow":
                scriptWindow.GetComponent<Image>().sprite = windows[5];
                speechEffect = obj_yellow.transform.Find("SpeechEffect").gameObject;

                if (mark_yellow.symbolState == SymbolState.green)
                {
                    obj_yellow.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    if(flowIndex_yellow == 0)
                    {
                        sentenceList = scriptStore.stDictionary["scene6_yellow"];
                        SetText(sentenceList);
                        listIndex = 18;
                    }
                    else //1
                    {
                        sentenceList = scriptStore.stDictionary["scene6_yellow2"];
                        SetText(sentenceList);
                        listIndex = 19;
                    }
                }
                mark_yellow.SetMark(false);
                break;
            case "Green":
                scriptWindow.GetComponent<Image>().sprite = windows[6];
                speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;

                if (mark_green.symbolState == SymbolState.green)
                {
                    obj_green.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene6_green"];
                    SetText(sentenceList);
                    listIndex = 20;
                }
                else //yellow mark
                {
                    sentenceList = scriptStore.stDictionary["scene6_green2"];
                    SetText(sentenceList);
                    listIndex = 21;
                }
                mark_green.SetMark(false);
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
                        printSpeechEffect = true;
                        scriptWindow.GetComponent<Image>().sprite = windows[0];
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 1: //Àü±â½Ç¿¡¼­ »ç¼ö¿Í ´ëÈ­
                switch (_sentenceIndex)
                {
                    case 0: 
                        playerController.DeactiveNav();
                        printSpeechEffect = true;
                        sentenceSkipPermit = true;
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 2: //¹èÀü¹Ý Á¶ÀÛ
                switch (_sentenceIndex)
                {
                    case 0:
                        FixSpeechEffect(0);
                        sentenceSkipPermit = true;
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        audio_elect.Play();
                        break;
                    case 1:
                        printDelayTime = 1.5f;
                        StartCoroutine(uiController.FadeInAndOut(0.5f, 0.2f, 0.5f, null));
                        break;
                }
                break;
            case 3: //¹èÀü¹Ý Á¶ÀÛ ¿Ï·á ÈÄ »ç¼ö¿Í
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        printSpeechEffect = true;
                        ScriptWindow_Down();
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 4: //orange
                switch (_sentenceIndex)
                {
                    case 0:
                        text_name.text = "½Â°´ O";
                        printSpeechEffect = true;
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(4);
                        break;
                }
                break;
            case 5: //orange2
                switch (_sentenceIndex)
                {
                    case 0:
                        text_name.text = "½Â°´ O";
                        printSpeechEffect = true;
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(4);
                        break;
                }
                break;
            case 6: //blue
                switch (_sentenceIndex)
                {
                    case 0:
                        text_name.text = "½Â°´ B";
                        printSpeechEffect = true;
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(9);
                        break;
                }
                break;
            case 7: //blue2
                switch (_sentenceIndex)
                {
                    case 0:
                        text_name.text = "½Â°´ B";
                        printSpeechEffect = true;
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(9);
                        break;
                }
                break;
            case 8: //sky
                switch (_sentenceIndex)
                {
                    case 0:
                        text_name.text = "½Â°´ S";
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(8);
                        printSpeechEffect = true;
                        break;
                    case 2:
                        FixSpeechEffect(0);
                        speechEffect = obj_white.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ W";
                        ScriptWindow_Down(7);
                        break;
                    case 3:
                        obj_sky.GetComponent<RotatableObj>().ConvertSprite(5);
                        FixSpeechEffect(0);
                        speechEffect = obj_sky.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ S";
                        ScriptWindow_Down(8);
                        break;
                    case 4:
                        FixSpeechEffect(0);
                        speechEffect = obj_white.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ W";
                        ScriptWindow_Down(7);
                        break;
                    case 5:
                        FixSpeechEffect(0);
                        speechEffect = obj_sky.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ S";
                        ScriptWindow_Down(8);
                        break;
                    case 7:
                        FixSpeechEffect(0);
                        speechEffect = obj_white.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ W";
                        ScriptWindow_Down(7);
                        break;
                    case 11:
                        FixSpeechEffect(0);
                        speechEffect = obj_sky.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ S";
                        ScriptWindow_Down(8);
                        break;
                }
                break;
            case 9: //sky2
                switch (_sentenceIndex)
                {
                    case 0:
                        speechEffect = obj_sky.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ S";
                        ScriptWindow_Down(8);
                        break;
                    case 1:
                        FixSpeechEffect(0);
                        speechEffect = obj_white.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "½Â°´ W";
                        ScriptWindow_Down(7);
                        break;
                }
                break;
            case 10: //Á¶Á¾½Ç¿¡¼­ »ç¼ö
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
                        ScriptWindow_Down(0);
                        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
                        text_name.text = "»ç¼ö";
                        break;
                }
                break;
            case 12:
                switch (_sentenceIndex)
                {
                    case 0:
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        playerController.DeactiveNav();
                        ScriptWindow_Right(false);
                        break;
                }
                break;
            case 13:
            case 14:
            case 15:
                switch (_sentenceIndex)
                {
                    case 0:
                        ScriptWindow_Right(false);
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        break;
                }
                break;
            case 16:
                switch (_sentenceIndex)
                {
                    case 0:
                        audio_bell.Play();
                        ScriptWindow_Right(false);
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        sentenceSkipPermit = false;
                        break;
                }
                break;
            case 17:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(3);
                        text_name.text = "½Â°´ R";
                        break;
                }
                break;
            case 18:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(5);
                        text_name.text = "½Â°´ Y";
                        break;
                }
                break;
            case 19:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(5);
                        text_name.text = "½Â°´ Y";
                        break;
                }
                break;
            case 20:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(6);
                        text_name.text = "½Â°´ G";
                        break;
                }
                break;
            case 21:
                switch (_sentenceIndex)
                {
                    case 0:
                        sentenceSkipPermit = true;
                        ScriptWindow_Down(6);
                        text_name.text = "½Â°´ G";
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
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(2);
                obj_senior.transform.position = new Vector2(-22, 15f);
                mark_senior.SetMark(true);
                playerController.ActivateNav(obj_senior.transform.position);

                shipManager.SetDoor(1, true);
                shipManager.SetDoor(2, true);
                shipManager.SetDoor(15, true);
                mark_elect.SetMark(false);

                flowIndex = 1;
                break;
            case 1:
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(2);
                mark_senior.SetMark(false);
                mark_elect.SetMark(true);

                flowIndex = 2;
                break;
            case 2: //¹èÀü¹Ý Äù½ºÆ® ¿Ï·á
                printDelayTime = 0;
                mark_elect.SetMark(false);
                mark_senior.SetMark(true);

                flowIndex = 3;
                break;
            case 3: //»ç¼ö Á¶Á¾½Ç·Î
                obj_senior.transform.position = new Vector2(26.48f, 6.71f);
                obj_senior.GetComponent<RotatableObj>().ConvertSprite(0);
                mark_senior.SetMark(true);

                mark_orange.SetMark(true);
                mark_blue.SetMark(true);
                mark_sky.SetMark(true);
                mark_green.SetMark(true);
                mark_yellow.SetMark(true);

                flowIndex = 4;
                break;
            case 4:
                obj_orange.GetComponent<RotatableObj>().ConvertSprite(7);
                mark_orange.SetMark(true);
                mark_orange.ConvertMark(0);
                StartCoroutine(uiController.AddCredit(4));
                break;
            case 5:
                obj_orange.GetComponent<RotatableObj>().ConvertSprite(7);
                mark_orange.SetMark(true);
                mark_orange.ConvertMark(0);
                break;
            case 6:
                obj_blue.GetComponent<RotatableObj>().ConvertSprite(5);
                mark_blue.SetMark(true);
                mark_blue.ConvertMark(0);
                StartCoroutine(uiController.AddCredit(5));
                break;
            case 7:
                mark_blue.SetMark(true);
                mark_blue.ConvertMark(0);
                break;
            case 8:
                obj_sky.GetComponent<RotatableObj>().ConvertSprite(5);
                mark_sky.SetMark(true);
                mark_sky.ConvertMark(0);
                StartCoroutine(uiController.AddCredit(8));
                break;
            case 9:
                mark_sky.SetMark(true);
                mark_sky.ConvertMark(0);
                break;
            case 10:
                mark_senior.SetMark(false);
                shipManager.SetDoor(15, false);
                mark_bed.SetMark(true);
                obj_white.GetComponentInChildren<BoxCollider2D>().enabled = true;
                mark_sky.SetMark(false);
                flowIndex = 5;
                break;
            case 11:
                obj_senior.transform.position = new Vector2(-26.02f, -11.47f);
                obj_senior.GetComponent<RotatableObj>().enabled = false;
                obj_senior.GetComponent<SpriteRenderer>().sprite = sprite_fall_senior;
                shipManager.SetDoor(13, false);
                flowIndex = 7;
                break;
            case 12:
                obj_red.GetComponent<RotatableObj>().ConvertSprite(6);
                mark_door.SetMark(true);
                break;
            case 13:
                mark_senior.SetMark(true);
                break;
            case 14:
                shipManager.ActivateEnemy(14, 19);
                shipManager.SetEnemyToChase(14, 19);
                flowIndex = 11;
                break;
            case 16:
                shipManager.Sleep();
                return;
            case 17:
                obj_red.GetComponent<RotatableObj>().ConvertSprite(3);
                mark_red.SetMark(false);
                mark_yellow.SetMark(true);
                mark_yellow.ConvertMark(2);
                flowIndex_yellow = 1;
                break;
            case 18:
                obj_yellow.GetComponent<RotatableObj>().ConvertSprite(5);
                mark_yellow.SetMark(false);
                mark_red.SetMark(true);
                break;
            case 19:
                obj_yellow.GetComponent<RotatableObj>().ConvertSprite(5);
                mark_yellow.SetMark(false);
                StartCoroutine(uiController.AddCredit(6));
                break;
            case 20:
                obj_green.GetComponent<RotatableObj>().ConvertSprite(2);
                mark_green.SetMark(true);
                mark_green.ConvertMark(0);
                StartCoroutine(uiController.AddCredit(3));
                break;
            case 21:
                mark_green.SetMark(true);
                mark_green.ConvertMark(0); 
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

        mark_labber.SetMark(true);
        mark_labber.ConvertMark(0);
    }

    public IEnumerator StartFading_Fight()
    {
        obj_red.transform.position = new Vector2(-17.92f, 9.3f);
        obj_red.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        obj_red.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        //ÀüÅõ BGM
        StartCoroutine(GameManager.Instance.SoundFade(shipManager.audio_fight, 1, 0, shipManager.volume_fight));
        shipManager.interactPermit = false;
        shipManager.movePermit = false;

        obj_red.SetActive(false);
        obj_orange.SetActive(false);
        obj_yellow.SetActive(false);
        obj_green.SetActive(false);
        obj_white.SetActive(false);
        obj_sky.SetActive(false);
        obj_blue.SetActive(false);

        playerController.transform.position = new Vector2(26.62f, -6.57f);
        SetObjOutside(obj_senior);
        flowIndex = 5;

        StartCoroutine(uiController.FadeOut(1.5f, 0.5f));
        yield return new WaitForSeconds(1.5f);

        shipManager.interactPermit = true;
        shipManager.movePermit = true;
    }

    public void OpenElectric()
    {
        sentenceList = scriptStore.stDictionary["scene6_0"];
        SetText(sentenceList);
        listIndex = 2;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;

        mark_elect.SetMark(false);
    }

    public void OpenDoor()
    {
        audio_openDoor.Play();
        shipManager.SetDoor(0, true);

        sentenceList = scriptStore.stDictionary["scene6_1"];
        SetText(sentenceList);
        listIndex = 13;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;

        mark_door.SetMark(false);
    }

    IEnumerator SeekToSenior()
    {
        sentenceList = scriptStore.stDictionary["scene6_red"];
        SetText(sentenceList);
        listIndex = 12;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = false;

        yield return new WaitForSeconds(5);
        if(listIndex == 12)
            TextTrigger(); //ÀÚµ¿ ³Ñ±â±â
    }

    IEnumerator BackToControlRoom()
    {
        sentenceList = scriptStore.stDictionary["scene6_3"];
        SetText(sentenceList);
        listIndex = 15;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = false;

        yield return new WaitForSeconds(5);
        TextTrigger(); //ÀÚµ¿ ³Ñ±â±â
    }

    public void OpenFlightControl()
    {
        sentenceList = scriptStore.stDictionary["scene6_4"];
        SetText(sentenceList);
        listIndex = 16;

        coroutine = StartCoroutine(PrintText(sentence, 0, false));
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
