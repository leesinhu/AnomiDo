using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;


public enum InFlightStageState
{
    Idle,
    Fight,
    StageEnd,
}

public class ShipManager : MonoBehaviour
{
    public InFlightStageState stageState { get; set; } = InFlightStageState.Idle;
    [SerializeField] GameObject enemies_parent;
    public GameObject passengerGroup1, passengerGroup2;
    List<GameObject> enemies = new List<GameObject>();
    public int enemyRemainCount { get; set; } = 0;
    PlayerController playerController;
    PlayerHitBox playerHitBox;

    public bool movePermit { get; set; } = true;
    public bool interactPermit { get; set; } = true;

    int currentSceneNum;
    ScriptControl_2 sc2;
    ScriptControl_4 sc4;
    ScriptControl_6 sc6;
    ScriptControl_8 sc8;

    UiController_InFlight uiController;
    List<DoorAndFog> doors = new List<DoorAndFog>();

    Camera mainCamera;
    public AudioSource audio_main { get; set; }
    public AudioSource audio_fight { get; set; }
    public float volume_main = 0.25f, volume_fight = 0.25f;

    private void Awake()
    {
        currentSceneNum = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneNum == 2)
        {
            sc2 = GameObject.Find("ScriptManager").GetComponent<ScriptControl_2>();
        }
        else if (currentSceneNum == 4)
        {
            sc4 = GameObject.Find("ScriptManager").GetComponent<ScriptControl_4>();
        }
        else if (currentSceneNum == 6)
        {
            sc6 = GameObject.Find("ScriptManager").GetComponent<ScriptControl_6>();
        }
        else if (currentSceneNum == 8)
        {
            sc8 = GameObject.Find("ScriptManager").GetComponent<ScriptControl_8>();
        }

        for (int i = 0; i < enemies_parent.transform.childCount; i++)
        {
            enemies.Add(enemies_parent.transform.GetChild(i).gameObject);
        }
        enemyRemainCount = enemies.Count;

        playerController = GameObject.Find("Player").transform.GetComponent<PlayerController>();
        playerHitBox = GameObject.Find("Player").transform.Find("HitBoxCollider").GetComponent<PlayerHitBox>();
        uiController = GameObject.Find("Canvas").GetComponent<UiController_InFlight>();

        Transform parent_door = GameObject.Find("Background/Door").transform;
        foreach(Transform child in parent_door)
        {
            doors.Add(child.GetComponent<DoorAndFog>());
        }

