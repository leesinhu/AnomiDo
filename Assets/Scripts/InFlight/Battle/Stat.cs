using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] protected int _hp;
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _attack;
    [SerializeField]
    protected float _moveSpeed;
    public bool isStop;

    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {
        _moveSpeed = 2.4f;

        isStop = false;
    }

    private void Update()
    {
        if (isStop)
        {
            _moveSpeed = 0.0f;
        }
        else
        {
            _moveSpeed = 2.4f;
        }
    }
}
