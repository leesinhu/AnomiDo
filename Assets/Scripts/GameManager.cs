using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public enum GameState
{
    idle,
    gamePlaying,
    ending,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState gameState { get; set; } = GameState.idle;

    public int ActiveStageNum = 1;

    public int tutSkipStageNum { get; set; } = 0;

    //스크립트 관련
    public bool scriptPassPermit { get; set; }

    //변수 관리(정거장)
        //냉각포 최대 적재량
    public int level_Max = 0;
    float[] bulletMaxGage = new float[] { 12, 16, 20, 24, 28, 32f };

        //냉각포 충전 속도
    public int level_Speed = 0;
    float[] bulletChargeSpeed = new float[] { 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f };

        //냉각포의 체온 하락 속도
    public int level_LossAmount = 0;
    float[] lossTempAmount = new float[] { 0.3f, 0.5f, 0.7f, 0.9f, 1.1f, 1.3f };
    
    public int credit = 0;
    int[] creditCost = new int[] { 3, 5, 8, 12, 17, 0 };

    [System.NonSerialized] public bool[] hasInfoPrinted = new bool[14]; //정거장 스테이지에서 사수의 팝업 메시지 출력 여부

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 GameManager 유지
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스가 있을 경우 새로 생성되는 것을 방지
        }

        if(Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1600, 900, FullScreenMode.Windowed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
        {
            if (Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            }
            else
            {
                Screen.SetResolution(1600, 900, FullScreenMode.Windowed);
            }
        }

        if (Input.GetKeyDown(KeyCode.Keypad1)) LoadSceneByIndex(1);
        else if (Input.GetKeyDown(KeyCode.Keypad2)) LoadSceneByIndex(2);
        else if (Input.GetKeyDown(KeyCode.Keypad3)) LoadSceneByIndex(3);
        else if (Input.GetKeyDown(KeyCode.Keypad4)) LoadSceneByIndex(4);
        else if (Input.GetKeyDown(KeyCode.Keypad5)) LoadSceneByIndex(5);
        else if (Input.GetKeyDown(KeyCode.Keypad6)) LoadSceneByIndex(6);
        else if (Input.GetKeyDown(KeyCode.Keypad7)) LoadSceneByIndex(7);
        else if (Input.GetKeyDown(KeyCode.Keypad8)) LoadSceneByIndex(8);

    }

    public void Initialize()
    {
        Cursor.visible = true;

        gameState = GameState.idle;
        tutSkipStageNum = 0;
        ActiveStageNum = 1;

        level_Max = 0;
        level_Speed = 0;
        level_LossAmount = 0;

        for (int i = 0; i < hasInfoPrinted.Length; i++)
        {
            hasInfoPrinted[i] = false;
        }

        credit = 0;
    }

    public int GetCreditCost(string name)
    {
        int returnVal = 0;
        switch (name)
        {
            case "Max":
                returnVal = creditCost[level_Max];
                break;
            case "Speed":
                returnVal = creditCost[level_Speed];
                break;
            case "LossAmount":
                returnVal = creditCost[level_LossAmount];
                break;
        }
        return returnVal;
    }

    public float GetUpgradeElement(string name)
    {
        float returnVal = 0;
        switch(name)
        {
            case "Max":
                returnVal = bulletMaxGage[level_Max];
                break;
            case "Speed":
                returnVal = bulletChargeSpeed[level_Speed];
                break;
            case "LossAmount":
                returnVal = lossTempAmount[level_LossAmount];
                break;
        }
        return returnVal;
    }

    public void Upgrade(string name)
    {
        switch(name)
        {
            case "Max":
                credit -= creditCost[level_Max];
                level_Max += 1;
                break;
            case "Speed":
                credit -= creditCost[level_Speed];
                level_Speed += 1;
                break;
            case "LossAmount":
                credit -= creditCost[level_LossAmount];
                level_LossAmount += 1;
                break;
        }
        
    }

    public void AddCredit(int num)
    {
        credit += num;
    }

    public void LoadSceneByIndex(int index)
    {
        if (index > 0)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }

    public IEnumerator SoundFade(AudioSource source, float duration, float startVal, float endVal)
    {
        float elapsedTime = 0;

        if (endVal > 0.01f) //멈춰있을 경우 재생
            source.Play();

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            source.volume = Mathf.Lerp(startVal, endVal, t);
            yield return null;
        }
        source.volume = endVal; // 마지막에 정확히 목표 볼륨 설정
    }

    public int GetCurrentSceneIndex()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return currentSceneIndex;
    }
}
