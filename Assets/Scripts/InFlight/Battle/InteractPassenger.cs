using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class InteractPassenger : MonoBehaviour
{
    Animator animator;
    GameObject angerMark;
    Transform player;
    PlayerController playerController;
    Vector2 direction;
    NavMeshAgent agent;
    Vector3 destPosition;
    BoxCollider2D BoxCollider2D;
    int lastDirection;

    float[] XDirections = { 1, 1, 0, -1, -1, -1, 0, 1 };
    float[] YDirections = { 0, 1, 1, 1, 0, -1, -1, -1 };

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        angerMark = Util.FindChild(gameObject, "AngerMark");
        destPosition = Managers.PassPosition.GetPassPosition(transform.parent.name);
        BoxCollider2D = GetComponent<BoxCollider2D>();

        BoxCollider2D.offset = new Vector2(0.0f, -0.75f);
        BoxCollider2D.size = new Vector2(1.0f, 1.0f);

        agent = transform.parent.GetComponent<NavMeshAgent>();

        agent.baseOffset = 0;
        agent.speed = 2.4f;
        agent.angularSpeed = 50f;
        agent.acceleration = 100f;
        agent.stoppingDistance = 0;
        agent.autoBraking = false;
        agent.radius = 0.583f;
        agent.height = 1;
        agent.avoidancePriority = 50;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.AttackRange)
        {
            Vector3 centerPos = new Vector3(transform.position.x, transform.position.y, 0);
            playerController.PrintHitEffects(collision.transform.position, centerPos, 1);
            Managers.Sound.Play("hit1");

            direction = transform.position - player.position;
            lastDirection = DirectionToIndex(direction);

            StartCoroutine(KnockBackRoutine(direction.normalized, 4.0f));
        }
    }

    // 넉백
    IEnumerator KnockBackRoutine(Vector2 _knockBackDirection, float _knockBackPower)
    {
        animator.SetBool("isWithinPosition", false);
        animator.SetFloat("DirX", XDirections[InvertDirection(lastDirection)]);
        animator.SetFloat("DirY", YDirections[InvertDirection(lastDirection)]);
        angerMark.SetActive(true);

        float timer = 0f;
        float duration = 0.6f; // 넉백 지속 시간

        Vector2 knockBackDirection = _knockBackDirection;
        while (timer < duration)
        {
            float t = timer / duration;
            agent.velocity = knockBackDirection * Mathf.Lerp(_knockBackPower, 0, t);// 서서히 감속
            timer += Time.deltaTime;
            yield return null;
        }

        agent.velocity = Vector3.zero; // 최종적으로 속도 0
        agent.SetDestination(destPosition);
        StartCoroutine(WaitForArrival());
    }

    // 돌아가기
    IEnumerator WaitForArrival()
    {
        animator.SetBool("isMoving", true);

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.sqrMagnitude > 0f)
        {
            lastDirection = DirectionToIndex(agent.desiredVelocity);
            animator.SetFloat("DirX", XDirections[lastDirection]);
            animator.SetFloat("DirY", YDirections[lastDirection]);
            yield return null;
        }

        animator.SetBool("isMoving", false);
        animator.SetBool("isWithinPosition", true);
        angerMark.SetActive(false);
        agent.ResetPath();
    }

    private int DirectionToIndex(Vector2 _direction)
    {
        if (_direction.sqrMagnitude < 0.1f)
        {
            return lastDirection;
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
}
