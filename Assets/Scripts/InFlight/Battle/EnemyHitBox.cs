using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    Transform player;
    Stat _stat;
    PlayerStat _playerStat;
    EnemyController _enemyController;
    PlayerController _playerController;
    AudioSource audio_domino;
    bool hasSoundPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        _stat = GetComponentInParent<Stat>();
        _playerStat = player.GetComponent<PlayerStat>();
        _enemyController = GetComponentInParent<EnemyController>();
        _playerController = player.GetComponent<PlayerController>();

        audio_domino = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 공격 당함
        if (collision.gameObject.layer == (int)Define.Layer.AttackRange && collision.gameObject.tag == "Player")
        {
            Vector3 centerPos = new Vector3(_enemyController.transform.position.x, _enemyController.transform.position.y, 0);
            _playerController.PrintHitEffects(collision.transform.position, centerPos, 1);
            Managers.Sound.Play("hit1");

            if (_playerController._mouse == Define.MouseEvent.PointerDown)
            {
                _stat.Hp -= _playerStat.Attack;
                Debug.Log($"{transform.parent.parent.name} Hp : {_stat.Hp}");
                Managers.Sound.Play("hit1");

                if (_stat.Hp < 1)
                {
                    _enemyController._state = Define.State.Fall;
                }
                else
                {
                    _enemyController._state = Define.State.Attacked;
                }
            }
            else // 버티기
            {
                _enemyController._state = Define.State.Attacked;
                _playerStat.Stamina -= 2.5f;
                //_playerStat.Hp -= _stat.Attack / 2;

                if (_playerStat.Hp < 1)
                {
                    _playerController._state = Define.State.Die;
                }
            }
        }
        else if (collision.gameObject.layer == (int)Define.Layer.Fall)
        {
            if (!hasSoundPlayed)
            {
                audio_domino.Play();
                hasSoundPlayed = true;
            }

            _enemyController.LastDirection = collision.GetComponentInParent<EnemyController>().LastDirection;
            _enemyController.domino = true;
            _stat.Hp -= _stat.Hp;
            Debug.Log("Domino!");
            Debug.Log($"{transform.parent.name} Hp : {_stat.Hp}");
            _enemyController._state = Define.State.Fall;
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == (int)Define.Layer.Fall)
    //    {
    //        if(!hasSoundPlayed)
    //        {
    //            audio_domino.Play();
    //            hasSoundPlayed = true;
    //        }

    //        _enemyController.LastDirection = collision.GetComponentInParent<EnemyController>().LastDirection;
    //        _enemyController.domino = true;
    //        _stat.Hp -= _stat.Hp;
    //        Debug.Log("Domino!");
    //        Debug.Log($"{transform.parent.name} Hp : {_stat.Hp}");
    //        _enemyController._state = Define.State.Fall;
    //    }
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Enemy)
        {
            if (_enemyController.direction.sqrMagnitude > collision.transform.GetComponent<EnemyController>().direction.sqrMagnitude)
            {
                _stat.isStop = true;
            }
            else
            {
                _stat.isStop = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Enemy)
        {
            if (_enemyController.direction.sqrMagnitude > collision.transform.GetComponent<EnemyController>().direction.sqrMagnitude)
            {
                _stat.isStop = false;
            }
        }
    }

}
