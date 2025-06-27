using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScriptControl_8 : ScriptControl
{
    UiController_InFlight uiController;
    PlayerController playerController;

    [SerializeField] GameObject obj_senior, obj_red, obj_orange, obj_yellow, obj_green, obj_white, obj_sky, obj_blue, 
        obj_purple, obj_black, obj_anomido, obj_anomido2, obj_anomido3;
    [SerializeField] GameObject obj_labber, obj_bed, obj_door1, obj_door2, obj_door3, obj_door4, obj_controller1, obj_controller2, obj_elect;
    SymbolAnimation mark_senior, mark_red, mark_orange, mark_yellow, mark_green, mark_white, mark_sky, mark_blue, mark_purple, 
        mark_black, mark_anomido, mark_labber, mark_bed, mark_door1, mark_door2, mark_door3, mark_door4, mark_controller1, mark_controller2, mark_elect;

    [SerializeField] GameObject touchLimitPanel;
    [SerializeField] AudioSource audio_knock, audio_lightout, audio_elect;

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
        mark_purple = obj_purple.GetComponentInChildren<SymbolAnimation>();
        mark_black = obj_black.GetComponentInChildren<SymbolAnimation>();
        mark_anomido = obj_anomido.GetComponentInChildren<SymbolAnimation>();
        mark_labber = obj_labber.GetComponentInChildren<SymbolAnimation>();
        mark_bed = obj_bed.GetComponentInChildren<SymbolAnimation>();
        mark_door1 = obj_door1.GetComponentInChildren<SymbolAnimation>();
        mark_door2 = obj_door2.GetComponentInChildren<SymbolAnimation>();
        mark_door3 = obj_door3.GetComponentInChildren<SymbolAnimation>();
        mark_door4 = obj_door4.GetComponentInChildren<SymbolAnimation>();
        mark_controller1 = obj_controller1.GetComponentInChildren<SymbolAnimation>();
        mark_controller2 = obj_controller2.GetComponentInChildren<SymbolAnimation>();
        mark_elect = obj_elect.GetComponentInChildren<SymbolAnimation>();

        mark_red.ConvertMark(2);
        mark_orange.ConvertMark(2);
        mark_yellow.ConvertMark(2);
        mark_green.ConvertMark(1);
        mark_white.ConvertMark(2);
        mark_sky.ConvertMark(2);
        mark_blue.ConvertMark(2);
        mark_purple.ConvertMark(2);
        mark_black.ConvertMark(2);

        mark_senior.SetMark(false);
        mark_red.SetMark(false);
        mark_orange.SetMark(false);
        mark_yellow.SetMark(false);
        mark_green.SetMark(false);
        mark_white.SetMark(false);
        mark_sky.SetMark(false);
        mark_blue.SetMark(false);
        mark_purple.SetMark(false);
        mark_black.SetMark(false);
        mark_anomido.SetMark(false);
        mark_labber.SetMark(false);
        mark_bed.SetMark(false);
        mark_door1.SetMark(false);
        mark_door2.SetMark(false);
        mark_door3.SetMark(false);
        mark_door4.SetMark(false);
        mark_controller1.SetMark(false);
        mark_controller2.SetMark(false);
        mark_elect.SetMark(false);

        obj_anomido.SetActive(false);
        obj_anomido2.SetActive(false);
        obj_anomido3.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (uiController.GetIsMenuActiveSelf() == true) menuOpened = true;
        else menuOpened = false;

        base.Update();
        if(flowIndex == 3)
        {
            if(playerController.transform.position.x < -13)
            {
                StartCoroutine(BlindEvent());
                flowIndex = 4;
            }
        }
        else if(flowIndex == 5)
        {
            if(shipManager.enemyRemainCount == 0)
            {
                obj_anomido.SetActive(true);
                obj_anomido2.SetActive(true);
                obj_anomido3.SetActive(true);
                mark_anomido.SetMark(true);
                playerController.ActivateNav(obj_anomido.transform.position);
                flowIndex = 6;
            }
        }
        else if(flowIndex == 7)
        {
            if(mark_door1.symbolState == SymbolState.none && mark_door4.symbolState == SymbolState.none)
            {
                StartCoroutine(ControllerEvent());
                flowIndex = 8;
            }
        }
    }

    public void OpenScript(string name)
    {
        switch (name)
        {
            case "Red":
                if (mark_red.symbolState == SymbolState.red)
                {
                    obj_red.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene8_7"];
                    SetText(sentenceList);
                    listIndex = 7;
                }
                else if (mark_red.symbolState == SymbolState.yellow)
                {
                    sentenceList = scriptStore.stDictionary["scene8_1"];
                    SetText(sentenceList);
                    listIndex = 1;

                    shipManager.movePermit = false;
                    shipManager.interactPermit = false;
                    printSpeechEffect = true;
                    coroutine = StartCoroutine(PrintText(sentence, 0, false));
                    AddTaskByText(sentenceIndex, listIndex);
                    touchPermit = true;
                    return;
                }
                break;
            case "Orange":
                if (mark_orange.symbolState == SymbolState.yellow)
                {
                    sentenceList = scriptStore.stDictionary["scene8_1"];
                    SetText(sentenceList);
                    listIndex = 1;

                    shipManager.movePermit = false;
                    shipManager.interactPermit = false;
                    printSpeechEffect = true;
                    coroutine = StartCoroutine(PrintText(sentence, 0, false));
                    AddTaskByText(sentenceIndex, listIndex);
                    touchPermit = true;
                    return;
                }
                break;
            case "Yellow":
                if (mark_yellow.symbolState == SymbolState.yellow)
                { 
                    sentenceList = scriptStore.stDictionary["scene8_1"];
                    SetText(sentenceList);
                    listIndex = 1;

                    shipManager.movePermit = false;
                    shipManager.interactPermit = false;
                    printSpeechEffect = true;
                    coroutine = StartCoroutine(PrintText(sentence, 0, false));
                    AddTaskByText(sentenceIndex, listIndex);
                    touchPermit = true;
                    return;
                }
                break;
            case "Green":
                if (mark_green.symbolState == SymbolState.none)
                    return;

                if (mark_green.symbolState == SymbolState.red)
                {
                    sentenceList = scriptStore.stDictionary["scene8_0"];
                    SetText(sentenceList);
                    listIndex = 0;

                    obj_red.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_orange.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_yellow.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_green.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_white.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_sky.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_blue.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_purple.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    mark_green.SetMark(false);
                }
                else if (mark_green.symbolState == SymbolState.yellow)
                {
                    sentenceList = scriptStore.stDictionary["scene8_1"];
                    SetText(sentenceList);
                    listIndex = 1;

                    shipManager.movePermit = false;
                    shipManager.interactPermit = false;
                    printSpeechEffect = true;
                    coroutine = StartCoroutine(PrintText(sentence, 0, false));
                    AddTaskByText(sentenceIndex, listIndex);
                    touchPermit = true;
                    return;
                }
                speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;
                break;
            case "White:":
                if (mark_white.symbolState == SymbolState.yellow)
                {
                    sentenceList = scriptStore.stDictionary["scene8_1"];
                    SetText(sentenceList);
                    listIndex = 1;

                    shipManager.movePermit = false;
                    shipManager.interactPermit = false;
                    printSpeechEffect = true;
                    coroutine = StartCoroutine(PrintText(sentence, 0, false));
                    AddTaskByText(sentenceIndex, listIndex);
                    touchPermit = true;
                    return;
                }
                break;
            case "Sky":
                if (mark_sky.symbolState == SymbolState.yellow)
                {
                    sentenceList = scriptStore.stDictionary["scene8_1"];
                    SetText(sentenceList);
                    listIndex = 1;

                    shipManager.movePermit = false;
                    shipManager.interactPermit = false;
                    printSpeechEffect = true;
                    coroutine = StartCoroutine(PrintText(sentence, 0, false));
                    AddTaskByText(sentenceIndex, listIndex);
                    touchPermit = true;
                    return;
                }      
                break;
            case "Blue":
                if(mark_blue.symbolState == SymbolState.red)
                {
                    sentenceList = scriptStore.stDictionary["scene8_14"];
                    SetText(sentenceList);
                    listIndex = 14;

                    obj_blue.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_orange.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_red.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_purple.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    speechEffect = obj_blue.transform.Find("SpeechEffect").gameObject;
                    mark_blue.SetMark(false);
                }
                else if (mark_blue.symbolState == SymbolState.yellow)
                {
                    sentenceList = scriptStore.stDictionary["scene8_1"];
                    SetText(sentenceList);
                    listIndex = 1;

                    shipManager.movePermit = false;
                    shipManager.interactPermit = false;
                    printSpeechEffect = true;
                    coroutine = StartCoroutine(PrintText(sentence, 0, false));
                    AddTaskByText(sentenceIndex, listIndex);
                    touchPermit = true;
                    return;
                }
                break;
            case "Purple":
                if (mark_purple.symbolState == SymbolState.yellow)
                {
                    sentenceList = scriptStore.stDictionary["scene8_1"];
                    SetText(sentenceList);
                    listIndex = 1;

                    shipManager.movePermit = false;
                    shipManager.interactPermit = false;
                    printSpeechEffect = true;
                    coroutine = StartCoroutine(PrintText(sentence, 0, false));
                    AddTaskByText(sentenceIndex, listIndex);
                    touchPermit = true;
                    return;
                }
                break;
            case "Black":
                if (mark_black.symbolState == SymbolState.green)
                {
                    obj_black.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene8_2"];
                    SetText(sentenceList);
                    listIndex = 2;
                }
                else if(mark_black.symbolState == SymbolState.yellow)
                {
                    sentenceList = scriptStore.stDictionary["scene8_3"];
                    SetText(sentenceList);
                    listIndex = 3;
                }
                speechEffect = obj_black.transform.Find("SpeechEffect").gameObject;
                mark_black.SetMark(false);
                break;
            case "Anomido":
                if (mark_anomido.symbolState == SymbolState.red)
                {
                    obj_anomido.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_anomido2.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    obj_anomido3.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                    sentenceList = scriptStore.stDictionary["scene8_12"];
                    SetText(sentenceList);
                    listIndex = 12;
                }
                speechEffect = obj_anomido.transform.Find("SpeechEffect").gameObject;
                mark_anomido.SetMark(false);
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
                        ScriptWindow_Up(6); //Green
                        speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;
                        printSpeechEffect = true;
                        text_name.text = "½Â°´ G";
                        break;
                    case 1:
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(3); ///Red
                        text_name.text = "½Â°´ R";
                        break;
                    case 3:
                        FixSpeechEffect(0);
                        speechEffect = obj_purple.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(10); //Purple
                        text_name.text = "½Â°´ P";
                        break;
                    case 4:
                        FixSpeechEffect(0);
                        speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(4); //Orange
                        text_name.text = "½Â°´ O";
                        break;
                    case 5:
                        FixSpeechEffect(0);
                        speechEffect = obj_sky.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(8); //Sky
                        text_name.text = "½Â°´ S";
                        break;
                    case 6:
                        FixSpeechEffect(0);
                        speechEffect = obj_yellow.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(5); ///Yellow
                        text_name.text = "½Â°´ Y";
                        break;
                    case 10:
                        obj_blue.GetComponent<RotatableObj>().ConvertSprite(4);
                        FixSpeechEffect(0);
                        speechEffect = obj_blue.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(9); ///Blue
                        text_name.text = "½Â°´ B";
                        break;
                    case 11:
                        obj_red.GetComponent<RotatableObj>().ConvertSprite(1);
                        obj_orange.GetComponent<RotatableObj>().ConvertSprite(0);
                        obj_yellow.GetComponent<RotatableObj>().ConvertSprite(7);
                        obj_green.GetComponent<RotatableObj>().ConvertSprite(6);
                        obj_white.GetComponent<RotatableObj>().ConvertSprite(2);
                        obj_sky.GetComponent<RotatableObj>().ConvertSprite(5);
                        obj_purple.GetComponent<RotatableObj>().ConvertSprite(3);
                        FixSpeechEffect(0);
                        speechEffect = obj_white.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(7); //White
                        text_name.text = "½Â°´ W";
                        break;
                    case 12:
                        FixSpeechEffect(0);
                        speechEffect = obj_yellow.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(5); ///Yellow
                        text_name.text = "½Â°´ Y";
                        break;
                    case 14:
                        FixSpeechEffect(0);
                        speechEffect = obj_blue.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(9); ///Blue
                        text_name.text = "½Â°´ B";
                        break;
                    case 18:
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(3); ///Red
                        text_name.text = "½Â°´ R";
                        break;
                    case 21:
                        FixSpeechEffect(0);
                        speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(4); //Orange
                        text_name.text = "½Â°´ O";
                        break;
                    case 22:
                        FixSpeechEffect(0);
                        speechEffect = obj_sky.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(8); //Sky
                        text_name.text = "½Â°´ S";
                        break;
                    case 25:
                        printSoundFlag = false;
                        FixSpeechEffect(0);
                        ScriptWindow_Left_Guide();
                        printSpeechEffect = false;
                        break;
                    case 26:
                        FixSpeechEffect(0);
                        printSoundFlag = true;
                        printSpeechEffect = true;

                        ScriptWindow_Up(6); //Green
                        speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;
                        printSpeechEffect = true;
                        text_name.text = "½Â°´ G";
                        break;
                    case 28:
                        FixSpeechEffect(0);
                        speechEffect = obj_white.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(7); //White
                        text_name.text = "½Â°´ W";
                        break;
                    case 29:
                        FixSpeechEffect(0);
                        ScriptWindow_Up(6); //Green
                        speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;
                        printSpeechEffect = true;
                        text_name.text = "½Â°´ G";
                        break;
                    case 31:
                        FixSpeechEffect(0);
                        speechEffect = obj_purple.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(10); //Purple
                        text_name.text = "½Â°´ P";
                        break;
                    case 34:
                        obj_orange.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        FixSpeechEffect(0);
                        speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(4); //Orange
                        text_name.text = "½Â°´ O";
                        break;
                    case 35:
                        obj_red.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        obj_yellow.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        obj_green.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        obj_white.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        obj_sky.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        obj_blue.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        obj_purple.GetComponent<RotatableObj>().RotateForSpeech(playerController.transform.position);
                        FixSpeechEffect(0);
                        speechEffect = obj_blue.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(9); ///Blue
                        text_name.text = "½Â°´ B";
                        break;
                    case 36:
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(3); ///Red
                        text_name.text = "½Â°´ R";
                        break;
                    case 37:
                        obj_blue.GetComponent<RotatableObj>().ConvertSprite(4);
                        FixSpeechEffect(0);
                        speechEffect = obj_blue.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(9); ///Blue
                        text_name.text = "½Â°´ B";
                        break;
                    case 38:
                        obj_red.GetComponent<RotatableObj>().ConvertSprite(1);
                        obj_orange.GetComponent<RotatableObj>().ConvertSprite(0);
                        obj_yellow.GetComponent<RotatableObj>().ConvertSprite(7);
                        obj_green.GetComponent<RotatableObj>().ConvertSprite(6);
                        obj_white.GetComponent<RotatableObj>().ConvertSprite(2);
                        obj_sky.GetComponent<RotatableObj>().ConvertSprite(5);
                        obj_purple.GetComponent<RotatableObj>().ConvertSprite(3);
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(3); ///Red
                        text_name.text = "½Â°´ R";
                        break;
                    case 39:
                        FixSpeechEffect(0);
                        speechEffect = obj_sky.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(8); //Sky
                        text_name.text = "½Â°´ S";
                        break;
                    case 40:
                        FixSpeechEffect(0);
                        speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(4); //Orange
                        text_name.text = "½Â°´ O";
                        break;
                    case 41:
                        FixSpeechEffect(0);
                        ScriptWindow_Up(6); //Green
                        speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;
                        printSpeechEffect = true;
                        text_name.text = "½Â°´ G";
                        break;
                    case 42:
                        FixSpeechEffect(0);
                        speechEffect = obj_yellow.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Up(5); ///Yellow
                        text_name.text = "½Â°´ Y";
                        break;
                    case 43:
                        FixSpeechEffect(0);
                        ScriptWindow_Up(6); //Green
                        speechEffect = obj_green.transform.Find("SpeechEffect").gameObject;
                        printSpeechEffect = true;
                        text_name.text = "½Â°´ G";
                        break;
                }
                break;
            case 1:
                switch (_sentenceIndex)
                {
                    case 0:
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        break;
                }
                break;
            case 2:
                switch (_sentenceIndex)
                {
                    case 0:
                        printSpeechEffect = true;
                        text_name.text = "½Â°´ K";
                        ScriptWindow_Down(11);
                        break;
                }
                break;
            case 3:
                switch (_sentenceIndex)
                {
                    case 0:
                        printSpeechEffect = true;
                        text_name.text = "½Â°´ K";
                        ScriptWindow_Down(11);
                        break;
                }
                break;
            case 4:
                switch (_sentenceIndex)
                {
                    case 0:
                        shipManager.SetAllDoor(false);
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        break;
                }
                break;
            case 5:
                switch (_sentenceIndex)
                {
                    case 0:
                        audio_bell.Play();
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        sentenceSkipPermit = false;
                        break;
                }
                break;
            case 6:
                switch (_sentenceIndex)
                {
                    case 0:
                        audio_knock.Play();
                        sentenceSkipPermit = false;
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        break;
                }
                break;
            case 7:
                switch (_sentenceIndex)
                {
                    case 0:
                        ScriptWindow_Up(3);
                        text_name.text = "½Â°´ R";
                        break;
                }
                break;
            case 8:
                switch (_sentenceIndex)
                {
                    case 0:
                        ScriptWindow_Down(3);
                        text_name.text = "½Â°´ R";
                        break;
                    case 4:
                        shipManager.SetDoor(13, false);
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        sentenceSkipPermit = false;
                        break;
                }
                break;
            case 9:
                switch (_sentenceIndex)
                {
                    case 0:
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        break;
                }
                break;
            case 10:
                switch (_sentenceIndex)
                {
                    case 0:
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        break;
                }
                break;
            case 11:
                switch (_sentenceIndex)
                {
                    case 0:
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        break;
                    case 1:
                        printDelayTime = 0.75f;
                        audio_elect.Play();
                        break;
                }
                break;
            case 12:
                switch (_sentenceIndex)
                {
                    case 0:
                        playerController.DeactiveNav();
                        printSpeechEffect = true;
                        ScriptWindow_Down(12);
                        text_name.text = "¾Æ³ë¹Ìµµ";
                        break;
                }
                break;
            case 13:
                switch (_sentenceIndex)
                {
                    case 0: //°üÁ¦½Ç ¹® ¿­±â
                        playerController.DeactiveNav();
                        printSpeechEffect = false;
                        ScriptWindow_Right(false);
                        break;
                }
                break;
            case 14:
                switch (_sentenceIndex)
                {
                    case 0:
                        obj_orange.GetComponentInChildren<BoxCollider2D>().enabled = true;
                        obj_red.GetComponentInChildren<BoxCollider2D>().enabled = true;
                        FixSpeechEffect(0);
                        speechEffect = obj_blue.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Down(9); ///Blue
                        text_name.text = "½Â°´ B";
                        break;
                    case 4:
                        FixSpeechEffect(0);
                        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Down(3); ///Red
                        text_name.text = "½Â°´ R";
                        break;
                    case 5:
                        FixSpeechEffect(0);
                        speechEffect = obj_orange.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Down(4); //Orange
                        text_name.text = "½Â°´ O";
                        break;
                    case 8:
                        FixSpeechEffect(0);
                        speechEffect = obj_blue.transform.Find("SpeechEffect").gameObject;
                        ScriptWindow_Down(9); ///Blue
                        text_name.text = "½Â°´ B";
                        break;
                    case 10:
                        printDelayTime = 1.2f;
                        StartCoroutine(EveryOneGoOutEvent());
                        FixSpeechEffect(0);
                        printSpeechEffect = false;
                        sentenceSkipPermit = false;
                        ScriptWindow_Right(false);
                        break;
                }
                break;
            case 15:
                FixSpeechEffect(0);
                printSpeechEffect = false;
                ScriptWindow_Right(false);
                break;
            case 16:
                FixSpeechEffect(0);
                sentenceSkipPermit = false;
                printSpeechEffect = false;
                ScriptWindow_Right(false);
                break;
        }
    }

    protected override void LastTaskByText(int _listIndex)
    {
        base.LastTaskByText(_listIndex);
        
        switch (_listIndex)
        {
            case 0:
                obj_yellow.GetComponentInChildren<BoxCollider2D>().enabled = true;
                obj_sky.GetComponentInChildren<BoxCollider2D>().enabled = true;

                mark_red.SetMark(true);
                mark_orange.SetMark(true);
                mark_yellow.SetMark(true);
                mark_green.SetMark(true);
                mark_white.SetMark(true);
                mark_sky.SetMark(true);
                mark_blue.SetMark(true);
                mark_purple.SetMark(true);

                mark_red.ConvertMark(0);
                mark_orange.ConvertMark(0);
                mark_yellow.ConvertMark(0);
                mark_green.ConvertMark(0);
                mark_white.ConvertMark(0);
                mark_sky.ConvertMark(0);
                mark_blue.ConvertMark(0);
                mark_purple.ConvertMark(0);

                obj_red.GetComponent<RotatableObj>().ConvertSprite(1);
                obj_orange.GetComponent<RotatableObj>().ConvertSprite(0);
                obj_yellow.GetComponent<RotatableObj>().ConvertSprite(7);
                obj_green.GetComponent<RotatableObj>().ConvertSprite(6);
                obj_white.GetComponent<RotatableObj>().ConvertSprite(2);
                obj_sky.GetComponent<RotatableObj>().ConvertSprite(5);
                obj_blue.GetComponent<RotatableObj>().ConvertSprite(4);
                obj_purple.GetComponent<RotatableObj>().ConvertSprite(3);
                flowIndex = 1;

                mark_bed.SetMark(true);
                break;
            case 1:
                mark_red.SetMark(true);
                mark_orange.SetMark(true);
                mark_yellow.SetMark(true);
                mark_green.SetMark(true);
                mark_white.SetMark(true);
                mark_sky.SetMark(true);
                mark_blue.SetMark(true);
                mark_purple.SetMark(true);

                mark_red.ConvertMark(0);
                mark_orange.ConvertMark(0);
                mark_yellow.ConvertMark(0);
                mark_green.ConvertMark(0);
                mark_white.ConvertMark(0);
                mark_sky.ConvertMark(0);
                mark_blue.ConvertMark(0);
                mark_purple.ConvertMark(0);
                break;
            case 2:
                obj_black.GetComponent<RotatableObj>().ConvertSprite(7);
                mark_black.SetMark(true);
                mark_black.ConvertMark(0);
                StartCoroutine(uiController.AddCredit(5));
                break;
            case 3:
                mark_black.SetMark(true);
                mark_black.ConvertMark(0);
                break;
            case 4: //°üÁ¦½Ç·Î º¹±Í
                mark_bed.SetMark(false);
                mark_controller1.SetMark(true);
                flowIndex = 2;

                mark_red.SetMark(false);
                mark_orange.SetMark(false);
                mark_yellow.SetMark(false);
                mark_green.SetMark(false);
                mark_white.SetMark(false);
                mark_sky.SetMark(false);
                mark_blue.SetMark(false);
                mark_purple.SetMark(false);
                mark_black.SetMark(false);
                mark_labber.SetMark(false);
                break;
            case 5:
                mark_controller1.SetMark(false);
                mark_bed.SetMark(true);
                break;
            case 6:
                mark_door1.SetMark(true);
                break;
            case 7:
                mark_door2.SetMark(true);
                mark_red.SetMark(false);
                break;
            case 8: //Ã¢°í¹®À» ´ÝÀº µÚ
                shipManager.SetDoor(13, false);
                flowIndex = 3;
                sentenceSkipPermit = true;
                break;
            case 11:
                printDelayTime = 0;
                flowIndex = 5;
                uiController.SetBlind(false);
                Start_Fight(); //ÀüÅõ ½ÃÀÛ(flow => 5)
                break;
            case 12:
                obj_anomido.GetComponent<RotatableObj>().ConvertSprite(1);
                obj_anomido2.GetComponent<RotatableObj>().ConvertSprite(1);
                obj_anomido3.GetComponent<RotatableObj>().ConvertSprite(1);

                mark_door1.SetMark(true);
                mark_blue.SetMark(true);
                mark_blue.ConvertMark(1);

                obj_red.transform.position = new Vector2(27.78f, 5.54f);
                obj_orange.transform.position = new Vector2(26.05f, 2.22f);
                obj_yellow.transform.position = new Vector2(28.98f, 14.91f);
                obj_green.transform.position = new Vector2(27.99f, 12f);
                obj_white.transform.position = new Vector2(25.94f, 13.97f);
                obj_sky.transform.position = new Vector2(25.94f, 11.34f);
                obj_blue.transform.position = new Vector2(27.96f, 0.67f);
                obj_purple.transform.position = new Vector2(30.02f, 7.27f);

                obj_red.GetComponent<RotatableObj>().ConvertSprite(7);
                obj_orange.GetComponent<RotatableObj>().ConvertSprite(0);
                obj_yellow.GetComponent<RotatableObj>().ConvertSprite(4);
                obj_green.GetComponent<RotatableObj>().ConvertSprite(6);
                obj_white.GetComponent<RotatableObj>().ConvertSprite(7);
                obj_sky.GetComponent<RotatableObj>().ConvertSprite(0);
                obj_blue.GetComponent<RotatableObj>().ConvertSprite(0);
                obj_purple.GetComponent<RotatableObj>().ConvertSprite(6);

                obj_orange.GetComponentInChildren<BoxCollider2D>().enabled = false;
                obj_red.GetComponentInChildren<BoxCollider2D>().enabled = false;
                playerController.ActivateNav(obj_door1.transform.position);
                flowIndex = 6;
                break;
            case 13:
                mark_door1.SetMark(false);
                break;
            case 14:
                EveryOneGoOut();
                mark_door1.SetMark(true);
                mark_door4.SetMark(true);
                obj_door1.name = "Door_Control_Alter";
                obj_door4.name = "Door_Storage3_Alter";
                BoxCollider2D triggerCollider1 = obj_door1.GetComponentsInChildren<BoxCollider2D>()
                               .FirstOrDefault(c => c.isTrigger);
                BoxCollider2D triggerCollider2 = obj_door4.GetComponentsInChildren<BoxCollider2D>()
                               .FirstOrDefault(c => c.isTrigger);
                triggerCollider1.offset = new Vector2(0, -0.2f);
                triggerCollider1.size = new Vector2(1, 0.5f);
                triggerCollider2.offset = new Vector2(0, 0.8f);
                triggerCollider2.size = new Vector2(1, 0.5f);
                flowIndex = 7;
                break;
            case 16:
                shipManager.Sleep();
                return;
        }
        shipManager.LastSentenceEnd();
    }

    public override IEnumerator StartFading()
    {
        shipManager.interactPermit = false;
        shipManager.movePermit = false;

        obj_yellow.GetComponentInChildren<BoxCollider2D>().enabled = false;
        obj_sky.GetComponentInChildren<BoxCollider2D>().enabled = false;
        shipManager.DestroyDoor(3);
        shipManager.DestroyDoor(9);
        shipManager.DestroyDoor(10);

        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;

        flowIndex = 0;
        StartCoroutine(uiController.FadeOut(1.5f, 0.5f));
        StartCoroutine(GameManager.Instance.SoundFade(shipManager.audio_main, 1.5f, 0, shipManager.audio_main.volume));
        yield return new WaitForSeconds(1.5f);

        shipManager.interactPermit = true;
        shipManager.movePermit = true;
        touchPermit = true;

        mark_labber.SetMark(true);
        mark_labber.ConvertMark(0);
        mark_green.SetMark(true);
        mark_black.SetMark(true);
    }

    public void Start_Fight()
    {
        GameManager.Instance.ActiveStageNum = 11;
        if(playerController.transform.position.x > -13)
        {
            playerController.transform.position = new Vector2(-22, 13);
        }
        StartCoroutine(GameManager.Instance.SoundFade(shipManager.audio_fight, 1.5f, 0, shipManager.audio_fight.volume));
        shipManager.SetDoor(15, false);
        shipManager.SetDoor(14, true);
        shipManager.DestroyDoor(1);
        shipManager.DestroyDoor(2);
        shipManager.DestroyDoor(3);
        shipManager.DestroyDoor(4);
        shipManager.DestroyDoor(5);
        shipManager.DestroyDoor(6);
        shipManager.DestroyDoor(7);
        shipManager.DestroyDoor(8);
        shipManager.DestroyDoor(9);
        shipManager.DestroyDoor(10);
        shipManager.SetStageState(InFlightStageState.Fight);
        shipManager.SetEnemyToChase(0, 18);

        flowIndex = 5;
    }

    public IEnumerator StartFading_Fight()
    {
        shipManager.interactPermit = false;
        shipManager.movePermit = false;
        GameManager.Instance.ActiveStageNum = 11;
        if (playerController.transform.position.x > -13)
        {
            playerController.transform.position = new Vector2(-22, 15);
        }
        StartCoroutine(GameManager.Instance.SoundFade(shipManager.audio_fight, 1.5f, 0, shipManager.audio_fight.volume));
        obj_red.transform.position = new Vector2(20.07f, -6.83f);
        obj_orange.transform.position = new Vector2(18.39f, -6.36f);
        obj_yellow.transform.position = new Vector2(13.85f, -2.54f);
        obj_green.transform.position = new Vector2(17.56f, -4.2f);
        obj_white.transform.position = new Vector2(18.07f, -2.4f);
        obj_sky.transform.position = new Vector2(15.75f, -2.79f);
        obj_blue.transform.position = new Vector2(20.82f, -4.7f);
        obj_purple.transform.position = new Vector2(19.08f, -5.19f);
        obj_black.SetActive(false);

        obj_red.GetComponent<RotatableObj>().ConvertSprite(6);
        obj_orange.GetComponent<RotatableObj>().ConvertSprite(7);
        obj_yellow.GetComponent<RotatableObj>().ConvertSprite(0);
        obj_green.GetComponent<RotatableObj>().ConvertSprite(7);
        obj_white.GetComponent<RotatableObj>().ConvertSprite(7);
        obj_sky.GetComponent<RotatableObj>().ConvertSprite(7);
        obj_blue.GetComponent<RotatableObj>().ConvertSprite(5);
        obj_purple.GetComponent<RotatableObj>().ConvertSprite(6);

        shipManager.DestroyDoor(1);
        shipManager.DestroyDoor(2);
        shipManager.DestroyDoor(3);
        shipManager.DestroyDoor(4);
        shipManager.DestroyDoor(5);
        shipManager.DestroyDoor(6);
        shipManager.DestroyDoor(7);
        shipManager.DestroyDoor(8);
        shipManager.DestroyDoor(9);
        shipManager.DestroyDoor(10);
        shipManager.SetStageState(InFlightStageState.Fight);
        shipManager.SetEnemyToChase(0, 18);

        flowIndex = 5;
        StartCoroutine(uiController.FadeOut(1.5f, 0.5f));
        yield return new WaitForSeconds(1.5f);

        shipManager.interactPermit = true;
        shipManager.movePermit = true;
    }

    public void OpenFlightControl()
    {
        sentenceList = scriptStore.stDictionary["scene8_5"];
        SetText(sentenceList);
        listIndex = 5;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public void OpenFlightControl2()
    {
        mark_controller2.SetMark(false);
        sentenceList = scriptStore.stDictionary["scene8_16"];
        SetText(sentenceList);
        listIndex = 16;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public void OpenElectric()
    {
        mark_elect.SetMark(false);
        sentenceList = scriptStore.stDictionary["scene8_11"];
        SetText(sentenceList);
        listIndex = 11;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public void OpenDoor1()
    {
        audio_openDoor.Play();
        shipManager.SetDoor(15, true);
        mark_door1.SetMark(false);
        mark_red.SetMark(true);
        mark_red.ConvertMark(1);
        obj_sky.GetComponentInChildren<BoxCollider2D>().enabled = false;
        obj_orange.GetComponentInChildren<BoxCollider2D>().enabled = false;
        obj_blue.GetComponentInChildren<BoxCollider2D>().enabled = false;
    }

    public IEnumerator OpenDoor2()
    {
        audio_openDoor.Play();
        shipManager.SetDoor(13, true);
        mark_door2.SetMark(false);
        yield return new WaitForSeconds(1);

        StartCoroutine(uiController.FadeIn(0.3f));
        yield return new WaitForSeconds(0.3f);

        yield return new WaitForSeconds(0.1f);
        obj_red.transform.position = new Vector2(20.07f, -6.83f);
        obj_orange.transform.position = new Vector2(18.39f, -6.36f);
        obj_yellow.transform.position = new Vector2(13.85f, -2.54f);
        obj_green.transform.position = new Vector2(17.56f, -4.2f);
        obj_white.transform.position = new Vector2(18.07f, -2.4f);
        obj_sky.transform.position = new Vector2(15.75f, -2.79f);
        obj_blue.transform.position = new Vector2(20.82f, -4.7f);
        obj_purple.transform.position = new Vector2(19.08f, -5.19f);

        obj_red.GetComponent<RotatableObj>().ConvertSprite(6);
        obj_orange.GetComponent<RotatableObj>().ConvertSprite(7);
        obj_yellow.GetComponent<RotatableObj>().ConvertSprite(0);
        obj_green.GetComponent<RotatableObj>().ConvertSprite(7);
        obj_white.GetComponent<RotatableObj>().ConvertSprite(7);
        obj_sky.GetComponent<RotatableObj>().ConvertSprite(7);
        obj_blue.GetComponent<RotatableObj>().ConvertSprite(5);
        obj_purple.GetComponent<RotatableObj>().ConvertSprite(6);
        speechEffect = obj_red.transform.Find("SpeechEffect").gameObject;
        printSpeechEffect = true;

        obj_sky.GetComponentInChildren<BoxCollider2D>().enabled = true;
        obj_orange.GetComponentInChildren<BoxCollider2D>().enabled = true;
        obj_blue.GetComponentInChildren<BoxCollider2D>().enabled = true;

        StartCoroutine(uiController.FadeOut(0.3f));
        yield return new WaitForSeconds(1f);

        sentenceList = scriptStore.stDictionary["scene8_8"];
        SetText(sentenceList);
        listIndex = 8;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public void OpenDoor3()
    {
        audio_openDoor.Play();
        mark_door3.SetMark(false);
        sentenceList = scriptStore.stDictionary["scene8_10"];
        SetText(sentenceList);
        listIndex = 10;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public void OpenDoor4()
    {
        audio_openDoor.Play();
        shipManager.SetDoor(15, true);
        shipManager.SetDoor(12, true);
        obj_anomido.SetActive(false);
        obj_anomido2.SetActive(false);
        obj_anomido3.SetActive(false);
        sentenceList = scriptStore.stDictionary["scene8_13"];
        SetText(sentenceList);
        listIndex = 13;

        mark_door1.SetMark(false);
        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }
   
    public void CloseDoor(int i)
    {
        if (i == 1)
        {
            audio_closeDoor.Play();
            shipManager.SetDoor(15, false);
            mark_door1.SetMark(false);
        }
        else if(i == 2)
        {
            audio_closeDoor.Play();
            shipManager.SetDoor(14, false);
            mark_door4.SetMark(false);
        }
    }

    IEnumerator BlindEvent()
    {
        audio_lightout.Play();
        shipManager.DestroyDoor(2);
        mark_door3.SetMark(true);
        mark_elect.SetMark(true);
        uiController.SetBlind(true);
        sentenceList = scriptStore.stDictionary["scene8_9"];
        SetText(sentenceList);
        listIndex = 9;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = false;

        yield return new WaitForSeconds(5);
        if(listIndex == 9)
            TextTrigger(); //ÀÚµ¿ ³Ñ±â±â
    }

    IEnumerator ControllerEvent()
    {
        mark_controller2.SetMark(true);
        sentenceList = scriptStore.stDictionary["scene8_15"];
        SetText(sentenceList);
        listIndex = 15;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = false;

        yield return new WaitForSeconds(5);
        if (listIndex == 15)
            TextTrigger(); //ÀÚµ¿ ³Ñ±â±â
    }

    public void SetMark_Bed(bool b)
    {
        mark_bed.SetMark(b);
    }

    public void NotYetSleep()
    {
        sentenceList = scriptStore.stDictionary["scene8_4"];
        SetText(sentenceList);
        listIndex = 4;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public IEnumerator AfterSleep()
    {
        shipManager.interactPermit = false;
        shipManager.movePermit = false;
        mark_bed.SetMark(false);

        sentenceList = scriptStore.stDictionary["scene8_6"];
        SetText(sentenceList);
        listIndex = 6;

        speechEffect = obj_senior.transform.Find("SpeechEffect").gameObject;
        printSpeechEffect = true;

        shipManager.passengerGroup1.SetActive(false);
        shipManager.SetDoor(11, false);

        yield return new WaitForSeconds(0.7f);
        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public void SetPassengersFrontDoor()
    {
        obj_black.SetActive(false);
        obj_red.transform.position = new Vector2(28f, -10.13f);
        obj_blue.transform.position = new Vector2(29.74f, -12.52f);
        obj_orange.transform.position = new Vector2(28.05f, -13.13f);
        obj_green.transform.position = new Vector2(26.19f, -12.78f);
        obj_sky.transform.position = new Vector2(25.05f, -11.62f);
        obj_purple.transform.position = new Vector2(24.12f, -13.02f);
        obj_green.transform.position = new Vector2(25.78f, -14.01f);
        obj_yellow.transform.position = new Vector2(21.99f, -11.21f);
        obj_white.transform.position = new Vector2(22.37f, -13.86f);

        obj_red.GetComponent<RotatableObj>().ConvertSprite(2);
        obj_orange.GetComponent<RotatableObj>().ConvertSprite(2);
        obj_yellow.GetComponent<RotatableObj>().ConvertSprite(0);
        obj_green.GetComponent<RotatableObj>().ConvertSprite(2);
        obj_white.GetComponent<RotatableObj>().ConvertSprite(1);
        obj_sky.GetComponent<RotatableObj>().ConvertSprite(1);
        obj_blue.GetComponent<RotatableObj>().ConvertSprite(3);
        obj_purple.GetComponent<RotatableObj>().ConvertSprite(1);
    }

    IEnumerator EveryOneGoOutEvent()
    {
        shipManager.interactPermit = false;
        shipManager.movePermit = false;

        StartCoroutine(uiController.FadeIn(0.3f));
        yield return new WaitForSeconds(0.3f);

        yield return new WaitForSeconds(0.1f);
        EveryOneGoOut();
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(uiController.FadeOut(0.3f));
        yield return new WaitForSeconds(1f);
    }

    void EveryOneGoOut()
    {
        obj_red.transform.position = new Vector3(21.13f, -11.22f, obj_red.transform.position.z);
        obj_orange.transform.position = new Vector3(19.02f, -12f, obj_orange.transform.position.z);
        obj_yellow.transform.position = new Vector3(16.54f, -10.47f, obj_yellow.transform.position.z);
        obj_green.transform.position = new Vector3(13.98f, -13.4f, obj_green.transform.position.z);
        obj_white.transform.position = new Vector3(9.6f, -10.42f, obj_white.transform.position.z);
        obj_sky.transform.position = new Vector3(13.98f, -15.79f, obj_sky.transform.position.z);
        obj_blue.transform.position = new Vector3(20.92f, -14.11f, obj_blue.transform.position.z);
        obj_purple.transform.position = new Vector3(30f, -13.86f, obj_purple.transform.position.z);

        obj_red.GetComponent<RotatableObj>().ConvertSprite(5);
        obj_orange.GetComponent<RotatableObj>().ConvertSprite(7);
        obj_yellow.GetComponent<RotatableObj>().ConvertSprite(5);
        obj_green.GetComponent<RotatableObj>().ConvertSprite(0);
        obj_white.GetComponent<RotatableObj>().ConvertSprite(5);
        obj_sky.GetComponent<RotatableObj>().ConvertSprite(1);
        obj_blue.GetComponent<RotatableObj>().ConvertSprite(3);
        obj_purple.GetComponent<RotatableObj>().ConvertSprite(0);
    }
}
