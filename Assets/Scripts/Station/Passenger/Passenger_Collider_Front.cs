using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger_Collider_Front : MonoBehaviour
{
    Player player;
    PassManager passManager;
    protected Passenger passenger;
    protected AudioSource audioSource;

    PolygonCollider2D frontCollider;
    [HideInInspector]
    public bool isFalling = false;
    bool hasCalculatedSortingLayer = false;
    bool hasCalculatedOffset = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Passenger>().player;
        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
        passenger = GetComponentInParent<Passenger>();
        audioSource = GetComponent<AudioSource>();
        frontCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFalling && frontCollider != null)
        {
            if (passenger.passState == PassengerState.fall)
            {
                //frontCollider.offset = new Vector2(0.7f, 0.0f);
                //frontCollider.size = new Vector2(2.2f, 1.0f);
            }
            /*else if (passenger.passState == PassengerState.dead)
            {
                Vector2 dir = passenger.dir;
                if (dir == new Vector2(1, 0))
                {
                    frontCollider.offset = new Vector2(1.1f, 0.2f);
                    frontCollider.size = new Vector2(2.2f, 0.6f);
                }
                else if (dir == new Vector2(1, 1))
                {
                    frontCollider.offset = new Vector2(0.9f, -0.05f);
                    frontCollider.size = new Vector2(2.0f, 0.6f);
                    transform.rotation = Quaternion.Euler(0, 0, 35.0f);
                }
                else if (dir == new Vector2(0, 1))
                {
                    frontCollider.offset = new Vector2(0.8f, -0.05f);
                    frontCollider.size = new Vector2(1.7f, 0.7f);
                }
                else if (dir == new Vector2(-1, 1))
                {
                    frontCollider.offset = new Vector2(0.9f, 0f);
                    frontCollider.size = new Vector2(2.0f, 0.6f);
                    transform.rotation = Quaternion.Euler(0, 0, 145.0f);
                }
                else if (dir == new Vector2(-1, 0))
                {
                    frontCollider.offset = new Vector2(1.1f, -0.2f);
                    frontCollider.size = new Vector2(1.7f, 0.6f);
                }
                else if (dir == new Vector2(-1, -1))
                {
                    frontCollider.offset = new Vector2(0.9f, -0.05f);
                    frontCollider.size = new Vector2(2.0f, 0.6f);
                    transform.rotation = Quaternion.Euler(0, 0, 205.0f);
                }
                else if (dir == new Vector2(0, -1))
                {
                    frontCollider.offset = new Vector2(0.6f, 0f);
                    frontCollider.size = new Vector2(1.5f, 0.7f);
                }
                else if (dir == new Vector2(1, -1))
                {
                    frontCollider.offset = new Vector2(0.9f, 0.03f);
                    frontCollider.size = new Vector2(2.0f, 0.6f);
                    transform.rotation = Quaternion.Euler(0, 0, 335.0f);
                }
                footCollider.Resize(transform.rotation, frontCollider.offset, frontCollider.size);

                bodyCollider.transform.rotation = transform.rotation;
                bodyCollider.GetComponent<BoxCollider2D>().offset = frontCollider.offset;
                bodyCollider.GetComponent<BoxCollider2D>().size = frontCollider.size;
            }*/
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Back"))
        {
            //넘어뜨리기
            Passenger target = collision.gameObject.GetComponentInParent<Passenger>();

            if (isFalling)
            {
                if (target.passState == PassengerState.idle)
                {
                    audioSource.Play();
                    target.FallByOther(passenger.dir, passenger.passState);
                    passManager.PrintInfo(10);//연쇄 충돌

                    if (passenger.passState == PassengerState.fall && !hasCalculatedSortingLayer)
                    {
                        passenger.FixSortingOrder();
                        target.ReserveNewSortingOrder(passenger.spRender_normal.sortingOrder - 1);
                        hasCalculatedSortingLayer = true;
                    }
                    else if (passenger.passState == PassengerState.dead)
                    {
                        target.ReserveNewSortingOrder(- 1);
                    }
                }
                else if(passenger.passState == PassengerState.fall && target.passState == PassengerState.freeze && !hasCalculatedOffset)
                {
                    transform.parent.GetComponent<AudioSource>().Play();
                    Vector2 posOffset = passenger.GetDir().normalized * (-0.2f);
                    passenger.transform.position = passenger.transform.position + new Vector3(posOffset.x, posOffset.y, 0);
                    hasCalculatedOffset = true;
                }
                /*else if(target.passState != PassengerState.leave)
                {
                    if(target.passState == PassengerState.idle)
                    {
                        audioSource.Play();
                        target.FallByOther(passenger.dir, passenger.passState);
                    }
                    if(passenger.passState == PassengerState.fall)
                    {
                       StartCoroutine(target.FixSortingOrder(passenger.spRender_normal.sortingOrder - 1));
                    }
                }*/
            }
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //거리두기
        if (collision.gameObject.layer == LayerMask.NameToLayer("Back"))
        {
            Passenger target = collision.gameObject.GetComponentInParent<Passenger>();

            if (target != null)
            {
                passenger.AddFrontTarget(target.gameObject);
            }
                
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerFoot"))
        {
            passenger.AddFrontTarget(player.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //거리두기
        if (collision.gameObject.layer == LayerMask.NameToLayer("Back"))
        {
            Passenger target = collision.gameObject.GetComponentInParent<Passenger>();

            if (target != null)
            {
                passenger.RemoveFrontTarget(target.gameObject);
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerFoot"))
        {
            passenger.RemoveFrontTarget(player.gameObject);
        }
    }
}
