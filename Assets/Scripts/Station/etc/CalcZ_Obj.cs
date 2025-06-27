using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcZ_Obj : MonoBehaviour
{
    GameObject parent, newCenter;

    float charOffset = 0.75f;
    float scaleFactor = 0.01f;
    float fixedY;

    HashSet<Collider2D> collidingObjects = new HashSet<Collider2D>();
    [SerializeField] bool allowAlphaConvert = true;
    bool alphaConverted = false;

    PassManager passManager;
    SpriteRenderer spRender;
    [SerializeField]Sprite sp_normal, sp_heat;
    
    // Start is called before the first frame update
    private void Awake()
    {
        if(GameObject.Find("PassManager/SpawnPoint") != null)
            passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();

        spRender = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        parent = transform.parent.gameObject;
        newCenter = transform.GetChild(0).gameObject;
        fixedY = this.transform.position.y + newCenter.transform.localPosition.y;
    }

    private void Update()
    {
        if(passManager != null)
        {
            if (passManager.playerState == PlayerState.inSide)
            {
                spRender.sprite = sp_heat;
            }
            else //outside
            {
                spRender.sprite = sp_normal;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(allowAlphaConvert)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Body") || collision.gameObject.layer == LayerMask.NameToLayer("Player_Station"))
            {
                if (collision.transform.position.y >= this.transform.position.y)
                {
                    foreach (Transform child in parent.transform)
                    {
                        child.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 175f / 255f);
                    }
                }
/*                else
                {
                    foreach (Transform child in parent.transform)
                    {
                        child.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    }
                }*/
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collidingObjects.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //collidingObjects.Remove(collision);

 /*       if (collidingObjects.Count == 0 && allowAlphaConvert)
        {
            foreach (Transform child in parent.transform)
            {
                if(child.GetComponent<SpriteRenderer>() != null)
                {
                    child.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }  
            }
        }*/
    }
}
