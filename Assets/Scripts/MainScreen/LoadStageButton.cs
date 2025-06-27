using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStageButton : MonoBehaviour
{
    UIController_Main uiController;
    [SerializeField] AudioSource audio_ominous, audio_stop;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.Find("Canvas").GetComponent<UIController_Main>();
    }

    public void ActivateMiniShipButton()
    {
        uiController.ActivateMiniShipButton();
    }

    public void PrintSound_Ominous()
    {
        audio_ominous.Play();
    }

    public void PrintSound_Stop()
    {
        audio_stop.Play();
    }

    public void AutoLoadStage()
    {
        StartCoroutine(uiController.LoadStage());
    }
}
