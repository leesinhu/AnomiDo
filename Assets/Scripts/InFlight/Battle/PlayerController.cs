using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static Define;

public class PlayerController : MonoBehaviour
{
    //241226 추가
    ShipManager shipManager;
    //250130
    public String npcName { get; set; } = null;
    public String objName { get; set; } = null;

    [SerializeField] GameObject effect_hit;
    [SerializeField] GameObject effect_parry;
    [SerializeField] Vector2 firstDir;

    //UI
    GameObject navPoint;
    bool navigationActive = false;
    Vector2 navDest;

    //기존
    PlayerStat _stat;
    public Define.MouseEvent _mouse = Define.MouseEvent.None;
    public Define.State _state = Define.State.Chase;

    // 스프라이트렌더러
    SpriteRenderer spriteRenderer;

    // Move
    private Rigidbody2D rb;
    private Vector2 movement = Vector2.zero;
    private float moveSpeed = 0;
    Vector2 _direction;
    [SerializeField]
    float knockBackPower = 1.0f;
    float releaseTimer = 0.0f;
    float releaseTime = 0.025f;

    bool attacked = false;
    bool hasHit = false;
    public bool isNormalHit { get; set; } = false; //일반 타격인지,팔 뻗고 있기인지 구분 

    public bool HasHit
    {
        get { return hasHit; }
        set
        {
            hasHit = value;
            _animator.SetBool("hasHit", hasHit);
        }
    }
    bool parrySucceeded;
    public bool ParrySucceeded
    {
        get { return parrySucceeded; }
        set
        {
            parrySucceeded = value;
            _animator.SetBool("parrySucceeded", parrySucceeded);
        }
    }
    bool isRecoverStamina = true;

    // MouseEvent
    bool _pressed = false;
    [SerializeField]
    float _activeAttackTime = 0.1f;
    bool isAttacking = false;
    public bool IsAttacking
    {
        get { return isAttacking; }
        set
        {
            isAttacking = value;
            _animator.SetBool("isAttacking", isAttacking);
        }
    }
    public int _enemyDirection;

    // Animation
    Animator _animator;
    public int lastDirection;
    float[] XDirections = { 1, 1, 0, -1, -1, -1, 0, 1 };
    float[] YDirections = { 0, 1, 1, 1, 0, -1, -1, -1 };

    Transform AttackRangeCollider;
    Transform hitBoxCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        _animator = GetComponent<Animator>();

