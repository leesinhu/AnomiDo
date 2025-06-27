using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

//플레이어 모드(비행선 내부, 비행선 외부)
public enum StationStageState
{
    Tutorial,
    StageSetting,
    BeforeOpen,
    AfterOpen,
    GameOver,
    GameClear,
}
public enum PlayerState
{
    inSide,
    OutSide,
}
public enum PassengerState
{
    idle,
    leave,
    freeze,
    fall,
    dead,
}
public class PassManager : MonoBehaviour
{
    //튜토리얼용
    public bool tut_freeze { get; set; } = false;
    public bool tut_playerAct { get; set; } = false;
    public bool tut_InOut { get; set; } = false;
    public bool tut_cameraMove { get; set; } = false;

    ScriptControl_1 sc1;
    ScriptControl_3 sc3;
    ScriptControl_5 sc5;
    ScriptControl_7 sc7;

    //게임 흐름
    int currentSceneNum = 0;
    bool loading = false;

    //UI & 스프라이트/애니메이션
    UIController_Station uiController;

    [SerializeField] Animator anim_control;
    [SerializeField] RuntimeAnimatorController[] colorAnimators;
    [SerializeField] GameObject effect_explosion;
    [SerializeField] Animator anim_cctv;
    [SerializeField] Animator anim_targetMark;

    //사운드
    public AudioSource mainBGM { get; set; }
    float mainBGMVolume;
    AudioSource audioSource_freeze;
    AudioSource audioSource_shot;
    AudioSource audioSource_shot_hit;
    AudioSource audioSource_enter;
    AudioSource audioSource_door1, audioSource_door2;
    AudioSource audioSource_bang;
    AudioSource audioSource_takeOff;

    //대화 관련
    public bool playPermit { get; set; } = false;

    [HideInInspector]
    public StationStageState stageState { get; set; } = StationStageState.Tutorial;

    //플레이어 관련
    GameObject player;
    [HideInInspector]
    public PlayerState playerState { get; set; } = PlayerState.inSide;
    public bool isInGateBoundary { get; set; } = false;

    //카메라 관련
    GameObject mainCamera;
    Vector3 mainCameraPos;
    [SerializeField] float cameraSpeed = 15;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    Vector3 shipCamera_storedPos, charCamera_storedPos;

    //생성할 캐릭터 관련
    public GameObject passengers; 
    List<GameObject> spawnedPassengers = new List<GameObject>();
    float settingMaxTimer = 0;
    float settingDelayTimer = 0;
    public float minHeat, maxHeat;
    [SerializeField] public float heatOffset_init;

    public float spawnDelay;
    public int spawnInitNum;
    bool playerInSpawnPoint = false;
    bool spawnPermit = false;
    Coroutine spawnCoroutine = null;

        //게임 시작 전 대기줄에 승객 배치
    [HideInInspector]
    public bool gameStartSign = false; 

    //대기줄 관련
    public List<GameObject> wayPoints = new List<GameObject>();
    public GameObject gate;
    public bool gateOpenSign { get; set; } = false;
    [SerializeField] AlphaEffectObject[] wayRoad;
    [SerializeField] float roadEffectDuration, roadEffectDelay;
    [SerializeField] Animator anim_enter;
    [SerializeField] DoorMoving door;
    bool isDoorSetting = false;
    Coroutine wayEffectCoroutine;

    //제한 시간
    [SerializeField]
    private float gameLimitTime;
    public float GameLimitTime
    {
        get { return gameLimitTime; }
        set { gameLimitTime = value; }
    }
    [HideInInspector]
    public float gameTimer { get; set; } //남은 시간(변동)*/
    bool timePassPermit = true;

    //무단이탈자(이벤트)
    public event EventHandler OnLeaverPassed;
    [HideInInspector]
    public bool isLeaverPassed = false;

    //승객 관련
    [SerializeField]
    private int passGoalCount;
    public int PassGoalCount
    {
        get { return passGoalCount; }
        set { passGoalCount = value; }
    }
    [HideInInspector]
    private int passCount = 0;
    public int PassCount
    {
        get => passCount > passGoalCount ? passGoalCount : passCount;
        private set => passCount = value;
    }
    //private int fallCount = 0; //넘어진 승객 수
    //public int fallLimitCount; //넘어짐 승객 수 제한(게임오버)

