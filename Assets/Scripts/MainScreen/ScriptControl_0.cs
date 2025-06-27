using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptControl_0 : ScriptControl
{
    UIController_Main uiController;
    [SerializeField] Image seniorImg; //사용하진 않지만, speechEffect 작동 코드 이상 없기를 위해
    [SerializeField] AudioSource audio_pro1, audio_pro2, audio_pro3, audio_end1, audio_end2, audio_end3;
    protected override void Awake()
    {
        base.Awake();
        uiController = GameObject.Find("Canvas").GetComponent<UIController_Main>();
        speechEffect = seniorImg.gameObject.transform.Find("speechEffect").gameObject;
        allSkipPermit = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void AddTaskByText(int _sentenceIndex, int _listIndex)
    {
        if (_listIndex == 0)
        {
            switch (_sentenceIndex)
            {
                case 0:
                    sentenceSkipPermit = false;
                    uiController.SetPrologueImage(0, true);
                    break;
                case 1:
                    audio_pro1.Play();
                    uiController.SetPrologueImage(0, false);
                    uiController.SetPrologueImage(1, true);          
                    break;
                case 2:
                    uiController.SetPrologueImage(1, false);
                    uiController.SetPrologueImage(2, true);
                    break;
                case 3:
                    audio_pro2.Play();
                    uiController.SetPrologueImage(2, false);
                    uiController.SetPrologueImage(3, true);
                    break;
                case 4:
                    audio_pro3.Play();
                    uiController.SetPrologueImage(3, false);
                    uiController.SetPrologueImage(4, true);
                    break;
            }
        }
        else if(_listIndex == 1)
        {
            switch (_sentenceIndex)
            {
                case 0:
                    ScriptWindow_Right(false);
                    sentenceSkipPermit = false;
                    audio_end1.Play();
                    printDelayTime = 0.75f;
                    break;
                case 1:
                    printDelayTime = 0;
                    ScriptWindow_Left2(false);
                    break;
                case 2:
                    ScriptWindow_Right2(false);
                    break;
                case 3:
                    ScriptWindow_Left3(false);
                    break;
            }
        }
        else if (_listIndex == 2)
        {
            switch (_sentenceIndex)
            {
                case 0:
                    ScriptWindow_MiddleLetter();
                    sentenceSkipPermit = false;
                    break;
            }
        }
    }

    public override IEnumerator StartFading()
    {
        sentenceList = scriptStore.stDictionary["main_0"];
        SetText(sentenceList);
        listIndex = 0;

        StartCoroutine(uiController.FadeIn(1.0f));
        yield return new WaitForSeconds(1.75f);
        StartCoroutine(uiController.FadeOut(0));
        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    protected override void LastTaskByText(int _listIndex)
    {
        base.LastTaskByText(_listIndex);

        if (_listIndex == 0)
        {
            StartCoroutine(AfterPrologue());
        }
        else if (_listIndex == 1)
        {
            StartCoroutine(Ending2());
        }
        else if (_listIndex == 2)
        {
            StartCoroutine(AfterEnding());
        }
    }

    IEnumerator AfterPrologue()
    {
        StartCoroutine(uiController.FadeIn(1.0f));
        yield return new WaitForSeconds(1.75f);
        uiController.SetPrologueImage(4, false);
        uiController.Open_WorldMap_New();
    }

    public IEnumerator Ending()
    {
        yield return new WaitForSeconds(1f);
        uiController.SetEndingImage(0, true);
        StartCoroutine(uiController.FadeOut(1f));
        yield return new WaitForSeconds(1.25f);

        ScriptWindow_Right(false);
        sentenceList = scriptStore.stDictionary["main_1"];
        SetText(sentenceList);
        listIndex = 1;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;

        StartCoroutine(GameManager.Instance.SoundFade(audio_ambient, 3f, 0, 1f));
    }

    IEnumerator Ending2()
    {
        uiController.SetEndingImage_Scale(0, new Vector2(1.75f, 1.75f), 3f);
        StartCoroutine(GameManager.Instance.SoundFade(audio_ambient, 1.2f, 1f, 0.75f));
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(uiController.SetEndingImage_Fade(1, true, 0.6f));
        yield return new WaitForSeconds(2.0f);
        audio_end2.Play();
        yield return new WaitForSeconds(2.0f);
        uiController.SetEndingImage(2, true);
        yield return new WaitForSeconds(0.2f);
        audio_end3.Play();
        StartCoroutine(GameManager.Instance.SoundFade(audio_ambient, 1.2f, 0.75f, 0.2f));
        uiController.SetEndingImage(3, true);
        yield return new WaitForSeconds(0.2f);
        uiController.SetEndingImage(4, true);
        yield return new WaitForSeconds(0.2f);
        uiController.SetEndingImage(5, true);
        yield return new WaitForSeconds(0.2f);
        uiController.SetEndingImage(6, true);
        yield return new WaitForSeconds(0.2f);
        uiController.SetEndingImage(7, true);
        yield return new WaitForSeconds(0.2f);
        uiController.SetEndingImage(8, true);
        yield return new WaitForSeconds(0.75f);

        audio_bell.Play();
        ScriptWindow_MiddleLetter();
        sentenceList = scriptStore.stDictionary["main_2"];
        SetText(sentenceList);
        listIndex = 2;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        touchPermit = true;
    }

    IEnumerator AfterEnding()
    {
        StartCoroutine(uiController.FadeIn(1.0f));
        yield return new WaitForSeconds(1f);
        uiController.SetEndingImage(0, false);
        uiController.SetEndingImage(1, false);
        uiController.SetEndingImage(2, false);
        uiController.SetEndingImage(3, false);
        uiController.SetEndingImage(4, false);
        uiController.SetEndingImage(5, false);
        uiController.SetEndingImage(6, false);
        uiController.SetEndingImage(7, false);
        uiController.SetEndingImage(8, false);
        StartCoroutine(GameManager.Instance.SoundFade(audio_ambient, 1f, 0.2f, 0));
        yield return new WaitForSeconds(0.75f);
        uiController.Open_WorldMap();
    }
}
