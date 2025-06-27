using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger_AnimEvent : MonoBehaviour
{
    Passenger passenger;

    public void FallFront()
    {
        passenger.FallFront();
    }

    public void Dead()
    {
        passenger.Dead();
    }

    // Start is called before the first frame update
    void Start()
    {
        passenger = transform.parent.GetComponent<Passenger>();
    }
}
