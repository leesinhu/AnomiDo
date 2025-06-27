using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ScriptState
{
    idle, isPrinting, isPrinted, end
}

public class ScriptControl : MonoBehaviour
{
    protected PassManager passManager;
    protected ShipManager shipManager;

    public ScriptState scriptState { get; set; } = ScriptState.idle; //Dialogue 상태

    [SerializeField] protected ScriptStore scriptStore;

    protected List<string> sentenceList; //SetText에서 사용할 문장 리스트
    protected int listIndex = 0; //문장 묶음 번호
    protected string sentence; //TextTrigger와 SetText에서 사용할 문장
    protected int sentenceIndex = 0; //문장 번호

    //대화 출력
    protected Coroutine coroutine; 
    //대화 넘기기
    protected bool touchFlag = false;
    protected bool touchPermit = false;
    protected bool missonActive = false;
    protected bool sentenceSkipPermit = true;
    //대화 딜레이 && 대사 효과음 출력
    protected float printDelayTime = 0;
    protected bool printSoundFlag = true;
    protected bool printWhole = false;
    protected bool printSpeechEffect = true;

    //Script UI & Sound
    protected GameObject script_parent, scriptWindow, nameWindow, charImg; //대화창(전체)
    RectTransform parentRect, nameRect, charRect;
    protected TextMeshProUGUI text_script, text_name;
    protected RectTransform textRect;
    [SerializeField] protected Sprite[] windows;
    protected GameObject speechEffect;

    AudioSource audioSource_print, audioSource_pass;
    public AudioSource audio_ambient, audio_tut, audio_bell, audio_openDoor, audio_closeDoor;

    bool infoPrinting = false;
    protected bool allSkipPermit = true;
    protected bool menuOpened = false;

    //선내 스테이지 전용. 퀘스트 흐름 및 순서를 위한 변수
    public int flowIndex { get; set; }

    protected virtual void Awake()
    {
        script_parent = GameObject.Find("Canvas/Script");
        parentRect = script_parent.GetComponent<RectTransform>();

        scriptWindow = script_parent.transform.Find("ScriptWindow").gameObject;
        text_script = scriptWindow.transform.Find("text_script").GetComponent<TextMeshProUGUI>();
        textRect = text_script.GetComponent<RectTransform>();

        nameWindow = script_parent.transform.Find("NameWindow").gameObject;
        nameRect = nameWindow.GetComponent<RectTransform>();
        text_name = nameWindow.transform.Find("text_name").GetComponent<TextMeshProUGUI>();

        charImg = script_parent.transform.Find("Character").gameObject;
        charRect = charImg.GetComponent<RectTransform>();

        audioSource_print = transform.GetChild(0).GetComponent<AudioSource>();
        audioSource_pass = transform.GetChild(1).GetComponent<AudioSource>();
        audio_ambient = transform.GetChild(2).GetComponent<AudioSource>();
        audio_tut = transform.GetChild(3).GetComponent<AudioSource>();
        audio_bell = transform.GetChild(4).GetComponent<AudioSource>();
        audio_openDoor = transform.GetChild(5).GetComponent<AudioSource>();
        audio_closeDoor = transform.GetChild(6).GetComponent<AudioSource>();
        /*
        clip_print = Resources.Load<AudioClip>("Sound/" + "Tock");
        clip_pass = Resources.Load<AudioClip>("Sound/" + "Tock2");*/
    }