        GameManager.Instance.gameState = GameState.gamePlaying;
        SetFov(false);

        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        audio_main = mainCamera.transform.GetChild(1).GetComponent<AudioSource>();
        audio_fight = mainCamera.transform.GetChild(2).GetComponent<AudioSource>();
    }

    public void SetFov(bool b)
    {
        //if (FOV == null || Shadaow == null)
        //{
        //    Debug.Log("Fov 또는 Shadow 할당되지 않음");
        //    return;
        //}

        //if (b)
        //{
        //    FOV.SetActive(true);
        //    Shadaow.SetActive(true);
        //}
        //else
        //{
        //    FOV.SetActive(false);
        //    Shadaow.SetActive(false);
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        movePermit = true;
        interactPermit = true;

        if (currentSceneNum == 2)
        {
            GameManager.Instance.ActiveStageNum = 2;
            SetStageState(InFlightStageState.Idle);
            StartCoroutine(sc2.StartFading());
        }
        else if (currentSceneNum == 4)
        {
            if (GameManager.Instance.ActiveStageNum != 4) GameManager.Instance.ActiveStageNum = 5;

            if (GameManager.Instance.ActiveStageNum == 4)
            {
                SetStageState(InFlightStageState.Idle);
                StartCoroutine(sc4.StartFading());
            }
            else if(GameManager.Instance.ActiveStageNum == 5)
            {
                SetAllDoor(false);
                SetDoor(15, true);
                SetStageState(InFlightStageState.Fight);
                StartCoroutine(sc4.StartFading_Fight());
            }
        }
        else if (currentSceneNum == 6)
        {
            if (GameManager.Instance.ActiveStageNum != 7) GameManager.Instance.ActiveStageNum = 8;

            if (GameManager.Instance.ActiveStageNum == 7)
            {
                SetStageState(InFlightStageState.Idle);
                StartCoroutine(sc6.StartFading());
            }
            else if(GameManager.Instance.ActiveStageNum == 8)
            {
                SetAllDoor(false);
                SetDoor(15, true);
                SetStageState(InFlightStageState.Fight);
                StartCoroutine(sc6.StartFading_Fight());
            }
        }
        else if (currentSceneNum == 8)
        {
            if (GameManager.Instance.ActiveStageNum != 10) GameManager.Instance.ActiveStageNum = 11;

            if (GameManager.Instance.ActiveStageNum == 10)
            {
                SetStageState(InFlightStageState.Idle);
                StartCoroutine(sc8.StartFading());
            }
            else if (GameManager.Instance.ActiveStageNum == 11)
            {
                SetAllDoor(false);
                SetDoor(14, true);
                SetStageState(InFlightStageState.Fight);
                StartCoroutine(sc8.StartFading_Fight());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //모든 적 처치?
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyController enemyCtrl = enemies[i].transform.GetChild(0).GetComponent<EnemyController>();
            if (enemyCtrl._state == Define.State.Die && !enemyCtrl.deadCounted)
            {
                enemyRemainCount -= 1;
                enemyCtrl.deadCounted = true;
            }
        }

        /*        if (enemyRemainCount == 0 && currentSceneNum >= 8)
                {
                    stageState = InFlightStageState.StageEnd;
                    movePermit = false;
                }*/

        /* if (lastSentenceSign)
         {
             movePermit = true;
             StartCoroutine(DelayToInteractPermit(0.5f));
             lastSentenceSign = false;
         }*/

        //게임 오버. 다시 시작
        if (stageState == InFlightStageState.StageEnd && Input.GetKeyDown(KeyCode.Return) && interactPermit)
        {
            interactPermit = false;
            SetStageState(InFlightStageState.StageEnd);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiController.OpenMenu();
        }
    }

    public void LastSentenceEnd(bool _movePermit = true, bool _interactPermit = true)
    {
        if(_movePermit)
        {
            movePermit = true;
        }
        
        if(_interactPermit)
        {
            StartCoroutine(DelayToInteractPermit(0.5f));
        }
    }

    public void SetStageState(InFlightStageState state)
    {
        switch (state)
        {
            case InFlightStageState.Idle:
                stageState = InFlightStageState.Idle;
                passengerGroup1.SetActive(true);
                passengerGroup2.SetActive(false);
                foreach (GameObject enemy in enemies)
                {
                    enemy.SetActive(false);
                }
                break;
            case InFlightStageState.Fight:
                stageState = InFlightStageState.Fight;
                passengerGroup1.SetActive(false);
                passengerGroup2.SetActive(true);
                movePermit = true;
                foreach (GameObject enemy in enemies)
                {
                    enemy.SetActive(true);
                }
                break;
            case InFlightStageState.StageEnd:
                StartCoroutine(uiController.FadeToRestartScene(0.5f, audio_fight));
                break;
        }
    }

    public IEnumerator DelayToInteractPermit(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        interactPermit = true;
    }

    public void OpenScript(string _name)
    {
        if (currentSceneNum == 2)
            sc2.OpenScript(_name);
        else if (currentSceneNum == 4)
            sc4.OpenScript(_name);
        else if (currentSceneNum == 6)
            sc6.OpenScript(_name);
        else if (currentSceneNum == 8)
            sc8.OpenScript(_name);
    }

    public void OpenObjMenu(string _name)
    {
        if(_name == "Cooling Cannon")
        {
            movePermit = false;
            interactPermit = false;
            uiController.OpenUpgradePanel();
            
            if (currentSceneNum == 2)
            {
                if(sc2.flowIndex <= 2)
                    StartCoroutine(sc2.CoolingCannonTut());
            }
        }
        else if(_name == "PlayerBed")
        {
            movePermit = false;
            interactPermit = false;
            
            if (currentSceneNum < 8)
            {
                uiController.OpenSleepPanel();
            }
            else
            {
                if (sc8.flowIndex == 1)
                    sc8.NotYetSleep();
                else
                    uiController.OpenSleepPanel();
            }
        }
        else if(_name == "Door_Storage3")
        {
            if (currentSceneNum == 4)
            {
                movePermit = false;
                interactPermit = false;
                sc4.OpenDoor();
            }
        }
        else if (_name == "Cargo")
        {
            if (currentSceneNum == 4)
            {
                movePermit = false;
                interactPermit = false;
                sc4.OpenCargo();
            }
        }
        else if (_name == "ElectricPanel")
        {
            if (currentSceneNum == 6)
            {
                movePermit = false;
                interactPermit = false;
                sc6.OpenElectric();
            }
            if (currentSceneNum == 8)
            {
                movePermit = false;
                interactPermit = false;
                sc8.OpenElectric();
            }
        }
        else if (_name == "FlightControl")
        {
            if (currentSceneNum == 6)
            {
                movePermit = false;
                interactPermit = false;
                sc6.OpenFlightControl();
            }
            if (currentSceneNum == 8)
            {
                movePermit = false;
                interactPermit = false;
                sc8.OpenFlightControl();
            }
        }
        else if (_name == "FlightControl2")
        {
            if (currentSceneNum == 8)
            {
                movePermit = false;
                interactPermit = false;
                sc8.OpenFlightControl2();
            }
        }
        else if (_name == "Door_Engine")
        {
            if (currentSceneNum == 6)
            {
                movePermit = false;
                interactPermit = false;
                sc6.OpenDoor();
            }
        }
        else if (_name == "Door_Storage2")
        {
            if (currentSceneNum == 8)
            {
                movePermit = false;
                interactPermit = false;
                StartCoroutine(sc8.OpenDoor2());
            }
        }
        else if (_name == "Door_Storage3")
        {
            if (currentSceneNum == 8)
            {
                if (sc8.flowIndex == 7)
                    sc8.CloseDoor(2);
            }
        }
        else if (_name == "Door_Elect1")
        {
            if (currentSceneNum == 8)
            {
                movePermit = false;
                interactPermit = false;
                sc8.OpenDoor3();
            }
        }
        else if (_name == "Door_Control")
        {
            if (currentSceneNum == 8)
            {
                if (sc8.flowIndex < 6)
                    sc8.OpenDoor1();
                else if (sc8.flowIndex == 6)
                    sc8.OpenDoor4();
            }
        }
        else if (_name == "Door_Control_Alter")
        {
            if (currentSceneNum == 8)
            {
                sc8.CloseDoor(1);
            }
        }
        else if (_name == "Door_Storage3_Alter")
        {
            if (currentSceneNum == 8)
            {
                sc8.CloseDoor(2);
            }
        }
    }

    public void CloseUpgradePanel()
    {
        if (currentSceneNum == 2)
        {
            if (sc2.flowIndex <= 3)
                StartCoroutine(sc2.AfterCoolingCannonTut());
            else
            {
                movePermit = true;
                StartCoroutine(DelayToInteractPermit(0.5f));
            }
        }
        else
        {
            movePermit = true;
            StartCoroutine(DelayToInteractPermit(0.5f));
        }
    }

    public void Sleep()
    {
        if (currentSceneNum == 2)
        {
            GameManager.Instance.ActiveStageNum = 3;
            StartCoroutine(uiController.FadeToEndScene(1f, audio_main));
        }
        else if (currentSceneNum == 4)
        {
            if (GameManager.Instance.ActiveStageNum == 4)
            {
                GameManager.Instance.ActiveStageNum = 5;
                StartCoroutine(uiController.FadeToEndScene(1f, audio_main));   
            }
            else if (GameManager.Instance.ActiveStageNum == 5)
            {
                GameManager.Instance.ActiveStageNum = 6;
                StartCoroutine(uiController.FadeToEndScene(1f, audio_fight));
            }
        }
        else if (currentSceneNum == 6)
        {
            if (GameManager.Instance.ActiveStageNum == 7)
            {
                GameManager.Instance.ActiveStageNum = 8;
                StartCoroutine(uiController.FadeToEndScene(1f, audio_main));
            }
            else if (GameManager.Instance.ActiveStageNum == 8)
            {
                GameManager.Instance.ActiveStageNum = 9;
                StartCoroutine(uiController.FadeToEndScene(1f, audio_fight));
            }
        }
        else if (currentSceneNum == 8)
        {
            if (GameManager.Instance.ActiveStageNum == 10)
            {
                StartCoroutine(uiController.FadeInAndOut_SetPlayer(1f, 0.2f, 0.5f, new Vector2(26.62f, -6.6f), new Vector2(1, 0), audio_main,
                    ()=>StartCoroutine(sc8.AfterSleep())));
                sc8.SetMark_Bed(false);
            }
            if(GameManager.Instance.ActiveStageNum == 11)
            {
                StartCoroutine(uiController.FadeToEndScene(1f, audio_fight));
            }
        }
        uiController.CloseSleepPanel(true);
    }

    public void SetPlayer(Vector2 pos, Vector2 dir)
    {
        playerController.transform.position = pos;
        playerController.SetDirectionByOther(dir);
        if (currentSceneNum == 8)
            sc8.SetPassengersFrontDoor();
    }

    public void SetDoor(int index, bool b)
    {
        if(index < doors.Count)
        {
            doors[index].SetDoor(b);
        }
    }

    public void ConvertDoor(int index)
    {
        if (index < doors.Count)
        {
            if(doors[index].isOpen)
                doors[index].SetDoor(false);
            else //close
                doors[index].SetDoor(true);
        }
    }

    public void DestroyDoor(int index)
    {
        if (index < doors.Count)
        {
            doors[index].DestroyDoor();
        }
    }

    public void SetAllDoor(bool b)
    {
        foreach(DoorAndFog door in doors)
        {
            door.SetDoor(b);
        }
    }
    
    public void SetEnemyToChase(int startIndex, int endIndex)
    {
        for(int i = startIndex; i<= endIndex; i++)
        {
            enemies[i].transform.GetChild(0).GetComponent<EnemyController>().ConvertToChase();
        }
    }

    public void ActivateChasingGroup(int _sortNum)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyController enemyCtrl = enemies[i].transform.GetChild(0).GetComponent<EnemyController>();
            if (enemyCtrl.enemySortNum == _sortNum && enemyCtrl._state != Define.State.Chase)
            {
                SetEnemyToChase(i, i);
            }
        }
    }

    public void DeActivateEnemy(int startIndex, int endIndex)
    {
        for (int i = startIndex; i <= endIndex; i++)
        {
            enemies[i].SetActive(false);
        }
    }

    public void ActivateEnemy(int startIndex, int endIndex)
    {
        for (int i = startIndex; i <= endIndex; i++)
        {
            enemies[i].SetActive(true);
        }
    }

    public void GameOver()
    {
        movePermit = false;
        stageState = InFlightStageState.StageEnd;
    }

    public void ShakeCamera()
    {
        //패링 효과에 활용
        StartCoroutine(mainCamera.GetComponent<CameraController>().ShakeCamera(0.05f, 0.2f));
        Time.timeScale = 0.8f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        StartCoroutine(RecoverTimeScale(0.05f));
    }

    public IEnumerator RecoverTimeScale(float duration)
    {
        float startTime = Time.unscaledTime;
        float startScale = Time.timeScale;
        float endScale = 1.0f;

        while (Time.unscaledTime < startTime + duration)
        {
            float t = (Time.unscaledTime - startTime) / duration;
            Time.timeScale = Mathf.Lerp(startScale, endScale, t);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }

        Time.timeScale = endScale;
        Time.fixedDeltaTime = 0.02f;
    }
}
