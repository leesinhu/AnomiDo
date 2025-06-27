using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : MonoBehaviour
{
    Vector2 dir = Vector2.zero;
    public Vector3 targetPos { get; set; } = Vector3.zero;
    [SerializeField] float bulletSpeed = 10;
    bool bulletInBound = false;
    bool targetInBound = false;

    public PassManager passManager;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetInBound)
        {
            passManager.PrintBulletHitEffects(transform.position);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (targetPos != Vector3.zero)
        {
            dir = targetPos - transform.position;
            rb.MovePosition(rb.position + dir * bulletSpeed * Time.fixedDeltaTime);

            if (dir.sqrMagnitude < 0.15f)
            {
                bulletInBound = true;
            }
        } 
    }

    private void LateUpdate()
    {
        //bullet이 타격 실패했을 때 즉시 삭제
        if(bulletInBound && !targetInBound)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Passenger target = collision.gameObject.GetComponentInParent<Passenger>();
        if (target != null && bulletInBound)
        {
            if(target.passState == PassengerState.idle)
            {
                target.Freeze();
                targetInBound = true;
            }
            else if(target.passState == PassengerState.leave)
            {
                target.Freeze();
                targetInBound = true;
                passManager.PrintInfo(9);
            }
        }
        else if (passManager.stageState == StationStageState.Tutorial)
        {
            TutorialNPC npc = collision.gameObject.GetComponentInParent<TutorialNPC>();
            if(npc != null)
            {
                npc.Freeze();
                targetInBound = true;
            }
        }
    }
}
