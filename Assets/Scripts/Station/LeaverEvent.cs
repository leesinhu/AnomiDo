using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaverEvent : MonoBehaviour
{
    PassManager passManager;
    public float eventInterval = 30;
    float gameTimer = 0;
    bool eventSuccess = true;

    // Start is called before the first frame update
    private void Awake()
    {
        passManager = GameObject.Find("PassManager/SpawnPoint").GetComponent<PassManager>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (passManager.stageState == StationStageState.AfterOpen)
        {
            gameTimer += Time.deltaTime;
            if(!eventSuccess)
            {
                gameTimer += eventInterval / 2;
                eventSuccess = true;
            }
        }
        
        if(gameTimer > eventInterval)
        {
            gameTimer = 0;
            eventSuccess = passManager.InvokeLeaverEvent();
        }
    }
}