    protected virtual void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            && touchPermit && !missonActive && !menuOpened)
        {
            /*Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "TouchScreen")
                    touchFlag = true; //Touch란 게임 진행을 위해 클릭(터치)하는 행동
                else if (hit.collider.tag == "Button")
                    touchFlag = false;
            }*/
            touchFlag = true;
        }

        //스킵(개발자용)
        if(Input.GetKeyDown(KeyCode.C) && (scriptState == ScriptState.isPrinting || scriptState == ScriptState.isPrinted) 
            && allSkipPermit)
        {
            //Skip
            if(coroutine != null)
                StopCoroutine(coroutine);
            sentenceIndex = sentenceList.Count - 1;

            if(passManager != null)
                passManager.TutReset();

            NextText();
        }

        if (touchFlag)
        {
            TextTrigger();
            touchFlag = false;
        }

        if(passManager != null && passManager.playerState == PlayerState.inSide)
        {
            audio_ambient.mute = true;
        }
        else //outside
        {
            audio_ambient.mute = false;
        }
    }

    protected IEnumerator PrintText(string targetSentence, float delayTime = 0, bool audio = true, bool skip = false)
    {
        if((targetSentence == null) || (targetSentence == ""))
        {
            NextText();
            yield break;
        }

        if (!script_parent.activeSelf)
            script_parent.SetActive(true);

        bool nameOpened = false;
        bool charOpened = false;

        if (delayTime != 0)
        {
            scriptWindow.SetActive(false);
            if(nameWindow.activeSelf)
            {
                nameOpened = true;
                nameWindow.SetActive(false);
            }
            if (charImg.activeSelf)
            {
                charOpened = true;
                charImg.SetActive(false);
            }
            touchPermit = false;
            yield return new WaitForSeconds(delayTime);
            touchPermit = true;
        }

        if(!scriptWindow.activeSelf)
            scriptWindow.SetActive(true);

        if (nameOpened)
        {
            nameWindow.SetActive(true);
        }
        if (charOpened)
        {
            charImg.SetActive(true);
        }

        if ((targetSentence != null) && (targetSentence != ""))
        {
            scriptState = ScriptState.isPrinting;
            text_script.text = string.Empty;

            if (skip)
            {
                TextTrigger();
                yield break;
            }

            if (printSpeechEffect)
            {
                FixSpeechEffect(1);
                speechEffect.GetComponent<Animator>().SetBool("textPrinting", true);
            }

            int coloringNum = 0;
            string hexColor = "FFFFFF";
            for (int i = 0; i < targetSentence.Length; i++)
            {
                char letter = targetSentence[i];
                if (letter == '<')
                {
                    //Example : <color=#00ff00>정거장</color>
                    int frontTagStartIndex = i;
                    int frontTagEndIndex = i + 14;
                    string colorTag = targetSentence.Substring(i + 1, 13);
                    if (colorTag.StartsWith("color=")) // 색상 태그
                    {
                        hexColor = colorTag.Substring(6).TrimStart('#');
                    }

                    int backTagStartIndex = 0;
                    int backTagEndIndex = 0;
                    for (int j = frontTagEndIndex + 1; j < targetSentence.Length; j++)
                    {
                        if (targetSentence[j] == '<') // 색상을 몇 글자에 적용하는지
                        {
                            backTagStartIndex = j;
                            backTagEndIndex = j + 7;
                            coloringNum = backTagStartIndex - frontTagEndIndex - 1;
                            break;
                        }
                    }

                    string s1 = targetSentence.Substring(0, frontTagStartIndex);
                    string s2 = targetSentence.Substring(frontTagEndIndex + 1, coloringNum);
                    string s3 = targetSentence.Substring(backTagEndIndex + 1);
                    targetSentence = s1 + s2 + s3;
                    i -= 1;
                    continue;
                }

                string afterColoring = string.Empty;
                if (coloringNum > 0)
                {
                    afterColoring = "<color=#" + hexColor + ">" + letter + "</color>";
                    coloringNum -= 1;
                }
                else
                {
                    afterColoring = "<color=#FFFFFF>" + letter + "</color>";
                }

                text_script.text += afterColoring;
                if ((i % 2 == 0) && audio)
                {
                    audioSource_print.Play();
                }
                yield return new WaitForSeconds(0.04f);
            }
            scriptState = ScriptState.isPrinted;
            speechEffect.GetComponent<Animator>().SetBool("textPrinting", false);
        }
        else
        {
            scriptState = ScriptState.isPrinted;
            speechEffect.GetComponent<Animator>().SetBool("textPrinting", false);
        }
    }

    public void TextTrigger()
    {
        switch (scriptState)
        {
            case ScriptState.idle:
                break;
            case ScriptState.isPrinting:
                if(sentenceSkipPermit)
                {
                    if (coroutine != null)
                        StopCoroutine(coroutine);
                    text_script.text = null;
                    foreach (char letter in sentence)
                    {
                        text_script.text += letter;
                    }
                    scriptState = ScriptState.isPrinted;
                    speechEffect.GetComponent<Animator>().SetBool("textPrinting", false);
                }
                break;
            case ScriptState.isPrinted:
                if(printSoundFlag) audioSource_pass.Play();
                NextText(); //Print Next Sentence
                break;
            case ScriptState.end:
                text_script.text = null;
                script_parent.SetActive(false);
                FixSpeechEffect(0);
                break;
        }
    }
    protected void SetText(List<string> temp_stList) //텍스트 리스트 초기화
    {
        sentenceList = temp_stList;
        sentenceIndex = 0;

        if(temp_stList.Count == 0)
        {
            scriptState = ScriptState.end;
            TextTrigger(); //CloseDia() 
            return;
        }
        sentence = sentenceList[sentenceIndex];
    }

    protected void NextText() //다음에 출력할 문장, 일러스트 On/Off
    {
        text_script.text = "";
        if (sentenceIndex == sentenceList.Count - 1) //리스트의 마지막인지 확인
        {
            scriptState = ScriptState.end;
            TextTrigger(); //CloseDia() 
            LastTaskByText(listIndex);
        }
        else
        {
            sentenceIndex++;
            AddTaskByText(sentenceIndex, listIndex);
            sentence = sentenceList[sentenceIndex];
            coroutine = StartCoroutine(PrintText(sentence, printDelayTime, printSoundFlag, printWhole));
        }
    }

    public virtual IEnumerator StartFading()
    {
        yield return null;
        //시작 세팅
    }

    public virtual IEnumerator BeforeOpenTalk()
    {
        yield return null;
        //시작 세팅
    }

    protected virtual void AddTaskByText(int _sentenceIndex, int _listIndex)
    {
        //텍스트 출력과 함께 일어날 연출
    }

    protected virtual void LastTaskByText(int _listIndex)
    {
        //문장 묶음 종료 시 할 행동
        sentenceSkipPermit = true;
        missonActive = false;
        printDelayTime = 0;
    }

    protected IEnumerator FadeInImg(Image img, float fadeTime, System.Action onComplete)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        float elapsedTime = 0f;
        yield return new WaitForSeconds(0.2f);

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime); // 투명도 계산
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, 1); // 완전 불투명
        onComplete?.Invoke();
    }

    protected IEnumerator FadeOutImg(Image img, float fadeTime, System.Action onComplete)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
        float elapsedTime = 0f;
        yield return new WaitForSeconds(0.2f);

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime); // 투명도 계산
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, 0); // 완전 투명
        onComplete?.Invoke();
    }

    protected void ScriptWindow_Up(int spriteIndex = 0)
    {
        parentRect.anchoredPosition = new Vector3(0, 341, 0);

        charImg.SetActive(false);

        nameWindow.SetActive(true);
        nameRect.anchoredPosition = new Vector3(-730, -180, 0);

        scriptWindow.GetComponent<Image>().sprite = windows[spriteIndex];

        text_script.fontSize = 52;
        textRect.offsetMin = new Vector2(220, 440); // Bottom
        textRect.offsetMax = new Vector2(-420, -440); //Top
    }

    protected void ScriptWindow_Middle()
    {
        parentRect.anchoredPosition = new Vector3(0, 0, 0);

        charImg.SetActive(false);

        nameWindow.SetActive(true);
        nameRect.anchoredPosition = new Vector3(-730, 180, 0);

        scriptWindow.GetComponent<Image>().sprite = windows[0];

        text_script.fontSize = 52;
        textRect.offsetMin = new Vector2(220, 440); // Bottom
        textRect.offsetMax = new Vector2(-420, -440); //Top
    }

    protected void ScriptWindow_Down(int spriteIndex = 0)
    {
        parentRect.anchoredPosition = new Vector3(0, -341, 0);

        charImg.SetActive(false);

        nameWindow.SetActive(true);
        nameRect.anchoredPosition = new Vector3(-730, 180, 0);

        scriptWindow.GetComponent<Image>().sprite = windows[spriteIndex];

        text_script.fontSize = 52;
        textRect.offsetMin = new Vector2(220, 440); // Bottom
        textRect.offsetMax = new Vector2(-420, -440); //Top
    }

    protected void ScriptWindow_MiddleLetter()
    {
        parentRect.anchoredPosition = new Vector3(0, 0, 0);

        charImg.SetActive(false);

        nameWindow.SetActive(false);

        scriptWindow.GetComponent<Image>().sprite = windows[1];
        text_script.fontSize = 36;
        textRect.offsetMin = new Vector2(700, 420); // Bottom
        textRect.offsetMax = new Vector2(-720, -420); //Top
    }


    protected void ScriptWindow_Left()
    {
        parentRect.anchoredPosition = new Vector3(-500, 125, 0);

        //charImg.SetActive(true);
        charRect.anchoredPosition = new Vector3(charRect.anchoredPosition.x, 130, 0);

        nameWindow.SetActive(true);
        nameRect.anchoredPosition = new Vector3(-230, -180, 0);

        scriptWindow.GetComponent<Image>().sprite = windows[1];
        text_script.fontSize = 36;
        textRect.offsetMin = new Vector2(700, 420); // Bottom
        textRect.offsetMax = new Vector2(-720, -420); //Top
    }

    protected void ScriptWindow_Left_Guide(float width = 500, float height = 200)
    {
        parentRect.anchoredPosition = new Vector3(-500, 125, 0);

        charImg.SetActive(false);

        nameWindow.SetActive(false);

        scriptWindow.GetComponent<Image>().sprite = windows[1];
        text_script.fontSize = 36;
        textRect.offsetMin = new Vector2(700, 420); // Bottom
        textRect.offsetMax = new Vector2(-720, -420); //Top
    }

    protected void ScriptWindow_Right(bool nameOn = true)
    {
        parentRect.anchoredPosition = new Vector3(720, -160, 0);

        //charImg.SetActive(true);
        charRect.anchoredPosition = new Vector3(charRect.anchoredPosition.x, 130, 0);

        if(nameOn)
        {
            nameWindow.SetActive(true);
            nameRect.anchoredPosition = new Vector3(-116, 215, 0);
        }
        else
        {
            nameWindow.SetActive(false);
        }

        scriptWindow.GetComponent<Image>().sprite = windows[2];

        text_script.fontSize = 36;
        text_script.alignment = TextAlignmentOptions.TopLeft;
        textRect.offsetMin = new Vector2(800, 400); // Bottom
        textRect.offsetMax = new Vector2(-820, -400); //Top
    }

    protected void ScriptWindow_Right2(bool nameOn = true)
    {
        parentRect.anchoredPosition = new Vector3(720, 160, 0);

        //charImg.SetActive(true);
        charRect.anchoredPosition = new Vector3(charRect.anchoredPosition.x, 130, 0);

        if (nameOn)
        {
            nameWindow.SetActive(true);
            nameRect.anchoredPosition = new Vector3(-116, 215, 0);
        }
        else
        {
            nameWindow.SetActive(false);
        }

        scriptWindow.GetComponent<Image>().sprite = windows[2];

        text_script.fontSize = 36;
        text_script.alignment = TextAlignmentOptions.TopLeft;
        textRect.offsetMin = new Vector2(800, 400); // Bottom
        textRect.offsetMax = new Vector2(-820, -400); //Top
    }

    protected void ScriptWindow_Left2(bool nameOn = true)
    {
        parentRect.anchoredPosition = new Vector3(-500, -125, 0);

        //charImg.SetActive(true);
        charRect.anchoredPosition = new Vector3(charRect.anchoredPosition.x, 130, 0);

        if (nameOn)
        {
            nameWindow.SetActive(true);
            nameRect.anchoredPosition = new Vector3(-116, 215, 0);
        }
        else
        {
            nameWindow.SetActive(false);
        }

        scriptWindow.GetComponent<Image>().sprite = windows[2];

        text_script.fontSize = 36;
        text_script.alignment = TextAlignmentOptions.TopLeft;
        textRect.offsetMin = new Vector2(800, 400); // Bottom
        textRect.offsetMax = new Vector2(-820, -400); //Top
    }

    protected void ScriptWindow_Left3(bool nameOn = true)
    {
        parentRect.anchoredPosition = new Vector3(-450, 140, 0);

        //charImg.SetActive(true);
        charRect.anchoredPosition = new Vector3(charRect.anchoredPosition.x, 130, 0);

        if (nameOn)
        {
            nameWindow.SetActive(true);
            nameRect.anchoredPosition = new Vector3(-116, 215, 0);
        }
        else
        {
            nameWindow.SetActive(false);
        }

        scriptWindow.GetComponent<Image>().sprite = windows[2];

        text_script.fontSize = 36;
        text_script.alignment = TextAlignmentOptions.TopLeft;
        textRect.offsetMin = new Vector2(800, 400); // Bottom
        textRect.offsetMax = new Vector2(-820, -400); //Top
    }

    protected IEnumerator TouchPermitDelay(float delayTime)
    {
        touchPermit = false;
        yield return new WaitForSeconds(delayTime);
        touchPermit = true;
    }

    protected IEnumerator SetPlayer()
    {
        yield return new WaitForSeconds(0.8f);
        passManager.SetPlayer();
    }

    protected void EmptyAction()
    {

    }

    public IEnumerator PrintInfoText(int i)
    {
        if(GameManager.Instance.hasInfoPrinted[i-1] == false && !infoPrinting 
            && passManager.PassCount < passManager.PassGoalCount && passManager.stageState < StationStageState.GameOver)
        {
            GameManager.Instance.hasInfoPrinted[i - 1] = true;
            infoPrinting = true;
        }
        else
        {
            yield break;
        }

        audio_bell.Play();
        sentenceList = scriptStore.stDictionary["info" + i];
        SetText(sentenceList);
        listIndex = 3;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        sentenceSkipPermit = false;

        yield return new WaitForSeconds(5);
        if(listIndex == 3)
        {
            TextTrigger();
            audioSource_pass.Play();
        }
        infoPrinting = false;
    }

    public IEnumerator PrintInfoText_NoName(int i)
    {
        if (GameManager.Instance.hasInfoPrinted[i - 1] == false && !infoPrinting
            && passManager.PassCount < passManager.PassGoalCount && passManager.stageState < StationStageState.GameOver)
        {
            GameManager.Instance.hasInfoPrinted[i - 1] = true;
            infoPrinting = true;
        }
        else
        {
            yield break;
        }

        sentenceList = scriptStore.stDictionary["info" + i];
        SetText(sentenceList);
        listIndex = 3;

        coroutine = StartCoroutine(PrintText(sentence, 0));
        AddTaskByText(sentenceIndex, listIndex);
        sentenceSkipPermit = false;

        yield return new WaitForSeconds(5);
        if (listIndex == 3)
        {
            TextTrigger();
            audioSource_pass.Play();
        }
        infoPrinting = false;
    }

    protected void FixSpeechEffect(int i)
    {
        if(i == 0)
        {   //speechEffect 끄기
            if (speechEffect.GetComponent<Image>() != null)
                speechEffect.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            else if (speechEffect.GetComponent<SpriteRenderer>() != null)
                speechEffect.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        else
        {   //speechEffect 켜기
            if (speechEffect.GetComponent<Image>() != null)
                speechEffect.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            else if (speechEffect.GetComponent<SpriteRenderer>() != null)
                speechEffect.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
