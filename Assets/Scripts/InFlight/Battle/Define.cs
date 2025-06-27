using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }

    public enum State
    {
        Attack,
        Attacked,
        Chase,
        Die,
        Fall,
        Idle,
        ParriedFall,
        KnockBack,
        Parry,
        Patrol,
        SearchAround,
    }

    public enum Layer
    {
        Player_Battle = 20,
        Enemy = 22,
        AttackRange = 24,
        HitBox = 25,
        Parried = 26,
        Wall = 27,
        Fall = 28,
        PushWall = 29,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        None,
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum CameraMode
    {
        QuarterView,
    }
}
