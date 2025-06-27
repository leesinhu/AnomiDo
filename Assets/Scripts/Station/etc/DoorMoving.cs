using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMoving : MonoBehaviour
{
    Vector2 pos1, pos2;
    Vector2 dir; //���� �ö󰡴� ����
    float distance; //�� �̵��Ÿ�
    Rigidbody2D rb;
    Coroutine coroutine;
    public bool hasOpened { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        pos1 = transform.GetChild(0).position;
        pos2 = transform.GetChild(1).position;
        dir = (pos2 - pos1).normalized;
        distance = Vector2.Distance(pos1, pos2);

        rb = GetComponent<Rigidbody2D>();
    }

    public void OpenDoor(float _duration = 1.5f)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);

        if(!hasOpened)
        {
            coroutine = StartCoroutine(Cor_OpenDoor(_duration));
        }
    }

    public void CloseDoor(float _duration = 1.5f)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        if(hasOpened)
        {
            coroutine = StartCoroutine(Cor_CloseDoor(_duration));       
        }
    }


    IEnumerator Cor_OpenDoor(float _duration)
    {
        float duration = _duration;
        float elapsedTime = 0;
        Vector2 startPos =  transform.position;
        
        Vector2 targetPos = startPos + dir * distance;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            rb.MovePosition(Vector2.Lerp(startPos, targetPos, t)); // �ε巯�� �̵�
            yield return null;
        }

        rb.MovePosition(targetPos); // ���� ��ġ ����
        hasOpened = true;
    }

    IEnumerator Cor_CloseDoor(float _duration)
    {
        float duration = _duration;
        float elapsedTime = 0;
        Vector2 startPos = transform.position;
        Vector2 targetPos = startPos - dir * distance;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            rb.MovePosition(Vector2.Lerp(startPos, targetPos, t)); // �ε巯�� �̵�
            yield return null;
        }

        rb.MovePosition(targetPos); // ���� ��ġ ����
        hasOpened = false;
    }
}
