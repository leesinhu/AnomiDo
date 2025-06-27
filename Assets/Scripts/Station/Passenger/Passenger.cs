using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;
using TMPro;

/* Inspector에 표시되는 public 변수는 public 명시, 
 * Get과 Set 함수로 조정되는 private 변수는 private 명시
 * 나머지는 private 생략
 */

public class Passenger : MonoBehaviour
{
    PassManager passManager;
    public Player player;
    Transform bodyCollider;
    Transform frontCollider;
    Transform footCollider;
    Transform shadow;
    [SerializeField] Sprite shadow_normal, shadow_heat;

    Vector3 gatePos;
    Rigidbody2D rb;
    NavMeshAgent navAgent;

    public SpriteRenderer spRender_normal { get; private set; }
    public SpriteRenderer spRender_heat0 { get; private set; }
    public SpriteRenderer spRender_heat1 { get; private set; }
    public SpriteRenderer spRender_heat2 { get; private set; }
    public SpriteRenderer spRender_heat3 { get; private set; }
    public SpriteRenderer spRender_heat4 { get; private set; }
    Animator anim_normal, anim_heat0, anim_heat1, anim_heat2, anim_heat3, anim_heat4;
    int fixedSortingOrder = 0;

    //Sound
    AudioClip clip_danger;

    //State
    public PassengerState passState = PassengerState.idle;
    public bool isLeft { get; set; } = false; //대기줄 이탈 여부

    //이동 관련
    public float minSpeed, maxSpeed; 
    public float moveSpeed = 0;
    Vector2 movement; //실제 이동을 위해
    public Vector2 dir { get; private set; } //바라보는 방향
    int dirIndex = 0;
    int targetPosIndex = 0; //대기줄의 지점들 

         //멈춰있는 시간 계산
    float waitTime = 0;
    Vector3 lastPosition;

        //멈췄다가 이동 시작할 때까지의 딜레이
    public float delayTime; 
    float delayTimer;

    //체온 관련
    public float heat;
    float heatOffset;

        //냉각총
    [SerializeField]
    private float freezeTime;
    float freezeTimer;
    float lossTempAmount;
    public GameObject icePiece; //얼음 깨진 후 얼음 조각
    Vector3[] directions = new Vector3[]
        {
            Vector3.up,
            new Vector3(1, 1, 0).normalized,
            Vector3.right,
            new Vector3(1, -1, 0).normalized,
            Vector3.down,
            new Vector3(-1, -1, 0).normalized,
            Vector3.left,
            new Vector3(-1, 1, 0).normalized
        };
    Coroutine coroutine = null;
    Animator anim_iceBreak;

    //짜증 시스템(짜증은 총 3단계. 짜증 단계와 체온 상승 속도 비례
    bool isAnger;
    public float angerInterval; //짜증 주기 : 주기마다 확률적으로 짜증
    public float angerProb_init;
    float angerProb; //짜증 확률
    float angerTimer;
    
    //UI
    GameObject canvas;
    GameObject angerMark, dangerMark;

    //충돌 관련 처리
    HashSet<GameObject> frontTarget;
    private bool isWaitingToGate = false;
    public bool ignoreExitOneTime { get; set; } = false;

        //뒤로 넘어지는지 여부
    public bool reverse { get; set; } = false;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas_Prefab");
        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
        bodyCollider = transform.GetChild(6);
        frontCollider = transform.GetChild(7);
        footCollider = transform.GetChild(8);
        angerMark = transform.GetChild(9).gameObject;
        dangerMark = transform.GetChild(10).gameObject;
        shadow = transform.GetChild(11);

        heatOffset = passManager.heatOffset_init;
        freezeTimer = 0;

        isAnger = false;
        angerProb = angerProb_init;
        angerTimer = 0;

        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        navAgent.enabled = false;

