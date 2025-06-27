using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PassManager passManager;

    Rigidbody2D rb;
    Vector2 movement = Vector2.zero;
    public Vector2 dir { get; private set; } = Vector2.zero;
    Transform frontCollider;
    Transform footCollider;
    SpriteRenderer shadow;
    [SerializeField] Sprite shadow_normal, shadow_heat;

    public SpriteRenderer spRender_normal { get; private set; }
    public SpriteRenderer spRender_heat { get; private set; }
    Animator anim_normal, anim_heat;
    [SerializeField] GameObject effect_hit;

    [SerializeField] float moveSpeedInit = 5f;
    float moveSpeed = 0f;

    public float releaseTime = 0.025f;
    private float releaseTimer = 0.0f;

    private bool isAttack = false;
    AudioSource audio_hit_wind;

    public GameObject playerSpawnPoint;
    //플레이어 활동 시 초기 위치(비행선 게이트 앞)
    public void PlayerSpawn()
    {
        transform.position = new Vector3(playerSpawnPoint.transform.position.x, playerSpawnPoint.transform.position.y, 0);
    }

    private void Awake()
    {
        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
        playerSpawnPoint = GameObject.Find("WayPoint/PlayerSpawnPoint");

        rb = GetComponent<Rigidbody2D>();
        frontCollider = transform.Find("FrontCollider");
        footCollider = transform.Find("FootCollider");
        shadow = transform.Find("Shadow").GetComponent<SpriteRenderer>();

        spRender_normal = transform.Find("Sprite_Normal").GetComponent<SpriteRenderer>();
        spRender_heat = transform.Find("Sprite_Heat").GetComponent<SpriteRenderer>();
        anim_normal = transform.Find("Sprite_Normal").GetComponent<Animator>();
        anim_heat = transform.Find("Sprite_Heat").GetComponent<Animator>();
        audio_hit_wind = GetComponent<AudioSource>();
    }

    void Start()
    {
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        //gameObject.SetActive(false);
        SetAnimFloatParam("moveX", -1.0f);
        SetAnimFloatParam("moveY", -1.0f);

        moveSpeed = moveSpeedInit;
    }

    void Update()
    {
        if ((passManager.stageState == StationStageState.AfterOpen && passManager.playerState == PlayerState.OutSide) || passManager.tut_playerAct)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical") * 0.56f;
            movement = movement.normalized;

            if (movement != Vector2.zero && !isAttack) //입력을 받았을 때
            {
                UpdateSpriteDirection();
                SetAnimBoolParam("isWalk", true);
            }
            else
            {
                movement = Vector2.zero;
                SetAnimBoolParam("isWalk", false);
            }

            releaseTimer -= Time.deltaTime;

            if (Input.GetMouseButton(0))
            {
                if (!audio_hit_wind.isPlaying && !isAttack)
                    audio_hit_wind.Play();
                SetAnimBoolParam("isAttack", true);
                isAttack = true;
            }
        }
        else
        {
            movement = Vector2.zero;
            SetAnimBoolParam("isAttack", false);
            SetAnimBoolParam("isWalk", false);
        }

        //적외선 카메라 상에서 그림자 삭제
        if(passManager.playerState == PlayerState.inSide)
        {
            shadow.sprite = shadow_heat;
            spRender_normal.color = new Color(255, 255, 255, 0);
            spRender_heat.color = new Color(255, 255, 255, 255);
        }
        else
        {
            shadow.sprite = shadow_normal;
            spRender_normal.color = new Color(255, 255, 255, 255);
            spRender_heat.color = new Color(255, 255, 255, 0);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void UpdateSpriteDirection()
    {
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        float x = dir.x;
        float y = dir.y;

        if (angle >= -22.5f && angle < 22.5f) //right
        {
            if(releaseTimer <= 0.0f)
            {
                SetAnimFloatParam("moveX", 1.0f);
                SetAnimFloatParam("moveY", 0.0f);
                footCollider.rotation = Quaternion.Euler(0, 0, 0);
                frontCollider.rotation = Quaternion.Euler(0, 0, 0);
                x = 1;
                y = 0;
            }
        }
        else if (angle >= 22.5f && angle < 67.5f) //right-up
        {
            releaseTimer = releaseTime;
            SetAnimFloatParam("moveX", 1.0f);
            SetAnimFloatParam("moveY", 1.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 45);
            frontCollider.rotation = Quaternion.Euler(0, 0, 45);
            x = 1;
            y = 1;
        }
        else if (angle >= 67.5f && angle < 112.5f) //up
        {
            if(releaseTimer <= 0.0f)
            {
                SetAnimFloatParam("moveX", 0.0f);
                SetAnimFloatParam("moveY", 1.0f);
                footCollider.rotation = Quaternion.Euler(0, 0, 90);
                frontCollider.rotation = Quaternion.Euler(0, 0, 90);
                x = 0;
                y = 1;
            }  
        }
        else if (angle >= 112.5f && angle < 157.5f) //up-left
        {
            releaseTimer = releaseTime;
            SetAnimFloatParam("moveX", -1.0f);
            SetAnimFloatParam("moveY", 1.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 135);
            frontCollider.rotation = Quaternion.Euler(0, 0, 135);
            x = -1;
            y = 1;
        }
        else if (angle >= 157.5f || angle < -157.5f) //left
        {
            if(releaseTimer <= 0.0f)
            {
                SetAnimFloatParam("moveX", -1.0f);
                SetAnimFloatParam("moveY", 0.0f);
                footCollider.rotation = Quaternion.Euler(0, 0, 180);
                frontCollider.rotation = Quaternion.Euler(0, 0, 180);
                x = -1;
                y = 0;
            }
        }
        else if (angle >= -157.5f && angle < -112.5f) //left-down
        {
            releaseTimer = releaseTime;
            SetAnimFloatParam("moveX", -1.0f);
            SetAnimFloatParam("moveY", -1.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 225);
            frontCollider.rotation = Quaternion.Euler(0, 0, 225);
            x = -1;
            y = -1;
        }
        else if (angle >= -112.5f && angle < -67.5f) //down
        {
            if(releaseTimer <= 0.0f)
            {
                SetAnimFloatParam("moveX", 0.0f);
                SetAnimFloatParam("moveY", -1.0f);
                footCollider.rotation = Quaternion.Euler(0, 0, 270);
                frontCollider.rotation = Quaternion.Euler(0, 0, 270);
                x = 0;
                y = -1;
            }  
        }
        else if (angle >= -67.5f && angle < -22.5f) //down-right
        {
            releaseTimer = releaseTime;
            SetAnimFloatParam("moveX", 1.0f);
            SetAnimFloatParam("moveY", -1.0f);
            footCollider.rotation = Quaternion.Euler(0, 0, 315);
            frontCollider.rotation = Quaternion.Euler(0, 0, 315);
            x = 1;
            y = -1;
        }
        dir = new Vector2(x, y);
    }

    public void PrintHitEffects(Vector3 targetPos)
    {
        Vector3 dirOffset = (transform.position - targetPos).normalized;
        Vector3 effectPos = targetPos + dirOffset * 0.25f;
        Instantiate(effect_hit, effectPos, Quaternion.identity);
    }

    public void CancleDelay()
    {
        SetAnimBoolParam("isAttackEnd", false);
        isAttack = false;
    }

    public void AttackFront()
    {
        frontCollider.GetComponent<Player_Collider_Front>().isAttacking = true;
    }

    public void AttackFrontEnd()
    {
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitUntil(() => Input.GetMouseButton(0) == false);
        SetAnimBoolParam("isAttack", false);
        SetAnimBoolParam("isAttackEnd", true);
        frontCollider.GetComponent<Player_Collider_Front>().isAttacking = false;
    }

    void SetAnimBoolParam(string param, bool val)
    {
        anim_normal.SetBool(param, val);
        anim_heat.SetBool(param, val);
    }

    void SetAnimFloatParam(string param, float val)
    {
        anim_normal.SetFloat(param, val);
        anim_heat.SetFloat(param, val);
    }
}
