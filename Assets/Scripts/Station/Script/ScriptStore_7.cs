using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStore_7 : ScriptStore
{
    protected override void Awake()
    {
        base.Awake();
        stDictionary.Add("scene7_0", sentenceList_0);
    }

    [SerializeField] List<string> sentenceList_0 = new List<string>();
}