        spRender_normal = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spRender_heat0 = transform.GetChild(1).GetComponent<SpriteRenderer>();
        spRender_heat1 = transform.GetChild(2).GetComponent<SpriteRenderer>();
        spRender_heat2 = transform.GetChild(3).GetComponent<SpriteRenderer>();
        spRender_heat3 = transform.GetChild(4).GetComponent<SpriteRenderer>();
        spRender_heat4 = transform.GetChild(5).GetComponent<SpriteRenderer>();
        anim_normal = transform.GetChild(0).GetComponent<Animator>();
        anim_heat0 = transform.GetChild(1).GetComponent<Animator>();
        anim_heat1 = transform.GetChild(2).GetComponent<Animator>();
        anim_heat2 = transform.GetChild(3).GetComponent<Animator>();
        anim_heat3 = transform.GetChild(4).GetComponent<Animator>();
        anim_heat4 = transform.GetChild(5).GetComponent<Animator>();
        anim_iceBreak = transform.GetChild(12).GetComponent<Animator>();

        clip_danger = Resources.Load<AudioClip>("Sound/" + "Danger");

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gatePos = passManager.gate.transform.position;
        lastPosition = transform.position;

        heat = UnityEngine.Random.Range(passManager.minHeat, passManager.maxHeat);
        lossTempAmount = passManager.lossTempAmount;

        passManager.OnLeaverPassed += Anger;

        delayTimer = delayTime;

        frontTarget = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        SetSpriteRenderer(passManager.playerState, heat);  
            
        //초기화 시 이동속도 조절
        if (passManager.stageState == StationStageState.StageSetting)
        {
            moveSpeed = 75.0f;
        }
        else
        {
            if(passState != PassengerState.leave)
            {
                moveSpeed = Mathf.Lerp(maxSpeed, minSpeed, heat / 100.0f);
            }
            else //무단이탈 상태
            {
                navAgent.speed = Mathf.Lerp(maxSpeed, minSpeed, heat / 100.0f);
            }
        }


        //넘어짐
            //과열에 의한 넘어짐
        if (heat >= 100.0f && passState < PassengerState.fall)
        {
            heat = 100;
            passState = PassengerState.fall;
            ReserveNewSortingOrder(-1);
        }

        if (passState == PassengerState.fall)
        {
            SetAnimBoolParam("isFall", true);
            if(isLeft && navAgent.enabled)
                DeactivateNavMeshAgent();
        }

