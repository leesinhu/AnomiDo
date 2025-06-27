using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableObj : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = new Sprite[8];
    SpriteRenderer spRend;

    private void Start()
    {
        spRend = GetComponent<SpriteRenderer>();
    }
    public void ConvertSprite(int i)
    {
        if (i >= 8)
            return;

        spRend.sprite = sprites[i];
    }

    public void RotateForSpeech(Vector2 targetPos)
    {
        Vector2 dir = (targetPos - new Vector2(transform.position.x, transform.position.y)).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (angle >= -22.5f && angle < 22.5f) //right
        {
            spRend.sprite = sprites[0];
        }
        else if (angle >= 22.5f && angle < 67.5f) //right-up
        {
            spRend.sprite = sprites[1];
        }
        else if (angle >= 67.5f && angle < 112.5f) //up
        {
            spRend.sprite = sprites[2];
        }
        else if (angle >= 112.5f && angle < 157.5f) //up-left
        {
            spRend.sprite = sprites[3];
        }
        else if (angle >= 157.5f || angle < -157.5f) //left
        {
            spRend.sprite = sprites[4];
        }
        else if (angle >= -157.5f && angle < -112.5f) //left-down
        {
            spRend.sprite = sprites[5];
        }
        else if (angle >= -112.5f && angle < -67.5f) //down
        {
            spRend.sprite = sprites[6];
        }
        else if (angle >= -67.5f && angle < -22.5f) //down-right
        {
            spRend.sprite = sprites[7];
        }
    }
}
