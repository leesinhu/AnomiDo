using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    int minX = -65;
    int maxX = 65;

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.left * 0.3f * Time.deltaTime);
        if(transform.position.x < minX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
    }
}