    //냉각포 관련
    [SerializeField] FreezeBullet bullet;
    [SerializeField] Transform[] bulletShotPosition;
    [SerializeField] GameObject bulletShotEffect;
    int shotPosIndex = 3;
    [SerializeField] Animator anim_cannon;

    float bulletMaxGage; //냉각포 최대저장량
    float bulletChargeSpeed; //냉각포 충전속도
    public float lossTempAmount { get; set; } //냉각탄환의 체온감소량

    float bulletGage = 0;
    int bulletNum;

    float shotDelayTimer = 0;

    //클리어 보상
    public int credit;

    private void Awake()
    {
        currentSceneNum = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneNum == 1)
            sc1 = GameObject.Find("ScriptManager").GetComponent<ScriptControl_1>();
        else if (currentSceneNum == 3)
            sc3 = GameObject.Find("ScriptManager").GetComponent<ScriptControl_3>();
        else if (currentSceneNum == 5)
            sc5 = GameObject.Find("ScriptManager").GetComponent<ScriptControl_5>();
        else if (currentSceneNum == 7)
        {
            sc7 = GameObject.Find("ScriptManager").GetComponent<ScriptControl_7>();
            anim_control.SetBool("isSasu", false);
        }
            

        uiController = GameObject.Find("Canvas").GetComponent<UIController_Station>();

        audioSource_freeze = GetComponent<AudioSource>();
        audioSource_shot = transform.GetChild(0).GetComponent<AudioSource>();
        audioSource_shot_hit = transform.GetChild(1).GetComponent<AudioSource>();
        audioSource_enter = transform.GetChild(2).GetComponent<AudioSource>();
        audioSource_door1 = transform.GetChild(3).GetComponent<AudioSource>();
        audioSource_door2 = transform.GetChild(4).GetComponent<AudioSource>();
        audioSource_bang = transform.GetChild(5).GetComponent<AudioSource>();
        audioSource_takeOff = transform.GetChild(6).GetComponent<AudioSource>();

        player = GameObject.Find("Player").gameObject;
        mainCamera = GameObject.Find("Main Camera").gameObject;
        mainCameraPos = mainCamera.transform.position;
        mainBGM = mainCamera.GetComponent<AudioSource>();
        mainBGMVolume = mainBGM.volume;
        gate = GameObject.Find("Gate").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        shipCamera_storedPos = mainCamera.transform.position;
        charCamera_storedPos = mainCamera.transform.position;
        gameTimer = gameLimitTime;

        //GameManager에서 받아서 초기화
        bulletMaxGage = GameManager.Instance.GetUpgradeElement("Max");
        bulletChargeSpeed = GameManager.Instance.GetUpgradeElement("Speed");
        lossTempAmount = GameManager.Instance.GetUpgradeElement("LossAmount");

        bulletGage = bulletMaxGage;
        bulletNum = (int)bulletGage / 4;

        gate.transform.GetChild(0).gameObject.SetActive(false); //게이트 이펙트

        //게임 시작 알림
        GameManager.Instance.gameState = GameState.gamePlaying;

