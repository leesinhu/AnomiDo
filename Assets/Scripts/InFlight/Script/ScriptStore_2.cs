using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStore_2 : ScriptStore
{
    protected override void Awake()
    {
        base.Awake();
        stDictionary.Add("scene2_0", sentenceList_0);
        stDictionary.Add("scene2_senior", sentenceList_1);
        stDictionary.Add("scene2_senior_2", sentenceList_2);
        stDictionary.Add("scene2_1", sentenceList_3);
        stDictionary.Add("scene2_2", sentenceList_4);
        stDictionary.Add("scene2_3", sentenceList_5);
        stDictionary.Add("scene2_red", sentenceList_6);
        stDictionary.Add("scene2_orange", sentenceList_7);
        stDictionary.Add("scene2_yellow", sentenceList_8);
        stDictionary.Add("scene2_senior_3", sentenceList_9);
        stDictionary.Add("scene2_orange2", sentenceList_10);
        stDictionary.Add("scene2_yellow2", sentenceList_11);
        stDictionary.Add("scene2_senior_4", sentenceList_12);
    }

    [SerializeField] List<string> sentenceList_0 = new List<string>();
    [SerializeField] List<string> sentenceList_1 = new List<string>();
    [SerializeField] List<string> sentenceList_2 = new List<string>();
    [SerializeField] List<string> sentenceList_3 = new List<string>();
    [SerializeField] List<string> sentenceList_4 = new List<string>();
    [SerializeField] List<string> sentenceList_5 = new List<string>();
    [SerializeField] List<string> sentenceList_6 = new List<string>();
    [SerializeField] List<string> sentenceList_7 = new List<string>();
    [SerializeField] List<string> sentenceList_8 = new List<string>();
    [SerializeField] List<string> sentenceList_9 = new List<string>();
    [SerializeField] List<string> sentenceList_10 = new List<string>();
    [SerializeField] List<string> sentenceList_11 = new List<string>();
    [SerializeField] List<string> sentenceList_12 = new List<string>();
}
