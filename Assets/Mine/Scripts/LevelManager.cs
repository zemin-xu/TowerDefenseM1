using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.Utilities;

public class LevelManager : Singleton<LevelManager>
{

    // Don't know hot to use getter setter as well as setting them in Editor.
    public int money;
    public int life;

    public event Action moneyUpdated;
    public event Action lifeUpdated;

    private GameUI gameUI;

    private void Start()
    {
        gameUI = GameUI.instance;
    }

    public void UpdateMoney(int value)
    {
        money += value;
        if (moneyUpdated != null)
            moneyUpdated();
    }


    public void UpdateLife(int value)
    {
        life += value;
        if (lifeUpdated!= null)
            lifeUpdated();
    }
    


}