using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStore_0 : ScriptStore
{
    protected override void Awake()
    {
        stDictionary.Add("main_0", sentenceList_0);
        stDictionary.Add("main_1", sentenceList_1);
        stDictionary.Add("main_2", sentenceList_2);
    }

    [SerializeField] List<string> sentenceList_0 = new List<string>();
    [SerializeField] List<string> sentenceList_1 = new List<string>();
    [SerializeField] List<string> sentenceList_2 = new List<string>();
}
