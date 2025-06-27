using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SymbolState
{
    red,
    yellow,
    green,
    none,
}

public class SymbolAnimation : MonoBehaviour
{
    public float floatStrength = 0.2f;  // 위아래로 움직이는 범위
    public float floatSpeed = 3f;     // 움직이는 속도
    [SerializeField] float heightOffset = 0;

    private Vector3 startPos;
    GameObject parent;
    Vector3 parentPrevPos;

    [SerializeField] Sprite mark1, mark2, mark3; //mark1: 노랑(일반) mark2: 빨강(필수). mark3: 초록(크레딧 획득)
    SpriteRenderer spRend;
    public SymbolState symbolState { get; set; } = SymbolState.red;
    SymbolState lastState = SymbolState.red;

    private static readonly Color Transparent = new Color(1, 1, 1, 0);
    private static readonly Color Opaque = new Color(1, 1, 1, 1);

    private void Awake()
    {
        spRend = GetComponent<SpriteRenderer>();
        spRend.sprite = mark1;
    }

    void Start()
    {
        parent = transform.parent.gameObject;
        parentPrevPos = parent.transform.position;

        startPos = new Vector3(parent.transform.position.x, parent.transform.position.y + heightOffset, parent.transform.position.z);
        StartCoroutine(FloatUpDown());
    }

    private void Update()
    {
        if(parent.transform.position != parentPrevPos)
        {
            startPos = new Vector3(parent.transform.position.x, parent.transform.position.y + heightOffset, parent.transform.position.z);
            parentPrevPos = parent.transform.position;
        }
        
    }

    IEnumerator FloatUpDown()
    {
        while (true)
        {
            float time = 0f;
            while (time < Mathf.PI * 2) // 한 주기 (360도)
            {
                float newY = startPos.y + Mathf.Sin(time) * floatStrength;
                transform.position = new Vector3(startPos.x, newY, startPos.z);
                time += Time.deltaTime * floatSpeed;
                yield return null;
            }
        }
    }

    public void SetMark(bool isVisible)
    {
        if(isVisible)
        {
            spRend.color = Opaque;
            symbolState = lastState;
            StartCoroutine(FloatUpDown());
        }
        else
        {
            spRend.color = Transparent;
            symbolState = SymbolState.none;
        }
    }

    public void ConvertMark(int i = 1)
    {
        if(i == 1)
        {
            spRend.sprite = mark1;
            symbolState = SymbolState.red;
            lastState = SymbolState.red;
        }
        else if(i == 0 && mark2 != null)
        {
            spRend.sprite = mark2;
            symbolState = SymbolState.yellow;
            lastState = SymbolState.yellow;
        }
        else if(i == 2 && mark3 != null)
        {
            spRend.sprite = mark3;
            symbolState = SymbolState.green;
            lastState = SymbolState.green;
        }
    }
}
