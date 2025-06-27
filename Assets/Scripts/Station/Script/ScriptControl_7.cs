using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScriptControl_7 : ScriptControl
{
    UIController_Station uiController;

    GameObject playerObj;
    Image playerImg;
    Image unknownImg;

    protected override void Awake()
    {
        base.Awake();
        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
        uiController = GameObject.Find("Canvas").GetComponent<UIController_Station>();

        playerObj = GameObject.Find("Canvas").transform.Find("InGameUI/img_player").gameObject;
        playerImg = playerObj.GetComponent<Image>();
        unknownImg = GameObject.Find("Canvas").transform.Find("InGameUI/img_unknown").GetComponent<Image>();
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
                    audio_bell.Play();
                    FixSpeechEffect(0);
                    printSpeechEffect = false;
                    sentenceSkipPermit = false;
                    ScriptWindow_Left_Guide();
                    break;
                case 1:
                    printSpeechEffect = true;
                    printDelayTime = 1.0f;
                    sentenceSkipPermit = true;
                    printSoundFlag = true;
                    ScriptWindow_Up(3);
                    text_name.text = "???";
                    break;
                case 2:
                    printDelayTime = 0;
                    break;
            }
        }
        else if(_listIndex == 3)
        {
            switch (_sentenceIndex)
            {
                case 0:
                    FixSpeechEffect(0);
                    printSpeechEffect = false;
                    sentenceSkipPermit = false;
                    ScriptWindow_Right(false);
                    break;
            }
        }
    }

    protected override void LastTaskByText(int _listIndex)
    {
        base.LastTaskByText(_listIndex);

        if (_listIndex == 0)
        {
            passManager.StartStageSetting();
            StartCoroutine(GameManager.Instance.SoundFade(audio_ambient, 1.5f, 0, audio_ambient.volume));
            audio_ambient.Play();
        }
    }

    public override IEnumerator StartFading()
    {
        sentenceList = scriptStore.stDictionary["scene7_0"];
        speechEffect = unknownImg.gameObject.transform.Find("speechEffect").gameObject;
        speechEffect.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        SetText(sentenceList);
        listIndex = 0;

        playerObj.SetActive(true);

        StartCoroutine(uiController.FadeOut(0.5f, 0.5f));
        yield return new WaitForSeconds(1.5f);
        coroutine = StartCoroutine(PrintText(sentence, 0, false));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public override IEnumerator BeforeOpenTalk()
    {
        uiController.ShowOpenGuide();
        yield return new WaitForSeconds(0.5f);

        playerObj.SetActive(true);
        passManager.playPermit = true;
        touchPermit = false;
        printDelayTime = 0;    
    }
}
