using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcZ_Enemy : MonoBehaviour
{
    float previousY;
    Transform parentTransform;
    float scaleFactor = 0.01f;
    // Start is called before the first frame update
    private void Awake()
    {
        parentTransform = transform.parent;
    }
    void Start()
    {
        previousY = parentTransform.position.y;
        //float newZ = (parentTransform.position.y + 0.75f) * scaleFactor - parentTransform.localPosition.z; 
        transform.localPosition = new Vector3(0, 0.75f, 0);
    }

    void Update()
    {
        if (Mathf.Abs(parentTransform.position.y - previousY) > 0.01f)
        {
            //float newZ = (parentTransform.position.y + 0.75f) * scaleFactor - parentTransform.localPosition.z;
            transform.localPosition = new Vector3(0, 0.75f, 0);
            previousY = parentTransform.position.y;
        }  
    }
}
