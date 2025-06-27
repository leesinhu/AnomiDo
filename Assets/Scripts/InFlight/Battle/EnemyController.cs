using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    //241226 추가
    ShipManager shipManager;

    //기존
    private Vector3 target;
    NavMeshAgent agent;
    int initPriority;

    // Patrol 관련 변수
    List<Vector3> points;
    bool isReturning = false;
    int destPointIndex;

    Stat _stat;
    float moveSpeed;
    float previousY;

    public Define.State _state;
    public Define.State prevState;
    bool chaseSign = false; //원거리에서 플레이어 추적
    public int enemySortNum; //적이 한번에 몰려들기 위해서

    Transform player;
    Vector3 playerPos;
    PlayerController _playerController;
    int playerDirection;
    public Vector2 direction;
    [SerializeField]
    float knockBackPower;

    // 시야 관련 변수
    [SerializeField]
    float frontDetectionRange;
    [SerializeField]
    float aroundDetectionRange;
    [SerializeField]
    float detectAngle;
    [SerializeField]
    int rayCount;
    [SerializeField]
    float missedDetectionRange;


    // 공격 관련 변수
    [SerializeField]
    float _activeAttackTime = 1.0f;
    float _pressedTime = 0;
    bool _atkend = false;
    bool _pressed = false;

    [SerializeField]
    float attackDistance = 1.3f;

    public bool domino = false;
    public bool deadCounted = false; //shipmanager에서 처치 카운트 중복 안 되게

    // UI
    UI_HPBar HPBar;
    AudioSource[] audio_attack;
    AudioSource audio_dead;

    // 스프라이트렌더러
    SpriteRenderer spriteRenderer;

    Animator _animator;
    float directionUnlockCoolTime;
    float lastChangeTime;
    private int lastDirection;
    public int LastDirection
    {
        get { return lastDirection; }
        set
        {
            lastDirection = value;

            Quaternion rot = Quaternion.Euler(0, 0, lastDirection * 45);
            hitBox.rotation = rot;
            AttackRangeCollider.rotation = rot;
            FallBackCollider.rotation = rot;
            ParriedCollider.rotation = rot;
            PushWallCollider.rotation = rot;
        }
    }

    string[] SearchAroundDirections = { "SEARCHAROUND_E", "SEARCHAROUND_NE", "SEARCHAROUND_N", "SEARCHAROUND_NW", "SEARCHAROUND_W", "SEARCHAROUND_SW", "SEARCHAROUND_S", "SEARCHAROUND_SE" };
    float[] XDirections = { 1, 1, 0, -1, -1, -1, 0, 1 };
    float[] YDirections = { 0, 1, 1, 1, 0, -1, -1, -1 };

    bool isMoving;
    public bool IsMoving
    {
        get { return isMoving; }
        set
        {
            isMoving = value;
            agent.isStopped = !isMoving;
            _animator.SetBool("isMoving", isMoving);
        }
    }
    bool isAttacking;
    public bool IsAttacking
    {
        get { return isAttacking; }
        set
        {
            isAttacking = value;
            _animator.SetBool("isAttacking", isAttacking);
        }
    }
    bool hasHit;
    public bool HasHit
    {
        get { return hasHit; }
        set
        {
            hasHit = value;
            _animator.SetBool("hasHit", hasHit);
        }
    }

    Transform hitBox;
    Transform ParriedCollider;
    Transform FallBackCollider;
    Transform AttackRangeCollider;
    Transform PushWallCollider;
    Transform shadow;

    public bool hasFallByDomino { get; set; } = false;

    private void Awake()
    {
        HPBar = Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        audio_attack = transform.GetChild(5).GetComponents<AudioSource>();
        audio_dead = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _state = Define.State.Patrol;
        shipManager = GameObject.Find("ShipManager").GetComponent<ShipManager>();
        _animator = GetComponent<Animator>();

        agent = transform.parent.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.acceleration = 100f;
        agent.angularSpeed = 50f;
        agent.autoBraking = false;
        initPriority = agent.avoidancePriority;

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent가 NavMesh 위에 없음.");
        }

        knockBackPower = 4.0f;

        previousY = transform.position.y;
        player = GameObject.Find("Player").transform;
        _stat = transform.GetComponent<Stat>();
        moveSpeed = _stat.MoveSpeed;

        // 시야 관련 변수
        frontDetectionRange = 10.0f;
        aroundDetectionRange = 5.0f;
        detectAngle = 90.0f;
        rayCount = 30;
        missedDetectionRange = 50.0f; //추적 포기 거의 안하도록..

        directionUnlockCoolTime = 0.2f;
        lastChangeTime = -Mathf.Infinity;

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Organism";
        hitBox = Util.FindChild(gameObject, "HitBoxCollider").transform;
        AttackRangeCollider = Util.FindChild(gameObject, "AttackRangeCollider").transform;
        FallBackCollider = Util.FindChild(gameObject, "FallBackCollider").transform;
        ParriedCollider = Util.FindChild(gameObject, "ParriedCollider").transform;
        PushWallCollider = Util.FindChild(gameObject, "PushWallCollider").transform;

        _playerController = player.GetComponent<PlayerController>();

        // Patrol
        points = Managers.Patrol.GetPatrolsForScene(SceneManager.GetActiveScene().name, transform.parent.name);
        destPointIndex = 0;
        transform.parent.position = ParsePatrolPosition(points[destPointIndex]);

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Organism";
        spriteRenderer.sortingOrder = 10;

        shadow = transform.Find("Shadow");
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = _stat.MoveSpeed;
        playerPos = player.position;
        direction = playerPos - transform.position;

        if (_playerController._state == Define.State.Die)
        {
            _state = Define.State.Idle;
        }

        switch (_state)
        {
            case Define.State.Attack:
                Attack();
                break;
            case Define.State.Attacked:
                Attacked();
                break;
            case Define.State.Chase:
                Chase();
                break;
            case Define.State.Die:
                Die();
                break;
            case Define.State.Fall:
                Fall();
                break;
            case Define.State.Idle:
                Idle();
                break;
            case Define.State.KnockBack:
                KnockBack();
                break;
            case Define.State.ParriedFall:
                ParriedFall();
                break;
            case Define.State.Patrol:
                Patrol();
                break;
            case Define.State.SearchAround:
                SearchAround(prevState);
                break;
        }
    }

    #region State
    void Attack()
    {
        agent.avoidancePriority = 25;
        IsMoving = false;
        IsAttacking = true;

        // 누른 상태로 _activeAttackTime초 지나면 Press
        if (_pressed && Time.time >= _pressedTime + _activeAttackTime)
        {
            _pressed = false;
            _atkend = true;
            _pressedTime = 0;
        }
    }

    void Attacked()
    {
        IsMoving = false;
        IsAttacking = false;
        lastDirection = _playerController.lastDirection;
        Vector2 knockBackDirection = new Vector3(XDirections[lastDirection], YDirections[lastDirection]).normalized;
        StartCoroutine(KnockBackRoutine(knockBackDirection, knockBackPower));

        _state = Define.State.KnockBack;
    }

    void Idle()
    {
        //_animator.Play(IdleDirections[lastDirection]);
        IsMoving = false;
        IsAttacking = false;
    }

    void Chase()
    {
        HPBar.ShowBar();
        Vector3 destination = new Vector3(playerPos.x, playerPos.y - 0.75f, 0);

        IsMoving = true;
        IsAttacking = false;

        agent.SetDestination(destination);
        SetDirection();

        // 거리가 attackDistance보다 작아지면 공격 상태로 전환
        if (direction.sqrMagnitude < attackDistance * attackDistance)
        {
            _state = Define.State.Attack;
        }

        if (direction.sqrMagnitude > missedDetectionRange * missedDetectionRange)
        {
            prevState = Define.State.Chase;
            _state = Define.State.SearchAround;
        }
    }

    public void ConvertToChase()
    {
        chaseSign = true;
    }

    void Die()
    {
        agent.enabled = false;
        hitBox.gameObject.SetActive(false);
        ParriedCollider.gameObject.SetActive(false);
        FallBackCollider.gameObject.SetActive(false);
        AttackRangeCollider.gameObject.SetActive(false);
        PushWallCollider.gameObject.SetActive(false);

        spriteRenderer.sortingLayerName = "Structure";
        spriteRenderer.sortingOrder = 1;
        this.enabled = false;
    }

    void Fall()
    {
        //_animator.Play(FallBackDirections[lastDirection]);

        _animator.SetBool("die", true);
        _animator.SetFloat("DirX", XDirections[LastDirection]);
        _animator.SetFloat("DirY", YDirections[LastDirection]);
        IsMoving = false;
        IsAttacking = false;
        StartCoroutine(ShadowEffectForFall(0.5f));
    }

    void ParriedFall()
    {
        //_animator.Play(ParriedFallBackDirections[lastDirection]);
        _animator.SetBool("parryDie", true);
        IsMoving = false;
        IsAttacking = true;
        HasHit = false;
        StartCoroutine(ShadowEffectForFall(0.6f));
    }

    void KnockBack()
    {
        agent.avoidancePriority = 20;

        if (agent.velocity == Vector3.zero) // 거의 멈췄을 때
        {
            _state = Define.State.Chase;
            agent.avoidancePriority = 50;
        }
    }

    void Patrol()
    {
        IsMoving = true;
        HPBar.HideBar();

        if (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.1f)
        {
            prevState = Define.State.Patrol;
            _state = Define.State.SearchAround;
        }

        SetDirection();
        //_animator.Play(WalkDirections[lastDirection]);

        if (DetectInView()) //적 발견
        {
            prevState = Define.State.Patrol;
            _state = Define.State.Chase;
            shipManager.ActivateChasingGroup(enemySortNum);
        }
    }

    void SearchAround(Define.State prevState)
    {
        int _direction = lastDirection;

        if (prevState == Define.State.Chase)
        {
            _direction = lastDirection;
        }
        else if (prevState == Define.State.Patrol)
        {
            _direction = ParsePatrolAnimation(points[destPointIndex]);
        }

        if (_direction == -1)
        {
            _state = Define.State.Patrol;
            GotoNextPoint();
        }
        else
        {
            IsMoving = false;
            lastDirection = _direction;

            if (DetectInView())
            {
                _state = Define.State.Patrol;
            }
            else
            {
                if (points.Count == 1)
                {
                    //_animator.Play(IdleDirections[_direction]);
                }
                else
                {
                    _animator.Play(SearchAroundDirections[_direction]);
                }
            }
        }
    }
    #endregion

    void GotoNextPoint()
    {
        if (points.Count == 1)
            return;

        if (isReturning)
        {
            destPointIndex = destPointIndex - 1;
        }
        else
        {
            destPointIndex = destPointIndex + 1;
        }

        if (destPointIndex == 0)
        {
            isReturning = false;
        }
        if (destPointIndex == points.Count - 1)
        {
            isReturning = true;
        }

        agent.SetDestination(ParsePatrolPosition(points[destPointIndex]));
    }

    bool DetectInView()
    {
        Vector3 origin = transform.position - new Vector3(0.0f, 0.75f, 0.0f);
        Vector3 forward = new Vector3(XDirections[lastDirection], YDirections[lastDirection], 0);
        float startAngle = -detectAngle / 2;

        for (int i = 0; i < rayCount + 1; i++)
        {
            float currentAngle1 = startAngle + (detectAngle / rayCount) * i;
            Quaternion rotation1 = Quaternion.Euler(0, 0, currentAngle1);
            Vector3 direction1 = rotation1 * forward;

            float currentAngle2 = startAngle - ((360f - detectAngle) / rayCount) * i;
            Quaternion rotation2 = Quaternion.Euler(0, 0, currentAngle2);
            Vector3 direction2 = rotation2 * forward;

            RaycastHit2D hit1 = Physics2D.Raycast(origin, direction1, frontDetectionRange, ~(1 << (int)Define.Layer.HitBox));
            RaycastHit2D hit2 = Physics2D.Raycast(origin, direction2, aroundDetectionRange, ~(1 << (int)Define.Layer.HitBox));

            if (hit1.collider != null)
            {
                //Debug.DrawRay(origin, direction1 * hit1.distance, Color.red); // 감지된 Ray 표시

                if (hit1.transform.tag == "Player")
                {
                    return true;
                }
            }
            else
            {
                //Debug.DrawRay(origin, direction1 * frontDetectionRange, Color.green); // 감지되지 않은 Ray 표시
            }

            if (hit2.collider != null)
            {
                //Debug.DrawRay(origin, direction2 * hit2.distance, Color.red); // 감지된 Ray 표시

                if (hit2.transform.tag == "Player")
                {
                    return true;
                }
            }
            else
            {
                // Debug.DrawRay(origin, direction2 * aroundDetectionRange, Color.green); // 감지되지 않은 Ray 표시
            }
        }

        if (chaseSign)
            return true;

        return false;
    }

    public void DeActiveAgent()
    {
        //agent.velocity = Vector3.zero;
    }

    public void ActiveAgent()
    {
        //agent.isStopped = false;
    }

    public void SetDirection()
    {
        if (CanChangeDirection())
        {
            LastDirection = DirectionToIndex(agent.desiredVelocity);

            _animator.SetFloat("DirX", XDirections[LastDirection]);
            _animator.SetFloat("DirY", YDirections[LastDirection]);

            lastChangeTime = Time.time;
        }
    }

    bool CanChangeDirection()
    {
        return Time.time >= lastChangeTime + directionUnlockCoolTime;
    }

    IEnumerator KnockBackRoutine(Vector2 _knockBackDirection, float _knockBackPower)
    {

        float timer = 0f;
        float duration = 0.6f; // 넉백 지속 시간

        if (_knockBackDirection.x == 0)
            _knockBackDirection.x = 0.01f;
        Vector2 knockBackDirection = _knockBackDirection;
        while (timer < duration)
        {
            float t = timer / duration;
            agent.velocity = knockBackDirection * Mathf.Lerp(_knockBackPower, 0, t);// 서서히 감속
            timer += Time.deltaTime;
            yield return null;
        }

        agent.velocity = Vector3.zero; // 최종적으로 속도 0
    }

    Vector3 ParsePatrolPosition(Vector3 _points)
    {
        Vector3 position = new Vector3(_points.x, _points.y, 0.0f);

        return position;
    }

    int ParsePatrolAnimation(Vector3 _points)
    {
        int direction = (int)_points.z;

        return direction;
    }

    private int DirectionToIndex(Vector2 _direction)
    {
        if (_direction.sqrMagnitude < 0.1)
        {
            _direction = direction;
        }

        Vector2 norDir = _direction.normalized;

        float step = 360 / 8;

        float angle = Vector2.SignedAngle(Vector2.right, norDir);
        if (angle < 0)
        {
            angle += 360;
        }

        float stepCount = angle / step;

        return Mathf.RoundToInt(stepCount) % 8;
    }

    int InvertDirection(int Direction)
    {
        return (Direction + 4) % 8;
    }

    public void OnFallBackEvent()
    {
        hitBox.GetComponent<BoxCollider2D>().enabled = false;
        PushWallCollider.gameObject.SetActive(true);

        if (domino)
        {
            FallBackCollider.gameObject.SetActive(true);
        }
    }

    public void OnDeadEvent()
    {
        FallBackCollider.gameObject.SetActive(false);
        audio_dead.PlayOneShot(audio_dead.clip);
        //PushWallCollider.gameObject.SetActive(true);
        _state = Define.State.Die;
    }

    public void OnParriedEvent()
    {
        hitBox.gameObject.SetActive(false);
        ParriedCollider.gameObject.SetActive(true);
        audio_attack[1].Play();
    }

    public void OnHitEvent()
    {
        hitBox.gameObject.SetActive(true);
        ParriedCollider.gameObject.SetActive(false);
        AttackRangeCollider.gameObject.SetActive(true);

        _pressedTime = Time.time;
        HasHit = true;
    }

    public void OnEndHitEvent()
    {
        AttackRangeCollider.gameObject.SetActive(false);
    }

    public void OnAttackEndEvent()
    {
        HasHit = false;
        _state = Define.State.Chase;
        agent.avoidancePriority = initPriority;

        StartCoroutine(FreeTurnInMoment());
    }

    public void OnEndSearchAround()
    {
        _state = Define.State.Patrol;
        GotoNextPoint();
    }

    IEnumerator FreeTurnInMoment()
    {
        float tempTime = directionUnlockCoolTime;
        directionUnlockCoolTime = 0;
        yield return new WaitForSeconds(0.1f);
        directionUnlockCoolTime = tempTime;
    }

    IEnumerator ShadowEffectForFall(float duration)
    {
        Vector3 startPosition = shadow.localPosition;
        Vector3 startScale = shadow.localScale;
        Vector3 targetPosition;
        Vector3 targetScale;
        targetPosition = shadow.localPosition - FallBackCollider.right * 0.8f;
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
        shadow.gameObject.SetActive(false);
    }

    public void AttackStart()
    {
        audio_attack[0].Play();
    }

    public void Empty()
    {
        //null
        AttackRangeCollider.gameObject.SetActive(false);
    }
}