        //적외선 카메라 상에서 그림자 숨기기
        if(passState < PassengerState.dead)
        {
            if (passManager.playerState == PlayerState.inSide)
            {
                shadow.GetComponent<SpriteRenderer>().sprite = shadow_heat;
            }
            else
            {
                shadow.GetComponent<SpriteRenderer>().sprite = shadow_normal;
            }
        }
        else
        {
            shadow.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        //냉각 효과 + 지속시간에 따른 해제
        if (passState == PassengerState.freeze)
        {
            if (isLeft) //무단이탈 상태
            {
                DeactivateNavMeshAgent();
            }

            freezeTimer += Time.fixedDeltaTime;
            if((freezeTime - freezeTimer <= 1.5f && freezeTime - freezeTimer > 1.4f) && coroutine == null)
            {
                passManager.PrintInfo(11);
                coroutine = StartCoroutine(ShakeDominian(0.2f, 0.1f));
            }
            else if((freezeTime - freezeTimer <= 1.0f && freezeTime - freezeTimer > 0.9f) && coroutine == null)
            {
                coroutine = StartCoroutine(ShakeDominian(0.2f, 0.1f));
            }
            else if(freezeTime - freezeTimer <= 0)
            {
                if(!isLeft)
                {
                    passState = PassengerState.idle;
                }
                else
                {
                    passState = PassengerState.leave;
                    ActivateNavMeshAgent();
                }

                SetAnimBoolParam("isFreeze", false);
                freezeTimer = 0;

                /*//얼음 조각 생성
                int randomNum = Random.Range(1, 4);
                Vector3[] randomDirection = new Vector3[randomNum];
                for (int i = 0; i < randomNum; i++)
                {
                    int randomIndex = Random.Range(0, directions.Length);
                    int j = 0;
                    //방향 중복 방지
                    while (j < i)
                    {
                        if (directions[randomIndex] != randomDirection[j])
                        {
                            j++;
                        }
                        else
                        {
                            randomIndex = Random.Range(0, directions.Length);
                            j = 0;
                        }
                    }
                    randomDirection[i] = directions[randomIndex];
                    Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 1) + randomDirection[i] * 2f;
                    Instantiate(icePiece, spawnPosition, Quaternion.identity);
                }*/
            }
        }

        //체온 & 짜증
            //짜증 매커니즘
        /*if (!isGoingToGate && passManager.gameStartSign && !isFreeze && !isAnger & !isGoingToGate)
        { 
            angerTimer += Time.fixedDeltaTime;

            if (angerTimer >= angerInterval)
            {
                if (Random.Range(0f, 1f) < angerProb)
                {
                    heatOffset *= 2;
                    StartCoroutine(ShowEffectForDuration(angerMark, 2.0f));
                }
                angerTimer = 0;
            }
        }*/

            //오래 기다릴수록 짜증 확률 증가
       /* Vector3 deltaPos = transform.position - lastPosition;
        if (deltaPos.magnitude == 0 && !isFreeze) //멈춰있을 때(냉각 제외)
        {
            waitTimer += Time.fixedDeltaTime;
            if (waitTimer >= 1.0f)
            {
                angerProb += 0.001f;
                waitTimer = 0;
            }
        }
        lastPosition = transform.position;*/

        
        //체온 상승/하락
        if(passManager.stageState == StationStageState.AfterOpen)
        {
            if (passState < PassengerState.fall)
            {
                if (passState != PassengerState.freeze)
                {
                    heat += heatOffset;
                }
                else
                {
                    if (heat > 0)
                    {
                        heat -= lossTempAmount;
                    }
                    else
                    {
                        heat = 0;
                    }
                }
            }
            else if (passState >= PassengerState.dead && heat <= 100)
            {
                heat -= heatOffset * 2;
                if(heat < 0)
                {
                    heat = 0;
                }
            }
        }
        
        
        //움직임
            //이동 방향 설정
        if(passState == PassengerState.idle)
        {
            if (targetPosIndex <= passManager.wayPoints.Count - 1)
            {
                movement = new Vector2(passManager.wayPoints[targetPosIndex].transform.position.x - transform.position.x ,
                    passManager.wayPoints[targetPosIndex].transform.position.y - transform.position.y).normalized;
                Vector3 wayPos = passManager.wayPoints[targetPosIndex].transform.position;
                //각 지점 도착
                if ((new Vector2(wayPos.x, wayPos.y) - new Vector2(transform.position.x, transform.position.y)).sqrMagnitude < 0.025f)
                {
                    targetPosIndex += 1;
                }
            }
            else
            {   //게이트로
                movement = new Vector2(gatePos.x - transform.position.x , gatePos.y - transform.position.y).normalized;
                if (passManager.gateOpenSign == true)
                {
                    isWaitingToGate = false;
                }
                else
                {
                    isWaitingToGate = true;
                }
            }
        }
        
            //movement(일반)
        if (passState == PassengerState.idle && movement != Vector2.zero && frontTarget.Count == 0 && !isWaitingToGate)
        {
            waitTime = 0;
            delayTimer += Time.fixedDeltaTime;
            if (delayTimer > delayTime)
            {
                UpdateSpriteDirection(movement);
                SetAnimBoolParam("isWalk", true);
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            SetAnimBoolParam("isWalk", false);
            delayTimer = 0.0f;
            if(passManager.stageState == StationStageState.AfterOpen && passState < PassengerState.fall)
            {
                waitTime += Time.fixedDeltaTime;
                if (waitTime >= 20.0f) { passManager.PrintInfo(12); }
            }
        }

            //movement(무단이탈자)
        if(passState == PassengerState.leave && !isWaitingToGate && passManager.gateOpenSign == true)
        {
            Vector3 agentDir = navAgent.velocity.normalized;
            UpdateSpriteDirection(agentDir);
            SetAnimBoolParam("isWalk", true);
            
            //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            DeactivateNavMeshAgent();
        }

        //게이트 도달
        if ((new Vector2(gatePos.x, gatePos.y) - new Vector2(transform.position.x, transform.position.y)).sqrMagnitude < 0.025f)
        {
            if (passState == PassengerState.leave)
            {
                //무단이탈자의 게이트 통과
                passManager.isLeaverPassed = true;
                passManager.PrintInfo(13);
            }
            passManager.PrintEnterGateEffect();
            passManager.AddPassCount(1);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        //passenger가 destroy되는 경우는 게이트 통과할 때밖에 없음
        if (passManager != null)
        {
            passManager.RemoveSpawnedPassengers(this.gameObject);
            passManager.OnLeaverPassed -= Anger;
        }
    }

    public void Freeze()
    {
        passState = PassengerState.freeze;
        SetAnimBoolParam("isFreeze", true);
        //heat -= lossAmount;
        //heatOffset = passManager.heatOffset_init;
        //angerProb = angerProb_init;
        //for (int i = 0; i < angerMark.Length; i++)
        //{
        //    angerMark[i].gameObject.SetActive(false);
        //}
        //for (int i = 0; i < angerMark.Length; i++)
        //{
        //    int randomIndex = Random.Range(0, angerMark.Length - i);
        //    GameObject temp = angerMark[i];
        //    angerMark[i] = angerMark[randomIndex];
        //    angerMark[randomIndex] = temp;
        //}
    }

    public void UnFreeze()
    {
        if (!isLeft)
        {
            passState = PassengerState.idle;
        }
        else 
        {
            passState = PassengerState.leave;
            ActivateNavMeshAgent();
        }

        SetAnimBoolParam("isFreeze", false);
        anim_iceBreak.SetTrigger("iceBreak");
    }

    private IEnumerator ShakeDominian(float duration = 0.5f, float magnitude = 0.1f)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        coroutine = null;
    }

    public void FallByOther(Vector2 otherDir, PassengerState state = PassengerState.fall)
    {
        if(state != PassengerState.dead)
        {
            float dirDot = Vector2.Dot(this.dir, otherDir);
            if (dirDot > 0)
            {
                passState = PassengerState.fall;
            }
            else if (dirDot < 0)
            {
                SetAnimBoolParam("reverse", true);
                reverse = true;
                Vector2 tempDir = dir;
                passState = PassengerState.fall;
                dir = new Vector2(tempDir.x * (-1), tempDir.y * (-1));
                UpdateSpriteDirection(dir);
            }
            else
            {
                SetAnimBoolParam("reverse", true);
                reverse = true;
                passState = PassengerState.fall;
                if (state != PassengerState.dead)
                {
                    UpdateSpriteDirection(otherDir);
                }
            }
        }
        else //넘어진 도미노로 칠 경우
        {
            float dirDot = Vector2.Dot(this.dir, player.dir);
            if (dirDot < 0)
            {
                //바라보는 방향으로 넘어지기
                passState = PassengerState.fall;
            }
            else if (dirDot > 0)
            {
                //바라보는 방향의 반대 방향으로 넘어지기
                SetAnimBoolParam("reverse", true);
                reverse = true;
                passState = PassengerState.fall;

                Vector2 tempDir = this.dir;
                dir = new Vector2(tempDir.x * (-1), tempDir.y * (-1));
                UpdateSpriteDirection(dir);
            }
            else
            {
                passState = PassengerState.fall;

                dir = new Vector2(player.dir.x * (-1), player.dir.y * (-1));
                UpdateSpriteDirection(dir);
            }
        }
    }

    public void AddFrontTarget(GameObject target)
    {
        frontTarget.Add(target);
    }

    public void RemoveFrontTarget(GameObject target)
    {
        frontTarget.Remove(target);
    }

    public IEnumerator BornToLeaver()
    {
        dangerMark.SetActive(true);
        dangerMark.GetComponent<AudioSource>().PlayOneShot(clip_danger);

        yield return new WaitForSeconds(3);
        yield return new WaitUntil(() => (passState != PassengerState.freeze));
        if(passState == PassengerState.idle)
        {
            passState = PassengerState.leave;
            isLeft = true;
            ActivateNavMeshAgent();
        }
    }


    public void FallFront()
    {
        frontCollider.GetComponent<Passenger_Collider_Front>().isFalling = true;
        PolygonCollider2D frontCol = frontCollider.GetComponent<PolygonCollider2D>();
        PolygonCollider2D tempFront = frontCollider.GetChild(dirIndex).GetComponent<PolygonCollider2D>();
        frontCol.points = tempFront.GetComponent<PolygonCollider2D>().points;
    }

    public void Dead()
    {
        if (angerMark != null)
            angerMark.SetActive(false);
        if (dangerMark != null)
            dangerMark.SetActive(false);

        //맨 땅에 넘어진 승객의 order 설정(최종)
        if (spRender_normal.sortingOrder > -1)
        {
            FixSortingOrder(-1);
            AudioSource source = bodyCollider.GetComponent<AudioSource>();
            source.Play();
        }

        ColliderFixing();
        bodyCollider.GetComponent<PolygonCollider2D>().isTrigger = false;
        passState = PassengerState.dead;
        passManager.RemoveSpawnedPassengers(this.gameObject);
    }

    void ColliderFixing()
    {
        PolygonCollider2D bodyCol = bodyCollider.GetComponent<PolygonCollider2D>();
        PolygonCollider2D footCol = footCollider.GetComponent<PolygonCollider2D>();

        PolygonCollider2D tempBody = bodyCollider.GetChild(dirIndex).GetComponent<PolygonCollider2D>();
        PolygonCollider2D tempFoot = footCollider.GetChild(dirIndex).GetComponent<PolygonCollider2D>();

        bodyCol.points = tempBody.GetComponent<PolygonCollider2D>().points;
        footCol.points = tempFoot.GetComponent<PolygonCollider2D>().points;
    }

    void ActivateNavMeshAgent()
    {
        if(navAgent.enabled == false)
        {
            navAgent.enabled = true;
            navAgent.isStopped = false;
            navAgent.SetDestination(gatePos);
        }
    }

    void DeactivateNavMeshAgent()
    {
        if(navAgent.enabled == true)
        {
            navAgent.isStopped = true;
            navAgent.velocity = navAgent.velocity = new Vector3(0, 0, navAgent.velocity.z);
            navAgent.enabled = false;
        }
    }

    void UpdateSpriteDirection(Vector2 mv)
    {
        float angle = Vector2.SignedAngle(Vector2.right, mv);
        float x = dir.x; 
        float y = dir.y;

        if (angle >= -22.5f && angle < 22.5f) //right
        {
            SetAnimFloatParam("moveX", 1.0f);
            SetAnimFloatParam("moveY", 0.0f);
            frontCollider.rotation = Quaternion.Euler(0, 0, 0);
            footCollider.rotation = Quaternion.Euler(0, 0, 0);
            shadow.rotation = Quaternion.Euler(0, 0, 0);
            x = 1;
            y = 0;
            dirIndex = 0;
        }
        else if (angle >= 22.5f && angle < 67.5f) //right-up
        {
            SetAnimFloatParam("moveX", 1.0f);
            SetAnimFloatParam("moveY", 1.0f);
            frontCollider.rotation = Quaternion.Euler(0, 0, 45.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 45.0f);
            shadow.rotation = Quaternion.Euler(0, 0, 45.0f);
            x = 1;
            y = 1;
            dirIndex = 1;
        }
        else if (angle >= 67.5f && angle < 112.5f) //up
        {
            SetAnimFloatParam("moveX", 0.0f);
            SetAnimFloatParam("moveY", 1.0f);
            frontCollider.rotation = Quaternion.Euler(0, 0, 90.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 90.0f);
            shadow.rotation = Quaternion.Euler(0, 0, 90.0f);
            x = 0;
            y = 1;
            dirIndex = 2;
        }
        else if (angle >= 112.5f && angle < 157.5f) //up-left
        {
            SetAnimFloatParam("moveX", -1.0f);
            SetAnimFloatParam("moveY", 1.0f);
            frontCollider.rotation = Quaternion.Euler(0, 0, 135.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 135.0f);
            shadow.rotation = Quaternion.Euler(0, 0, 135.0f);
            x = -1;
            y = 1;
            dirIndex = 3;
        }
        else if (angle >= 157.5f || angle < -157.5f) //left
        {
            SetAnimFloatParam("moveX", -1.0f);
            SetAnimFloatParam("moveY", 0.0f);
            frontCollider.rotation = Quaternion.Euler(0, 0, 180.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 180.0f);
            shadow.rotation = Quaternion.Euler(0, 0, 180.0f);
            x = -1;
            y = 0;
            dirIndex = 4;
        }
        else if (angle >= -157.5f && angle < -112.5f) //left-down
        {
            SetAnimFloatParam("moveX", -1.0f);
            SetAnimFloatParam("moveY", -1.0f);
            frontCollider.rotation = Quaternion.Euler(0, 0, 225.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 225.0f);
            shadow.rotation = Quaternion.Euler(0, 0, 225.0f);
            x = -1;
            y = -1;
            dirIndex = 5;
        }
        else if (angle >= -112.5f && angle < -67.5f) //down
        {
            SetAnimFloatParam("moveX", 0.0f);
            SetAnimFloatParam("moveY", -1.0f);
            frontCollider.rotation = Quaternion.Euler(0, 0, 270.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 270.0f);
            shadow.rotation = Quaternion.Euler(0, 0, 270.0f);
            x = 0;
            y = -1;
            dirIndex = 6;
        }
        else if (angle >= -67.5f && angle < -22.5f) //down-right
        {
            SetAnimFloatParam("moveX", 1.0f);
            SetAnimFloatParam("moveY", -1.0f);
            frontCollider.rotation = Quaternion.Euler(0, 0, 315.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 315.0f);
            shadow.rotation = Quaternion.Euler(0, 0, 315.0f);
            x = 1;
            y = -1;
            dirIndex = 7;
        }

        dir = new Vector2(x, y);
    }

    public void ReserveNewSortingOrder(int sortingOrder)
    {
        //사실상 상태가 fall일 때만 발동하므로, 넘어질 때의 그림자 연출도 여기서.
        StartCoroutine(ShadowEffectForFall(0.4f));
        fixedSortingOrder = sortingOrder;
    }

    IEnumerator ShadowEffectForFall(float duration)
    {
        Vector3 startPosition = shadow.localPosition;
        Vector3 startScale = shadow.localScale;
        Vector3 targetPosition;
        Vector3 targetScale;
        targetPosition = shadow.localPosition + shadow.right * 0.8f;
        targetScale = new Vector3(1.5f, 1, 1);
        
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            shadow.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            shadow.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            yield return null;
        }

        shadow.localPosition = targetPosition;
        shadow.localScale = targetScale;
    }

