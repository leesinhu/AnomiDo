using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimEvent : MonoBehaviour
{
    Player player;

    public void AttackFront()
    {
        player.AttackFront();
    }

    public void AttackFrontEnd()
    {
        player.AttackFrontEnd();
    }

    public void CancleDelay()
    {
        player.CancleDelay();
    }

    void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }
}
