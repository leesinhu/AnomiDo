using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAndFog : MonoBehaviour
{
    [SerializeField] Sprite sprite_open, sprite_close;
    [SerializeField] GameObject[] targetFog;
    SpriteRenderer spRend;
    public bool isOpen;
    new BoxCollider2D collider = null;
    bool isDestroyed = false;
    private void Awake()
    {
        spRend = transform.GetComponent<SpriteRenderer>();
        collider = transform.GetComponent<BoxCollider2D>();

        SetDoor(isOpen);
    }

    private void Update()
    {

    }

    public void DestroyDoor()
    {
        if (targetFog != null)
        {
            foreach (GameObject target in targetFog)
                target.SetActive(false);
        }

        this.gameObject.SetActive(false);
        isDestroyed = true;
    }

    public void SetDoor(bool b)
    {
        if(!isDestroyed)
        {
            if (b) //true, ø≠∏≤
            {
                spRend.sprite = sprite_open;
                //collider.enabled = false;
                collider.offset = new Vector2(-0.37f, 0);
                collider.size = new Vector2(0.25f, 1);

                if (targetFog != null)
                {
                    foreach (GameObject target in targetFog)
                        target.SetActive(false);
                }
                isOpen = true;
            }
            else  //false, ¿·±Ë
            {
                spRend.sprite = sprite_close;
                collider.enabled = true;
                collider.offset = new Vector2(0, 0.37f);
                collider.size = new Vector2(1, 0.25f);

                if (targetFog != null)
                {
                    foreach (GameObject target in targetFog)
                        target.SetActive(true);
                }

                isOpen = false;
            }
        } 
    }
}
