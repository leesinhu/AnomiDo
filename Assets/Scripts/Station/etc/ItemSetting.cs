using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    public float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        int randomIndex = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randomIndex];

        
        Destroy(gameObject, destroyTime);
     }

    // Update is called once per frame
    void Update()
    {
        
    }
}
