using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptControl_5 : ScriptControl
{
    UIController_Station uiController;

    GameObject playerObj;
    Image playerImg;
    Image seniorImg;

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
                    audio_tut.Play();
                    sentenceSkipPermit = true;
                    ScriptWindow_Up();
                    text_name.text = "사수";
                    break;
                case 4:
                    uiController.FadeTutImg(0, true);
                    break;
                case 6:
                    uiController.FadeTutImg(1, true);
                    break;
                case 7:
                    uiController.FadeTutImg(0, false);
                    uiController.FadeTutImg(1, false);
                    uiController.FadeTutImg(2, true);
                    break;
                case 8:
                    uiController.FadeTutImg(2, false);
                    FixSpeechEffect(0);
                    printDelayTime = 1.0f;
                    break;
            }
        }
        if (_listIndex == 1)
        {
            switch (_sentenceIndex)
            {
                case 0:
                    playerImg.color = new Color(255, 255, 255, 255);
                    seniorImg.color = new Color(255, 255, 255, 255);
                    sentenceSkipPermit = true;
                    ScriptWindow_Up();
                    text_name.text = "사수";
                    break;
                case 2:
                    FixSpeechEffect(0);

                    printDelayTime = 2f;
                    StartCoroutine(FadeOutImg(playerImg, 1.0f, null));
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
        sentenceList = scriptStore.stDictionary["scene5_0"];
        speechEffect = seniorImg.gameObject.transform.Find("speechEffect").gameObject;
        speechEffect.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        SetText(sentenceList);
        listIndex = 0;

        playerObj.SetActive(true);

        StartCoroutine(uiController.FadeOut(0.5f, 0.5f));
        yield return new WaitForSeconds(1.5f);
        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public override IEnumerator BeforeOpenTalk()
    {
        sentenceList = scriptStore.stDictionary["scene5_1"];
        SetText(sentenceList);
        listIndex = 1;
        speechEffect = seniorImg.gameObject.transform.Find("speechEffect").gameObject;
        yield return new WaitForSeconds(0.5f);

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    public IEnumerator Info_Recall()
    {
        StopCoroutine(coroutine);

        audio_bell.Play();
        sentenceList = scriptStore.stDictionary["scene5_recall"];
        SetText(sentenceList);
        listIndex = 2;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = false;
        sentenceSkipPermit = false;

        yield return new WaitUntil(() => !passManager.gateOpenSign);
        TextTrigger();
    }

    public void Info_Fail()
    {
        if (listIndex == 4) return;
        if (coroutine != null) StopCoroutine(coroutine);

        audio_bell.Play();
        sentenceList = scriptStore.stDictionary["scene5_fail"];
        SetText(sentenceList);
        listIndex = 4;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = false;
        sentenceSkipPermit = false;
    }
    protected override void LastTaskByText(int _listIndex)
    {
        base.LastTaskByText(_listIndex);

        if (_listIndex == 0)
        {
            touchPermit = false;
            printDelayTime = 0;

            uiController.FadeTutImg(0, false);
            uiController.FadeTutImg(1, false);
            uiController.FadeTutImg(2, false);

            passManager.SetPlayerToOutSide();
            passManager.StartStageSetting();
            StartCoroutine(GameManager.Instance.SoundFade(audio_tut, 5f, audio_tut.volume, 0));
            StartCoroutine(GameManager.Instance.SoundFade(audio_ambient, 3f, 0, audio_ambient.volume));
            audio_ambient.Play();
        }
        if (_listIndex == 1)
        {
            playerImg.color = new Color(playerImg.color.r, playerImg.color.g, playerImg.color.b, 0);
            passManager.playPermit = true;
            touchPermit = false;
            printDelayTime = 0;
            uiController.ShowOpenGuide();
        }
    }
}