        if (GameManager.Instance.tutSkipStageNum == SceneManager.GetActiveScene().buildIndex)
        {
            StartStageSetting(0);
        }
        else
        {
            if (currentSceneNum == 1)
            {
                GameManager.Instance.ActiveStageNum = 1;
                StartCoroutine(sc1.StartFading());
            }
            else if (currentSceneNum == 3)
            {
                GameManager.Instance.ActiveStageNum = 3;
                StartCoroutine(sc3.StartFading());
            }
            else if (currentSceneNum == 5)
            {
                GameManager.Instance.ActiveStageNum = 6;
                StartCoroutine(sc5.StartFading());
            }
            else if (currentSceneNum == 7)
            {
                GameManager.Instance.ActiveStageNum = 9;
                StartCoroutine(sc7.StartFading());
            }
        }
    }
    void Update()
    {
        //게임 구조
        //초기화
        if (stageState == StationStageState.StageSetting)
        {
            if(spawnedPassengers.Count == 0)
            {
                spawnPermit = true;
                SpawnPassengers();
                Time.fixedDeltaTime = 0.0005f;
            }

            settingMaxTimer += Time.deltaTime;
            settingDelayTimer += Time.deltaTime;
            if (spawnedPassengers.Count >= spawnInitNum && settingMaxTimer > uiController.videoPlayer.clip.length)
            {   
                if(settingDelayTimer > 3f)
                {
                    spawnPermit = false;
                    settingDelayTimer = 0;
                    if (currentSceneNum == 1)
                    {   
                        StartCoroutine(uiController.FadeInAndOut_ConvertStageState(0.5f, 0.2f, 0.5f, StationStageState.BeforeOpen, false, 
                            () => StartCoroutine(sc1.BeforeOpenTalk()))); 
                    } 
                    else if (currentSceneNum == 3)
                    {   
                        StartCoroutine(uiController.FadeInAndOut_ConvertStageState(0.5f, 0.2f, 0.5f, StationStageState.BeforeOpen, false, 
                            () => StartCoroutine(sc3.BeforeOpenTalk()))); 
                    }
                    else if (currentSceneNum == 5)
                    {   
                        StartCoroutine(uiController.FadeInAndOut_ConvertStageState(0.5f, 0.2f, 0.5f, StationStageState.BeforeOpen, false, 
                            () => StartCoroutine(sc5.BeforeOpenTalk()))); }
                    else if (currentSceneNum == 7)
                    {   
                        StartCoroutine(uiController.FadeInAndOut_ConvertStageState(0.5f, 0.2f, 0.5f, StationStageState.BeforeOpen, false,
                            () => StartCoroutine(sc7.BeforeOpenTalk())));
                        gate.transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
            }
        }
        else if(stageState == StationStageState.BeforeOpen)
        {
            Time.fixedDeltaTime = 0.02f;
        }

        //비행선 문 열기(게임 시작)
        if (stageState == StationStageState.BeforeOpen && Input.GetKeyDown(KeyCode.O) && playPermit && !isDoorSetting)
        {
            StartCoroutine(DoorSetting(1, null));
        }

        //게임 오버(게임 타이머)
        if (stageState == StationStageState.AfterOpen && timePassPermit)
        {
            gameTimer -= Time.deltaTime;
            if(gameTimer <= 30 && gameTimer > 29.9f)
            {
                PrintInfo(5);
            }

            if(gameTimer <= 0)
            {
                StartCoroutine(GameManager.Instance.SoundFade(mainBGM, 1.5f, mainBGMVolume, 0.01f));
                gameTimer = 0;
                //Time.timeScale = 0f;
                stageState = StationStageState.GameOver;
            }
        }

        //게임 오버 - 재시작
        if (stageState == StationStageState.GameOver)
        {
            if(currentSceneNum == 1)
            {
                sc1.Info_Fail();
            }
            else if(currentSceneNum == 3)
            {
                sc3.Info_Fail();
            }
            else if (currentSceneNum == 5)
            {
                sc5.Info_Fail();
            }

            if (Input.GetKeyDown(KeyCode.R) && playPermit)
            {
                if(SceneManager.GetActiveScene().buildIndex < 7)
                    uiController.OpenGameOverWindow();
                else
                {
                    GameManager.Instance.tutSkipStageNum = currentSceneNum;
                    StartCoroutine(uiController.FadeToRestartScene(0.5f, mainBGM));
                }
                    
            }
        }

        //게임 클리어 - 다음 스테이지
        if(stageState == StationStageState.GameClear)
        {
            if (Input.GetKeyDown(KeyCode.G) && !loading)
            {
                audioSource_takeOff.Play();
                GameManager.Instance.AddCredit(this.credit);
                if (currentSceneNum == 1)
                {
                    GameManager.Instance.ActiveStageNum = 2;
                    GameManager.Instance.SoundFade(sc1.audio_ambient, 0.5f, sc1.audio_ambient.volume, 0);
                }
                else if (currentSceneNum == 3)
                {
                    GameManager.Instance.ActiveStageNum = 4;
                    GameManager.Instance.SoundFade(sc3.audio_ambient, 0.5f, sc3.audio_ambient.volume, 0);
                }
                else if (currentSceneNum == 5)
                {
                    GameManager.Instance.ActiveStageNum = 7;
                    GameManager.Instance.SoundFade(sc5.audio_ambient, 0.5f, sc5.audio_ambient.volume, 0);
                }
                else if (currentSceneNum == 7)
                {
                    GameManager.Instance.ActiveStageNum = 10;
                    GameManager.Instance.SoundFade(sc7.audio_ambient, 0.5f, sc7.audio_ambient.volume, 0);
                }
                    
                loading = true;
                StartCoroutine(uiController.FadeToEndScene(0.5f, mainBGM));
            }
        }

        //메뉴 열기
        if(Input.GetKeyDown(KeyCode.Escape) && (!uiController.videoPlayer.isPlaying|| stageState != StationStageState.StageSetting))
        {
            uiController.OpenMenu();
        }

        //인게임
        //무단이탈자 이벤트
        if (isLeaverPassed)
        {
            OnLeaverPassed?.Invoke(this, EventArgs.Empty);
            audioSource_bang.Play();
            isLeaverPassed = false;
        }

        //냉각포
            //장전(자동)
        if (stageState == StationStageState.AfterOpen || stageState == StationStageState.Tutorial)
        {
            if (bulletGage < bulletMaxGage)
            {
                bulletGage += Time.deltaTime * bulletChargeSpeed;
            }
            else
            {
                bulletGage = bulletMaxGage;
            }
        }
        bulletNum = (int)bulletGage / 4;

        shotDelayTimer += Time.deltaTime;
            //발사
        if(shotDelayTimer > 0.5f)
        {
            anim_control.SetBool("Button", false);
            if (Input.GetMouseButtonDown(0) && ((stageState == StationStageState.AfterOpen || tut_freeze) && playerState == PlayerState.inSide))
            {
                if (bulletGage >= 4)
                {
                    anim_control.SetBool("Button", true);
                    anim_targetMark.SetTrigger("Shot");

                    // 마우스 위치에서 Raycast를 생성
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Body"));

                    //냉각포 방향 조절
                    Vector2 cannonDir = mousePosition -
                        new Vector2(bulletShotPosition[shotPosIndex].position.x, bulletShotPosition[shotPosIndex].position.y);
                    float cannonAngle = Mathf.Atan2(cannonDir.y, cannonDir.x) * Mathf.Rad2Deg;

                    if (cannonAngle >= 0 && cannonAngle < 112.5f)
                    {
                        anim_cannon.SetInteger("Angle", 90);
                        shotPosIndex = 0;
                    }
                    else if (cannonAngle >= 112.5f && cannonAngle < 157.5f)
                    {
                        anim_cannon.SetInteger("Angle", 135);
                        shotPosIndex = 1;
                    }
                    else if (cannonAngle >= 157.5f || cannonAngle < -157.5f)
                    {
                        anim_cannon.SetInteger("Angle", 180);
                        shotPosIndex = 2;
                    }
                    else if (cannonAngle >= -157.5f && cannonAngle < -112.5f)
                    {
                        anim_cannon.SetInteger("Angle", 225);
                        shotPosIndex = 3;
                    }
                    else if (cannonAngle >= -112.5f && cannonAngle < 0)
                    {
                        anim_cannon.SetInteger("Angle", 270);
                        shotPosIndex = 4;
                    }

                    //냉각탄환 발사
                    FreezeBullet freezeBullet = Instantiate(bullet, bulletShotPosition[shotPosIndex].position, Quaternion.identity);
                    freezeBullet.targetPos = mousePosition;
                    freezeBullet.passManager = this;
                    bulletShotEffect.transform.position = bulletShotPosition[shotPosIndex].position;
                    PrintBulletShotEffect();
                    bulletGage -= 4;
                    shotDelayTimer = 0;
                }
                else if(stageState != StationStageState.Tutorial)
                {
                    PrintInfo(2);
                }
            }
        }

        //카메라 이동
        if ((stageState == StationStageState.AfterOpen && playerState == PlayerState.inSide) || tut_cameraMove)
        {
            //내부
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            anim_control.SetFloat("moveX", horizontal);
            anim_control.SetFloat("moveY", vertical);

            Vector3 targetPosition = mainCamera.transform.position + new Vector3(horizontal, vertical, 0) * cameraSpeed * Time.deltaTime;
            
            if (targetPosition.x >= minBounds.x && targetPosition.x <= maxBounds.x 
                && targetPosition.y <=maxBounds.y && targetPosition.y >= minBounds.y)
            {
                mainCamera.transform.position = targetPosition;
            }
            shipCamera_storedPos = mainCamera.transform.position;

            if (mainCamera.transform.position.x > minBounds.x + cameraSpeed * Time.deltaTime) { uiController.SetCameraPoints(2, true); }
            else { uiController.SetCameraPoints(2, false); }
            if (mainCamera.transform.position.x < maxBounds.x - cameraSpeed * Time.deltaTime) { uiController.SetCameraPoints(3, true); }
            else { uiController.SetCameraPoints(3, false); }
            if (mainCamera.transform.position.y > minBounds.y + cameraSpeed * Time.deltaTime) { uiController.SetCameraPoints(1, true); }
            else { uiController.SetCameraPoints(1, false); }
            if (mainCamera.transform.position.y < maxBounds.y - cameraSpeed * Time.deltaTime) { uiController.SetCameraPoints(0, true); }
            else { uiController.SetCameraPoints(0, false); }
        }
        else
        {
            uiController.SetCameraPoints(0, false);
            uiController.SetCameraPoints(1, false);
            uiController.SetCameraPoints(2, false);
            uiController.SetCameraPoints(3, false);
        }

        if((gateOpenSign && playerState == PlayerState.OutSide) || tut_playerAct)
        {
            //외부
            Vector3 cameraPosition = mainCamera.transform.position;
            cameraPosition.x = player.transform.position.x;
            cameraPosition.y = player.transform.position.y;

            if (cameraPosition.x <= minBounds.x)
            {
                cameraPosition.x = minBounds.x;
            }
            else if(cameraPosition.x >= maxBounds.x)
            {
                cameraPosition.x = maxBounds.x;
            }

            if(cameraPosition.y <= minBounds.y)
            {
                cameraPosition.y = minBounds.y;
            }
            else if(cameraPosition.y >= maxBounds.y)
            {
                cameraPosition.y = maxBounds.y;
            }

            mainCamera.transform.position = cameraPosition;
            charCamera_storedPos = mainCamera.transform.position;
        }
        
        //내외부 전환
        if (Input.GetKeyDown(KeyCode.Space) && ((stageState == StationStageState.AfterOpen) || tut_InOut))
        {
            if(passCount < passGoalCount)
            {
                if (playerState == PlayerState.inSide)
                {
                    if (SceneManager.GetActiveScene().buildIndex < 7)
                    {
                        playerState = PlayerState.OutSide;
                        if (tut_InOut) player.GetComponent<Player>().PlayerSpawn();
                        mainCamera.transform.position = charCamera_storedPos;
                    }
                    else
                    {
                        SetPlayer();
                        playerState = PlayerState.OutSide;
                        player.GetComponent<Player>().PlayerSpawn();
                        mainCamera.transform.position = charCamera_storedPos;
                    }
                }
                else if (!tut_InOut)
                {
                    if (SceneManager.GetActiveScene().buildIndex < 7)
                    {
                        playerState = PlayerState.inSide;
                        mainCamera.transform.position = shipCamera_storedPos;
                    }
                    else if (SceneManager.GetActiveScene().buildIndex == 7)
                    {
                        if(isInGateBoundary)
                        {
                            SetPlayerToOutSide();
                            playerState = PlayerState.inSide;
                            mainCamera.transform.position = shipCamera_storedPos;
                        }
                        else
                        {
                            PrintInfo(14);
                        }
                    }
                    anim_targetMark.SetTrigger("Reset");
                }
            }
            else //정원 탑승 시
            {
                if (((playerState == PlayerState.OutSide && isInGateBoundary) || (playerState == PlayerState.inSide && currentSceneNum == 7)) 
                    && !isDoorSetting)
                {
                    StartCoroutine(DoorSetting(0, null));
                }
            }

            Vector3 fixedPos = mainCamera.transform.position;

            if (fixedPos.x < minBounds.x)
                fixedPos.x = minBounds.x;
            else if (fixedPos.x > maxBounds.x)
                fixedPos.x = maxBounds.x;

            if (fixedPos.y < minBounds.y)
                fixedPos.y = minBounds.y;
            else if (fixedPos.y > maxBounds.y)
                fixedPos.y = maxBounds.y;

            mainCamera.transform.position = fixedPos;
        }

        if(playerState == PlayerState.inSide)
        {
            anim_cannon.SetBool("Heat", true);
            for (int i = 0; i < wayRoad.Length; i++)
                wayRoad[i].GetComponent<SpriteRenderer>().sortingOrder = -5;
        }
        else //outside
        {
            anim_cannon.SetBool("Heat", false);
            for (int i = 0; i < wayRoad.Length; i++)
                wayRoad[i].GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < spawnedPassengers.Count; i++)
        {
            Destroy(spawnedPassengers[i].gameObject);
        }
        spawnedPassengers.Clear();

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    public bool InvokeLeaverEvent()
    {
        int attemptNum = 0;
        Passenger target = null;
        if (spawnedPassengers.Count < 10)
            return false;

        do
        {    
            int randomIndex = UnityEngine.Random.Range(9, spawnedPassengers.Count - 1);
            target = spawnedPassengers[randomIndex].GetComponent<Passenger>();

            attemptNum++;
        } while (target.passState != PassengerState.idle && attemptNum <= spawnedPassengers.Count - 1);

        if(attemptNum <= spawnedPassengers.Count - 1 && target != null)
        {
            //무단이탈자로 전향
            StartCoroutine(target.BornToLeaver());
            PrintInfo(4);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveSpawnedPassengers(GameObject obj)
    {
        Passenger passenger = obj.GetComponent<Passenger>();
        if (passenger != null && passenger.passState == PassengerState.dead && !passenger.isLeft)
        {
            PrintInfo(1);
        }

        spawnedPassengers.Remove(obj);
    }

    public void AddPassCount(int i)
    {
        if (stageState == StationStageState.GameOver)
            return;
        passCount += i;

        if(passCount == passGoalCount - 5)
        {
            PrintInfo(6);
        }

        if (passCount >= passGoalCount && timePassPermit)
        {
            passCount = passGoalCount;
            timePassPermit = false;
            gate.transform.GetChild(0).gameObject.SetActive(false);
            gate.transform.GetChild(1).gameObject.SetActive(true);
            uiController.ShowPassComplete();

            if (currentSceneNum == 1)
            {
                StartCoroutine(sc1.Info_Recall());
            }
            else if (currentSceneNum == 3)
            {
                StartCoroutine(sc3.Info_Recall());
            }
            else if (currentSceneNum == 5)
            {
                StartCoroutine(sc5.Info_Recall());
            }

            if (playerState == PlayerState.inSide && currentSceneNum < 7)
            {
                playerState = PlayerState.OutSide;
                mainCamera.transform.position = charCamera_storedPos;
            }
        }
    }

    public void StartStageSetting(float fadeTime = 0.5f)
    {
        //튜토리얼 종료 후 스테이지 준비
        settingMaxTimer = 0;
        settingDelayTimer = 0;
        StartCoroutine(uiController.FadeInAndOut_ConvertStageState(fadeTime, 0.2f, 0.5f, StationStageState.StageSetting, true, () => EmptyFunc()));
    }

    public IEnumerator DoorSetting(int i, System.Action onComplete)
    {
        if(i == 1) //문 열기
        {
            isDoorSetting = true;
            uiController.CloseOpenGuide();
            audioSource_door1.Play();
            door.OpenDoor();
            yield return new WaitUntil(() => door.hasOpened);
            audioSource_door2.Play();
            gate.transform.GetChild(0).gameObject.SetActive(true);

            if(currentSceneNum < 7) //마지막 정거장에서는 플레이어 혼자
                SetPlayer();

            yield return new WaitForSeconds(1f);
            stageState = StationStageState.AfterOpen;
            gateOpenSign = true;
            spawnPermit = true;
            StartCoroutine(WayRoadEffect());

            StartCoroutine(GameManager.Instance.SoundFade(mainBGM, 1.5f, 0, mainBGMVolume));
            mainBGM.Play();
            isDoorSetting = false;
        }
        else if(i ==0 )//문 닫기
        {
            isDoorSetting = true;
            StartCoroutine(GameManager.Instance.SoundFade(mainBGM, 1.5f, mainBGMVolume, 0.01f));

            audioSource_door1.Play();
            door.CloseDoor();
            SetPlayerToOutSide();
            gate.transform.GetChild(1).gameObject.SetActive(false);
            gateOpenSign = false;

            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => !door.hasOpened);
            audioSource_door2.Play();
            stageState = StationStageState.GameClear;
            isDoorSetting = false;
        }
        else if(i == 2)//연출용 문 열기
        {
            audioSource_door1.Play();
            door.OpenDoor();
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => door.hasOpened);
            audioSource_door2.Play();
            yield return new WaitForSeconds(0.5f);
        }
        else if(i ==3) //연출용 문 닫기
        {
            door.CloseDoor(0.1f);
        }
        else if (i == 4) //연출용 문 닫기
        {
            door.OpenDoor(0.1f);
        }
        onComplete?.Invoke();
    }

    private IEnumerator SpawnPassengersWithDelay(float delayTime = 0)
    {
        yield return new WaitForSeconds(delayTime);
        yield return new WaitUntil(() => !playerInSpawnPoint && spawnPermit);
        SpawnPassengers();
        spawnCoroutine = null;
    }

    private void SpawnPassengers()
    {
        if (colorAnimators != null)
        {
            RuntimeAnimatorController tempAnim;
            tempAnim = colorAnimators[UnityEngine.Random.Range(0, colorAnimators.Length)];
            passengers.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = tempAnim;
        }
        GameObject spawned = Instantiate(passengers, transform.position, Quaternion.identity);
        spawnedPassengers.Add(spawned);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerFoot"))
        {
            playerInSpawnPoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!enabled) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerFoot"))
        {
            playerInSpawnPoint = false;
        }

        if (spawnCoroutine == null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Back"))
            {
                Passenger target = collision.gameObject.GetComponentInParent<Passenger>();
                if (target == null) return;
                if (target.ignoreExitOneTime)
                {
                    target.ignoreExitOneTime = false;
                    return;
                }


                if (stageState == StationStageState.StageSetting)
                {
                    spawnCoroutine = StartCoroutine(SpawnPassengersWithDelay(0.1f));
                }
                else
                {
                    spawnCoroutine = StartCoroutine(SpawnPassengersWithDelay(spawnDelay));
                }
            }
        }
    }

    public Vector3 GetMainCameraPos()
    {
        return mainCamera.transform.position;
    }

    public void SetMainCameraPos()
    {
        mainCamera.transform.position = mainCameraPos;
    }

    public void PrintBulletHitEffects(Vector3 printPos)
    {
        StartCoroutine(ShakeCamera(0.1f, 0.1f));

        Instantiate(effect_explosion, printPos, Quaternion.identity);
        audioSource_shot_hit.PlayOneShot(audioSource_shot_hit.clip);
        audioSource_freeze.PlayOneShot(audioSource_freeze.clip);
    }

    void PrintBulletShotEffect()
    {
        audioSource_shot.PlayOneShot(audioSource_shot.clip);
        bulletShotEffect.GetComponent<Animator>().SetTrigger("Shot");
    }

    public void PrintEnterGateEffect()
    {
        anim_enter.SetTrigger("EnterGate");
        audioSource_enter.PlayOneShot(audioSource_enter.clip);
    }

    IEnumerator WayRoadEffect()
    {
        int index = 0;
        while(stageState <= StationStageState.AfterOpen)
        {
            index = index % (wayRoad.Length);
            wayRoad[index].AlphaInAndOutEffect(roadEffectDuration, 0.7f, 0);
            yield return new WaitForSeconds(roadEffectDelay);
            index += 1;
        }
    }

    public IEnumerator ShakeCamera(float duration = 0.1f, float magnitudePos = 0.03f)
    {
        Transform shakeCamera = mainCamera.transform;
        Vector3 originPos = shakeCamera.position;
        float passTime = 0.0f;
        while (passTime < duration)
        {
            originPos = shakeCamera.position;

            Vector3 shakePos = (Vector3)UnityEngine.Random.insideUnitCircle;
            //shakePos.z = originPos.z;
            shakeCamera.position = originPos + shakePos * magnitudePos;

            passTime += Time.deltaTime;
            yield return null;
            shakeCamera.position = originPos;
        }

        shakeCamera.position = originPos;
        Vector3 tempPos = shakeCamera.position;
        yield return null;

        if (shakeCamera.position.x < minBounds.x)
            tempPos.x = minBounds.x;
        else if (shakeCamera.position.x > maxBounds.x)
            tempPos.x = maxBounds.x;

        if (shakeCamera.position.y < minBounds.y)
            tempPos.y = minBounds.y;
        else if (shakeCamera.position.y > maxBounds.y)
            tempPos.y = maxBounds.y;

        shakeCamera.position = tempPos;
    }


    public Vector3 GetPlayerPos()
    {
        return player.transform.position;
    }

    public void SetPlayerPos(Vector3 pos = default)
    {
        if (pos == default)
            pos = Vector3.zero;
        player.transform.position = pos;
    }

    public void PlayerIsInGate()
    {
        isInGateBoundary = true;
        if(currentSceneNum < 7)
        {
            if (passCount >= passGoalCount)
                uiController.ShowInteractGuide(true);
        }
        else
            uiController.ShowInteractGuide(true);
    }

    public void PlayerIsNotInGate()
    {
        isInGateBoundary = false;
        if (currentSceneNum < 7)
        {
            if (passCount >= passGoalCount)
                uiController.ShowInteractGuide(false);
        }
        else
            uiController.ShowInteractGuide(false);
    }

    public int GetBulletNum()
    {
        return bulletNum;
    }

    public float[] GetBulletGage()
    {
        float[] temp = new float[] { bulletGage, bulletMaxGage };
        return temp;
    }

    public void SetBulletGage(float offset)
    {
        bulletGage += offset;
    }

    /*void OnDisable()
   {
       for (int i = 0; i < spawnedPassengers.Count; i++)
       {
           if (spawnedPassengers[i] != null)
           {
               Destroy(spawnedPassengers[i]);  // 프리팹 삭제
           }
       }
       spawnedPassengers.Clear();  // 리스트 초기화
   }*/

    void EmptyFunc()
    {
        //Empty
    }

    public void SetPlayer()
    {
        player.SetActive(true);
        player.GetComponent<Player>().PlayerSpawn();
    }

    public void SetPlayerToOutSide()
    {
        SetPlayerPos(new Vector3(13.5f, -5.8f, -999f));
    }

    public void PrintInfo(int i)
    {
        if (currentSceneNum == 1)
        {
            if (i == 1 || i == 2 || i == 3 || i == 5 || i == 12) 
                { StartCoroutine(sc1.PrintInfoText(i)); }
        }
        else if (currentSceneNum == 3)
        {
            StartCoroutine(sc3.PrintInfoText(i));
        }
        else if (currentSceneNum == 5)
        {
            StartCoroutine(sc5.PrintInfoText(i));
        }
        else if (currentSceneNum == 7)
        {
            if (i == 14)
                StartCoroutine(sc7.PrintInfoText_NoName(i));
        }
    }

    public void convertController(int i)
    {
        if(i == 0)
        {
            anim_control.SetBool("isSasu", false);
        }
        else
        {
            anim_control.SetBool("isSasu", true);
        }
    }

    public void TutReset()
    {
        tut_cameraMove = false;
        tut_freeze = false;
        tut_InOut = false;
        tut_playerAct = false;
    }

    public void RestartThisStage_NotTut()
    {
        playPermit = false;
        uiController.CloseGameOverWindowAndMenu();
        uiController.OpenMenu(false);
        GameManager.Instance.tutSkipStageNum = currentSceneNum;
        if (currentSceneNum == 1)
        {
            GameManager.Instance.SoundFade(sc1.audio_ambient, 0.5f, sc1.audio_ambient.volume, 0);
        }
        else if (currentSceneNum == 3)
        {
            GameManager.Instance.SoundFade(sc3.audio_ambient, 0.5f, sc3.audio_ambient.volume, 0);
        }
        else if (currentSceneNum == 5)
        {
            GameManager.Instance.SoundFade(sc5.audio_ambient, 0.5f, sc5.audio_ambient.volume, 0);
        }
        else if (currentSceneNum == 7)
        {
            GameManager.Instance.SoundFade(sc7.audio_ambient, 0.5f, sc7.audio_ambient.volume, 0);
        }
        StartCoroutine(uiController.FadeToRestartScene(0.5f, mainBGM));
    }

    public void RestartThisStage_DoTut()
    {
        playPermit = false;
        uiController.CloseGameOverWindowAndMenu();
        uiController.OpenMenu(false);
        GameManager.Instance.tutSkipStageNum = 0;
        if (currentSceneNum == 1)
        {
            GameManager.Instance.SoundFade(sc1.audio_ambient, 0.5f, sc1.audio_ambient.volume, 0);
        }
        else if (currentSceneNum == 3)
        {
            GameManager.Instance.SoundFade(sc3.audio_ambient, 0.5f, sc3.audio_ambient.volume, 0);
        }
        else if (currentSceneNum == 5)
        {
            GameManager.Instance.SoundFade(sc5.audio_ambient, 0.5f, sc5.audio_ambient.volume, 0);
        }
        else if (currentSceneNum == 7)
        {
            GameManager.Instance.SoundFade(sc7.audio_ambient, 0.5f, sc7.audio_ambient.volume, 0);
        }
        StartCoroutine(uiController.FadeToRestartScene(0.5f, mainBGM));
    }
}
