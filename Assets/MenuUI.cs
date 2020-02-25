using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject IndicationUI;


    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)  || Input.GetKeyDown(KeyCode.I) )
        {
            CloseIndicationUI();
        }
    }
    public void OnNewGameButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    

    public void OnIndicationButtonPressed()
    {
        if (IndicationUI.activeSelf)
        {
            CloseIndicationUI();
        }
        else
        {
            IndicationUI.SetActive(true);
        }
    }

    private void CloseIndicationUI()
    {
        IndicationUI.SetActive(false);
    }

    public void OnCloseButtonPressed()
    {
        CloseIndicationUI();
    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
    }
}


