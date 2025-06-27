using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParried : MonoBehaviour
{
    Transform player;
    Stat _stat;
    //Stat _playerStat;
    EnemyController _enemyController;
    //PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        _stat = GetComponentInParent<Stat>();
        //_playerStat = player.GetComponent<Stat>();
        //_playerController = player.GetComponent<PlayerController>();
        _enemyController = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 공격 당함
        if (collision.gameObject.layer == (int)Define.Layer.AttackRange)
        {
            PlayerController PC = collision.gameObject.GetComponentInParent<PlayerController>();
            if (PC != null && PC.isNormalHit)
            {
                Vector3 centerPos = new Vector3(_enemyController.transform.position.x, _enemyController.transform.position.y, 0);
                PC.PrintHitEffects(collision.transform.position, centerPos, 2);

                _stat.Hp -= _stat.Hp;
                Debug.Log($"Parry!");
                Debug.Log($"{transform.parent.parent.name} Hp : {_stat.Hp}");
                Managers.Sound.Play("hit2(parry)");

                PC._enemyDirection = (_enemyController.LastDirection + 4) % 8;
                PC._state = Define.State.Parry;

                _enemyController._state = Define.State.ParriedFall;
                _enemyController.domino = true;
                gameObject.SetActive(false);
            }
        }
    }
}
