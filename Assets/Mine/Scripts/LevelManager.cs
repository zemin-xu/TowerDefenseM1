using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.Utilities;

// a class to control the variables in level like money and life.
public class LevelManager : Singleton<LevelManager>
{

    // Don't know hot to use getter setter as well as setting them in Editor.
    public int money;
    public int life;

    public event Action moneyUpdated;
    public event Action lifeUpdated;

    public event Action gameover;
    public event Action gameWin;

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
        if (lifeUpdated != null)
            lifeUpdated();

        if (life <= 0 && gameover != null)
            gameover();
    }

    public void TriggerGameWin()
    {
        if (gameWin != null)
            gameWin();
    }

    // Detect whether all of the enemies are cleaned.
    public void DetectAllEnemiesCleaned()
    {
        if (WaveManager.instance.isAllWavesSpawned)
        {
            if (WaveManager.instance.enemyNum <= 0 && gameover != null)
            {
                gameWin();
            }
        }
    }
}