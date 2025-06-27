using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    float _stamina;
    [SerializeField]
    float _maxStamina;
    [SerializeField]
    float _playerSpeed;

    public float Stamina { get { return _stamina; } set { _stamina = value; } }
    public float MaxStamina { get { return _maxStamina; } set { _maxStamina = value; } }
    public float PlayerSpeed { get { return _playerSpeed; } set { _playerSpeed = value; } }

    private void Start()
    {
        _playerSpeed = 5.0f;
        _stamina = 100.0f;
        _maxStamina = 100.0f;
    }
}
