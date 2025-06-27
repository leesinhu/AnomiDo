using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collider_Front : MonoBehaviour
{
    Player player;
    PassManager passManager;
    AudioSource audioSource;
    AudioClip clip_hit, clip_iceBreaking;

    public bool isAttacking { get; set; } = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isAttacking && collision.gameObject.layer == LayerMask.NameToLayer("Back"))
        {
            Passenger target = collision.gameObject.GetComponentInParent<Passenger>();

            if(target != null)
            {
                if (target.passState == PassengerState.idle)
                {
                    passManager.PrintInfo(3);

                    audioSource.clip = clip_hit;
                    audioSource.Play();

                    target.FallByOther(player.dir);
                    target.ReserveNewSortingOrder(-1);

                    player.PrintHitEffects(target.transform.position);
                }
                else if(target.passState == PassengerState.leave)
                {
                    passManager.PrintInfo(8);

                    audioSource.clip = clip_hit;
                    audioSource.Play();

                    target.FallByOther(player.dir);
                    target.ReserveNewSortingOrder(-1);

                    player.PrintHitEffects(target.transform.position);
                }    
                else if (target.passState == PassengerState.freeze)
                {
                    audioSource.clip = clip_iceBreaking;
                    audioSource.Play();
                    target.UnFreeze();
                    isAttacking = false;

                    player.PrintHitEffects(target.transform.position);
                }
            }
            else if(passManager.tut_playerAct)
            {
                TutorialNPC npc = collision.gameObject.GetComponentInParent<TutorialNPC>();
                audioSource.clip = clip_iceBreaking;
                audioSource.Play();

                player.PrintHitEffects(npc.transform.position);
                npc.UnFreeze();
                isAttacking = false;
            }    
        }
    }

    private void Awake()
    {
        player = GetComponentInParent <Player>();
        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
        audioSource = GetComponent<AudioSource>();

        clip_hit = Resources.Load<AudioClip>("Sound/" + "hit1");
        clip_iceBreaking = Resources.Load<AudioClip>("Sound/" + "IceBreaking");
    }
}
