using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    PlayerStat _stat;
    PlayerController _playerController;
    EnemyController _enemyController;
    Collider2D tempCollider;

    UiController_InFlight uiController;
    // Start is called before the first frame update
    void Start()
    {
        _stat = GetComponentInParent<PlayerStat>();
        _playerController = GetComponentInParent<PlayerController>();

        uiController = GameObject.Find("Canvas").GetComponent<UiController_InFlight>();
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Enemy)
        {
            //_stat.Hp -= collision.transform.GetComponentInParent<Stat>().Attack;
            _enemyController = collision.transform.GetComponentInParent<EnemyController>();
            _playerController._enemyDirection = (_enemyController.LastDirection + 4) % 8;
            Debug.Log($"Player Hp : {_stat.Hp}");

            if (_stat.Hp < 1)
            {
                _playerController._state = Define.State.Die;
            }
            else
            {
                _playerController._state = Define.State.Attacked;
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.AttackRange)
        {
            Vector3 centerPos = new Vector3(_playerController.transform.position.x, _playerController.transform.position.y, 0);
            _playerController.PrintHitEffects(collision.gameObject.transform.position, centerPos, 1);

            _stat.Hp -= collision.GetComponentInParent<Stat>().Attack;
            _enemyController = collision.GetComponentInParent<EnemyController>();
            _playerController._enemyDirection = (_enemyController.LastDirection + 4) % 8;
            Debug.Log($"Player Hp : {_stat.Hp}");
            Managers.Sound.Play("hit1");

            if (_stat.Hp < 1)
            {
                _playerController._state = Define.State.Die;
            }
            else
            {
                _playerController._state = Define.State.Attacked;
            }
        }
        else if(collision.gameObject.layer == (int)Define.Layer.HitBox)
        {
            _enemyController = collision.GetComponentInParent<EnemyController>();
            _playerController._enemyDirection = (_enemyController.LastDirection + 4) % 8;
            Managers.Sound.Play("hit1");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            SymbolAnimation interactMark = collision.gameObject.transform.Find("InteractMark").GetComponent<SymbolAnimation>();
            if(interactMark.symbolState != SymbolState.none)
            {
                uiController.SetInterActText(true, "대화");
                _playerController.npcName = collision.gameObject.name;
            }
            else
            {
                _playerController.npcName = null;
            }

        }
        else if(collision.gameObject.tag == "Object")
        {          
            SymbolAnimation interactMark = collision.gameObject.transform.Find("InteractMark").GetComponent<SymbolAnimation>();
            
            if (collision.gameObject.name == "Cooling Cannon")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "올라가기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
                    
            }
            else if (collision.gameObject.name == "PlayerBed")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "취침");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "Door_Storage3")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "문 열기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "Cargo")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "살펴보기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "ElectricPanel")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "살펴보기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "FlightControl")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "조작하기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "FlightControl2")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "조작하기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "Door_Engine")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "문 열기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "Door_Control")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "문 열기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "Door_Storage2")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "문 열기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "Door_Elect1")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "문 열기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if(collision.gameObject.name == "Door_Control_Alter")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "문 잠그기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
            else if (collision.gameObject.name == "Door_Storage3_Alter")
            {
                if (interactMark.symbolState != SymbolState.none)
                {
                    uiController.SetInterActText(true, "문 잠그기");
                    _playerController.objName = collision.gameObject.name;
                }
                else
                {
                    _playerController.objName = null;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            _playerController.npcName = null;
            uiController.SetInterActText(false);
        }
        else if (collision.gameObject.tag == "Object")
        {
            _playerController.objName = null;
            uiController.SetInterActText(false);
        }
    }
}