    public void FixSortingOrder(int tempSortingOrder = -1)
    {
        if(fixedSortingOrder != 0)
        {
            spRender_normal.sortingOrder = fixedSortingOrder;
            spRender_heat0.sortingOrder = fixedSortingOrder;
            spRender_heat1.sortingOrder = fixedSortingOrder;
            spRender_heat2.sortingOrder = fixedSortingOrder;
            spRender_heat3.sortingOrder = fixedSortingOrder;
            spRender_heat4.sortingOrder = fixedSortingOrder;
        }
        else
        {
            spRender_normal.sortingOrder = tempSortingOrder;
            spRender_heat0.sortingOrder = tempSortingOrder;
            spRender_heat1.sortingOrder = tempSortingOrder;
            spRender_heat2.sortingOrder = tempSortingOrder;
            spRender_heat3.sortingOrder = tempSortingOrder;
            spRender_heat4.sortingOrder = tempSortingOrder;
        }
    }

    public Vector2 GetDir()
    {
        return dir;
    }

    public bool GetisWaitingToGate()
    {
        return isWaitingToGate;
    }

    private void Anger(object sender, EventArgs eventArgs)
    {
        if(passState == PassengerState.idle)
        {
            StartCoroutine(AngerForDuration(angerMark, 2.0f));
        }
    }

