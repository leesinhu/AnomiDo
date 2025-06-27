using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidableObj : MonoBehaviour
{
    GameObject childObj;
    SpriteRenderer spRend, childSpRend;
    Color originColor, childOriginColor;

    [SerializeField] int childIndex = 0;
    [SerializeField] bool sibilingInsteadChild = false;
    void Start()
    {
        if(GetComponent<SpriteRenderer>() != null)
        {
            spRend = GetComponent<SpriteRenderer>();
        }
        else
        {
            spRend = transform.parent.GetComponent<SpriteRenderer>();
        }
        
        originColor = spRend.color;
        
        if (!sibilingInsteadChild)
        {
            if(transform.childCount > 0)
            {
                childObj = transform.GetChild(childIndex).gameObject;
                childSpRend = childObj.GetComponent<SpriteRenderer>();
                childOriginColor = childSpRend.color;
            }
        }
        else
        {
            childObj = transform.parent.GetChild(childIndex).gameObject;
            childSpRend = childObj.GetComponent<SpriteRenderer>();
            childOriginColor = childSpRend.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fog"))
        {
            spRend.color = new Color(1, 1, 1, 0);

            if(childObj != null)
                childSpRend.color = new Color(1, 1, 1, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fog") && spRend.color.a > 0)
        {
            spRend.color = new Color(1, 1, 1, 0);

            if (childObj != null)
                childSpRend.color = new Color(1, 1, 1, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fog"))
        {
            spRend.color = originColor; //그림자(alpha 적용)의 정상 복귀

            if (childObj != null && childObj.name != "InteractMark")
                childSpRend.color = childOriginColor;
        }
    }
}