        navPoint = transform.GetChild(3).gameObject;
    }

    private void Start()
    {
        lastDirection = DirectionToIndex(firstDir);

        //241226 추가
        shipManager = GameObject.Find("ShipManager").GetComponent<ShipManager>();

        //기존
        _stat = gameObject.GetComponent<PlayerStat>();
        AttackRangeCollider = Util.FindChild(gameObject, "AttackRangeCollider").transform;
        hitBoxCollider = Util.FindChild(gameObject, "HitBoxCollider").transform;
        knockBackPower = 4.0f;

        // 플레이어 상태 UI
        Managers.UI.ShowSceneUI<UI_PlayerBar>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Organism";
        spriteRenderer.sortingOrder = 10;
    }

    void Update()
    {
        MouseEvent();
        switch (_state)
        {
            case Define.State.Attacked:
                Attacked();
                break;
            case Define.State.Chase:
                if (shipManager.movePermit)
                {
                    Chase();
                }
                else
                {
                    _animator.SetBool("isMoving", false);
                    SetDirection(_direction);
                    moveSpeed = 0;
                }
                break;
            case Define.State.Die:
                Die();
                break;
            case Define.State.KnockBack:
                KnockBack();
                break;
            case Define.State.Parry:
                Parry();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space) && shipManager.interactPermit)
        {
            if (npcName != null)
            {
                shipManager.OpenScript(npcName);
            }
            else if (objName != null)
            {
                shipManager.OpenObjMenu(objName);
            }
        }

        // 스테미너 회복
        if (isRecoverStamina && _stat.Stamina < _stat.MaxStamina)
        {
            _stat.Stamina += 5.0f * Time.deltaTime;

            if (!IsAttacking)
            {
                _stat.Stamina += 5.0f * Time.deltaTime;
            }
        }

        if (navigationActive && navDest != null)
        {
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 dir = navDest - playerPos;
            Vector2 norDir = dir.normalized;
            Vector2 navPointPos = playerPos + (norDir * 3f);
            float navPointRot = Mathf.Atan2(norDir.y, norDir.x) * Mathf.Rad2Deg;

            navPoint.transform.position = navPointPos;
            navPoint.transform.rotation = Quaternion.Euler(0, 0, navPointRot);

            if (dir.sqrMagnitude < 25)
            {
                navPoint.SetActive(false);
            }
            else
            {
                navPoint.SetActive(true);
            }
        }
    }

    void FixedUpdate()
    {
        if (!attacked)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // 조작
    void Chase()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
        releaseTimer -= Time.deltaTime;

        _animator.SetFloat("DirX", XDirections[lastDirection]);
        _animator.SetFloat("DirY", YDirections[lastDirection]);

        if (IsAttacking || movement == Vector2.zero)
        {
            moveSpeed = 0;
            _animator.SetBool("isMoving", false);
        }
        else
        {
            UpdateSpriteDirection();
            moveSpeed = _stat.PlayerSpeed;
            _animator.SetBool("isMoving", true);
        }

        SetDirection(_direction);
    }

    void UpdateSpriteDirection()
    {
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        float x = _direction.x;
        float y = _direction.y;

        if (angle >= -22.5f && angle < 22.5f) //right
        {
            if (releaseTimer <= 0.0f)
            {
                x = 1;
                y = 0;
            }
        }
        else if (angle >= 22.5f && angle < 67.5f) //right-up
        {
            releaseTimer = releaseTime;
            x = 1;
            y = 1;
        }
        else if (angle >= 67.5f && angle < 112.5f) //up
        {
            if (releaseTimer <= 0.0f)
            {
                x = 0;
                y = 1;
            }
        }
        else if (angle >= 112.5f && angle < 157.5f) //up-left
        {
            releaseTimer = releaseTime;
            x = -1;
            y = 1;
        }
        else if (angle >= 157.5f || angle < -157.5f) //left
        {
            if (releaseTimer <= 0.0f)
            {
                x = -1;
                y = 0;
            }
        }
        else if (angle >= -157.5f && angle < -112.5f) //left-down
        {
            releaseTimer = releaseTime;
            x = -1;
            y = -1;
        }
        else if (angle >= -112.5f && angle < -67.5f) //down
        {
            if (releaseTimer <= 0.0f)
            {
                x = 0;
                y = -1;
            }
        }
        else if (angle >= -67.5f && angle < -22.5f) //down-right
        {
            releaseTimer = releaseTime;
            x = 1;
            y = -1;
        }
        _direction = new Vector2(x, y);
    }

    // 죽음
    void Die()
    {
        _animator.SetTrigger("Die");
        hitBoxCollider.gameObject.SetActive(false);
        moveSpeed = 0;
    }

    // 공격당했을 때 밀려남
    void Attacked()
    {
        AttackRangeCollider.gameObject.SetActive(false);
        ParrySucceeded = false;
        IsAttacking = false;
        attacked = true;
        Vector3 knockBackDirection = new Vector2(XDirections[InvertDirection(_enemyDirection)], YDirections[InvertDirection(_enemyDirection)]).normalized;
        StartCoroutine(KnockBackRoutine(knockBackDirection, knockBackPower));

        lastDirection = _enemyDirection;

        _state = Define.State.KnockBack;
    }

    // 뒤로 밀려난 velocity를 회복
    void KnockBack()
    {
        if (rb.velocity.sqrMagnitude < 0.01f)
        {
            _state = Define.State.Chase;
            attacked = false;
        }
    }

    IEnumerator KnockBackRoutine(Vector3 _knockBackDirection, float _knockBackPower)
    {
        float timer = 0f;
        float duration = 0.6f; // 넉백 지속 시간
        if (_knockBackDirection.x == 0)
            _knockBackDirection.x = 0.01f;
        Vector2 knockBackDirection = _knockBackDirection;

        while (timer < duration)
        {
            float t = timer / duration;
            rb.velocity = knockBackDirection * Mathf.Lerp(_knockBackPower, 0, t); // 서서히 감속
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero; // 최종적으로 속도 0
    }

    // 패링에 성공했을 때
    void Parry()
    {
        shipManager.ShakeCamera();
        ParrySucceeded = true;
        IsAttacking = false;
        attacked = true;
        Vector3 knockBackDirection = new Vector2(XDirections[InvertDirection(_enemyDirection)], YDirections[InvertDirection(_enemyDirection)]).normalized;
        StartCoroutine(KnockBackRoutine(knockBackDirection, 2.0f));

        lastDirection = _enemyDirection;

        _state = Define.State.KnockBack;
    }

    public void PrintHitEffects(Vector3 hitPos, Vector3 targetPos, int i)
    {
        Vector3 dirOffset = (hitPos - targetPos).normalized;
        Vector3 effectPos = targetPos + dirOffset * 0.25f;
        if (i == 1)
        {
            Instantiate(effect_hit, effectPos, Quaternion.identity);
        }
        else if (i == 2)
        {
            Instantiate(effect_parry, effectPos, Quaternion.identity);
        }
    }

    public void SetDirection(Vector2 _direction)
    {
        if (moveSpeed != 0)
        {
            lastDirection = DirectionToIndex(_direction);
        }

        AttackRangeCollider.rotation = Quaternion.Euler(0, 0, lastDirection * 45);
        hitBoxCollider.rotation = Quaternion.Euler(0, 0, lastDirection * 45);
    }

    private int DirectionToIndex(Vector2 _direction)
    {
        Vector2 norDir = _direction;
        float step = 360 / 8;

        float angle = Vector2.SignedAngle(Vector2.right, norDir);
        if (angle < 0)
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.RoundToInt(stepCount) % 8;
    }

    void MouseEvent()
    {
        if (shipManager.movePermit)
        {
            // UI 누르면 바로 return
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // 마우스 누르면 실행
            if (Input.GetMouseButtonDown(0))
            {
                // 공격이 끝나야 다시 공격 가능
                if (!IsAttacking && _stat.Stamina >= 20)
                {
                    _mouse = Define.MouseEvent.PointerDown;
                    HasHit = false;
                    IsAttacking = true;
                    isRecoverStamina = false;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                // 누른 상태로 _activeAttackTime초 지나면 Press
                if (_stat.Stamina >= 2.5f && _pressed && !isNormalHit)
                {
                    _mouse = Define.MouseEvent.Press;
                }
            }
        }
    }

    int InvertDirection(int Direction)
    {
        return (Direction + 4) % 8;
    }

    public void OnFallBackEvent()
    {
    }

    public void OnDeadEvent()
    {
        shipManager.GameOver();
    }

    public void OnStartHitEvent()
    {
        _stat.Stamina -= 20;
        Managers.Sound.Play("hit3(wind)_alter");
    }

    public void OnHitEvent()
    {
        AttackRangeCollider.gameObject.SetActive(true);
        isNormalHit = true;
        _pressed = true;
    }

    public void OnHitEvent2()
    {
        isNormalHit = false;
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitUntil(() => Input.GetMouseButton(0) == false);
        if(!ParrySucceeded)
        {
            _mouse = Define.MouseEvent.PointerUp;
            HasHit = true;
            _pressed = false;
        }
    }

    public void OnEndHitEvent()
    {
        AttackRangeCollider.gameObject.SetActive(false);
        ParrySucceeded = false;
    }

    public void OnAttackEndEvent()
    {
        HasHit = false;
        IsAttacking = false;
        isRecoverStamina = true;
    }

    public void OnParryEnd()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        isRecoverStamina = true;
    }

    public void SetDirectionByOther(Vector2 dir)
    {
        lastDirection = DirectionToIndex(dir);
        _direction = dir;

        _animator.SetFloat("DirX", XDirections[lastDirection]);
        _animator.SetFloat("DirY", YDirections[lastDirection]);
    }

    public void ActivateNav(Vector2 dest)
    {
        navDest = dest;
        navPoint.SetActive(true);
        navigationActive = true;
    }

    public void DeactiveNav()
    {
        navPoint.SetActive(false);
        navigationActive = false;
    }
}