    private IEnumerator AngerForDuration(GameObject effect, float duration)
    {
        if(effect != null)
        {
            isAnger = true;
            effect.SetActive(true);
            heatOffset = heatOffset * 4;
            yield return new WaitForSeconds(duration);
            isAnger = false;
            if (effect != null)
                effect.SetActive(false);
            heatOffset = passManager.heatOffset_init;
        }
    }

    void SetSpriteRenderer(PlayerState state, float heat)
    {
        Color visible = new Color(255, 255, 255, 255);
        Color invisible = new Color(255, 255, 255, 0);

        if(state == PlayerState.OutSide)
        {
            spRender_normal.color = visible;
            spRender_heat0.color = invisible;
            spRender_heat1.color = invisible;
            spRender_heat2.color = invisible;
            spRender_heat3.color = invisible;
            spRender_heat4.color = invisible;
        }
        else if(state == PlayerState.inSide)
        {
            spRender_normal.color = invisible;
            if (heat <= 30)
            {
                spRender_heat0.color = visible;
                spRender_heat1.color = invisible;
                spRender_heat2.color = invisible;
                spRender_heat3.color = invisible;
                spRender_heat4.color = invisible;
            }
            else if (heat <= 55)
            {
                spRender_heat0.color = invisible;
                spRender_heat1.color = visible;
                spRender_heat2.color = invisible;
                spRender_heat3.color = invisible;
                spRender_heat4.color = invisible;
            }
            else if (heat <= 75)
            {
                spRender_heat0.color = invisible;
                spRender_heat1.color = invisible;
                spRender_heat2.color = visible;
                spRender_heat3.color = invisible;
                spRender_heat4.color = invisible;
            }
            else if (heat <= 90)
            {
                spRender_heat0.color = invisible;
                spRender_heat1.color = invisible;
                spRender_heat2.color = invisible;
                spRender_heat3.color = visible;
                spRender_heat4.color = invisible;
            }
            else
            {
                spRender_heat0.color = invisible;
                spRender_heat1.color = invisible;
                spRender_heat2.color = invisible;
                spRender_heat3.color = invisible;
                spRender_heat4.color = visible;
            }
        }
    }


    void SetAnimBoolParam(string param, bool val)
    {
        anim_normal.SetBool(param, val);
        anim_heat0.SetBool(param, val);
        anim_heat1.SetBool(param, val);
        anim_heat2.SetBool(param, val);
        anim_heat3.SetBool(param, val);
        anim_heat4.SetBool(param, val);
    }

    void SetAnimFloatParam(string param, float val)
    {
        anim_normal.SetFloat(param, val);
        anim_heat0.SetFloat(param, val);
        anim_heat1.SetFloat(param, val);
        anim_heat2.SetFloat(param, val);
        anim_heat3.SetFloat(param, val);
        anim_heat4.SetFloat(param, val);
    }
}

