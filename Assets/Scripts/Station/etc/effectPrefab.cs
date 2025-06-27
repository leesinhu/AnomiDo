using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectPrefab : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        AnimationClip clip = anim.runtimeAnimatorController.animationClips[0];
        float clipLength = clip.length;
        Destroy(gameObject, clipLength);
    }
}
