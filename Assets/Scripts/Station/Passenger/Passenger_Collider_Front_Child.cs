using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger_Collider_Front_Child : Passenger_Collider_Front
{
    Passenger_Collider_Foot footCollider;

    // Start is called before the first frame update
    void Start()
    {
        passenger = GetComponentInParent<Passenger>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Back"))
        {
            //³Ñ¾î¶ß¸®±â
            Passenger target = collision.gameObject.GetComponentInParent<Passenger>();

            if (target.passState == PassengerState.idle)
            {
                audioSource.Play();
                target.FallByOther(passenger.dir, passenger.passState);
            }
        }   
    }
}
