using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger_Collider_Foot : MonoBehaviour
{
    Passenger passenger;
    PolygonCollider2D footCollider;
    // Start is called before the first frame update
    void Start()
    {
        passenger = GetComponentInParent<Passenger>();
        footCollider = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //¹«´Ü ÀÌÅ» »óÅÂÀÏ ¶§ ´Ù¸¥ ½Â°´ ³Ñ¾î¶ß¸²
        if (collision.gameObject.layer == LayerMask.NameToLayer("Back"))
        {
            Passenger target = collision.gameObject.GetComponentInParent<Passenger>();
            if (passenger.passState == PassengerState.leave && target.passState == PassengerState.idle)
            {
                AudioSource source = passenger.GetComponent<AudioSource>();
                source.Play();
                target.ReserveNewSortingOrder(-1);
                target.FallByOther(passenger.dir);
            }
        }
    }

    public void Resize(Quaternion _rotation, Vector2 _offset, Vector2 _size)
    {
        transform.rotation = _rotation;
        footCollider.offset = _offset;
        //footCollider.size = _size;
    }
}
