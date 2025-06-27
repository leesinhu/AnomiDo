using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutNpcState
{
    none,
    idle,
    arrive,
    freeze,
}

public class TutorialNPC : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    PassManager passManager;
    Animator anim_iceBreak;

    Vector3 spot = new Vector3(3, -0.85f);
    Vector2 movement;

    float delayTime = 0;
    public TutNpcState state { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
        anim_iceBreak = transform.GetChild(2).GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = TutNpcState.none;
    }

    private void OnEnable()
    {
        anim.SetFloat("moveX", -1);
        anim.SetFloat("moveY", -1);
    }

    void Update()
    {
        if(state == TutNpcState.none)
        {
            delayTime += Time.deltaTime;
            if (delayTime >= 0.5f)
                state = TutNpcState.idle;
        }

        if(passManager.playerState == PlayerState.inSide)
        {
            anim.SetBool("heat", true);
        }
        else //outside
        {
            anim.SetBool("heat", false);
        }
    }

    private void FixedUpdate()
    {
        if ((new Vector2(spot.x, spot.y) - new Vector2(transform.position.x, transform.position.y)).sqrMagnitude > 0.025f)
        {
            movement = new Vector2(spot.x - transform.position.x, spot.y - transform.position.y).normalized;
        }
        else
        {
            movement = Vector2.zero;
            anim.SetBool("isWalk", false);
            state = TutNpcState.arrive;
        }

        if (movement != Vector2.zero && state == TutNpcState.idle)
        {
            anim.SetFloat("moveX", -1);
            anim.SetFloat("moveY", -1);
            anim.SetBool("isWalk", true);
            rb.MovePosition(rb.position + movement * 1.0f * Time.fixedDeltaTime);
        }
    }

    public void Freeze()
    {
        anim.SetBool("isFreeze", true);
        state = TutNpcState.freeze;
    }

    public void UnFreeze()
    {
        anim.SetBool("isFreeze", false);
        state = TutNpcState.idle;
        anim_iceBreak.SetTrigger("iceBreak");
    }
}
