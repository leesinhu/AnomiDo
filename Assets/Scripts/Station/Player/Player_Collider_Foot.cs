using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collider_Foot : MonoBehaviour
{
    PassManager passManager;
    Player player;
    AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어와 승객 충돌
        if (collision.gameObject.layer == LayerMask.NameToLayer("Back"))
        {
            Passenger target = collision.gameObject.GetComponentInParent<Passenger>();
            if (target.passState == PassengerState.idle)
            {
                passManager.PrintInfo(7);
                audioSource.Play();
                target.ReserveNewSortingOrder(-1);
                target.FallByOther(player.dir);
                passManager.gameTimer -= 5;
            }
        }

        //게이트 범위(in)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Gate"))
        {
            passManager.PlayerIsInGate();
        }

        //아이템 획득
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            passManager.SetBulletGage(1.2f);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //게이트 범위(out)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Gate"))
        {
            passManager.PlayerIsNotInGate();
        }
    }

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
    }
}
