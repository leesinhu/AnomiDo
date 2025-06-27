using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcZ_Passenger : MonoBehaviour
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
        /*previousY = parentTransform.position.y;
        float newZ = parentTransform.position.y * scaleFactor - parentTransform.localPosition.z;
        transform.localPosition = new Vector3(0, 0, newZ);*/
    }

    void Update()
    {
        /*if (Mathf.Abs(parentTransform.position.y - previousY) > 0.01f)
        {
            float newZ = parentTransform.position.y * scaleFactor - parentTransform.localPosition.z;
            transform.localPosition = new Vector3(0, 0, newZ);
            previousY = parentTransform.position.y;
        }*/
    }
}
