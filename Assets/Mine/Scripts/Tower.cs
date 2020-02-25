using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionGameFramework.Health;
using Core.Utilities;

// Tower class controls the life and some basic info of a tower.
public class Tower : Targetable
{
    public TowerLevel[] levels;

    [HideInInspector]
    public bool hasBuilded;

    public string towerName;

    public int currentPrice { get; protected set; }

    public int currentLevel { get; protected set; }

    public TowerLevel currTowerLevel { get; protected set; }

    public GameObject ghost;

    // the base that it occupies.

    private Base currentBase;

    public int purchaseCost
    {
        get { return levels[0].cost; }
    }

    public bool isAtMaxLevel
    {
        get { return currentLevel == levels.Length - 1; }
    }

    public void ConfirmPlacement(Vector3 pos)
    {
        transform.position = pos;
        SetLevel(0);
        if (ghost != null)
        {
            ghost.SetActive(false);
        }
    }

// Try confirm placement is necessary in case that the fact of lacking money will stop it.
    public void TryConfirmPlacement(Base baseModel)
    {
        if (!baseModel.isOccupied && TryPurchase())
        {
            ConfirmPlacement(baseModel.towerPoint.position);
            baseModel.isOccupied = true;
            currentBase = baseModel;
            OnBuildFinished();
        }
        else
        {
            OnBuildFinished();
            Debug.LogWarning("this place is occupied or you are lack of money.");
            Destroy(gameObject);
        }

    }

    private void OnBuildFinished()
    {
       GameUI.instance.state = InteractiveState.Default;
            GameUI.instance.towerInfoUI.SetActive(false);
            GameUI.instance.towerOptionUI.SetActive(false);
            GameUI.instance.currentBuildingTower = null; 
    }

    public void SellTower()
    {
        LevelManager.instance.UpdateMoney(currentPrice / 2);
        currentBase.isOccupied = false;
        Destroy(gameObject);
    }

    public bool TryPurchase()
    {
        if (LevelManager.instance.money - purchaseCost >= 0)
        {
            LevelManager.instance.UpdateMoney(-purchaseCost);
            currentPrice += purchaseCost;
            return (true);
        }
        return (false);
    }

    public int GetCostForNextLevel()
    {
        if (isAtMaxLevel)
        {
            return -1;
        }
        return levels[currentLevel + 1].cost;
    }

    public virtual bool UpgradeTower()
    {
        if (isAtMaxLevel)
        {
            return false;
        }
        if (LevelManager.instance.money - GetCostForNextLevel() >= 0)
        {
            SetLevel(currentLevel + 1);
            LevelManager.instance.UpdateMoney(-currTowerLevel.cost);
            currentPrice += currTowerLevel.cost;
            return true;
        }
        else
        {
            Debug.LogWarning("Don't have enough money");
            return false;
        }
    }

    protected void SetLevel(int level)
    {
        if (level < 0 || level >= levels.Length)
        {
            return;
        }
        if (currTowerLevel != null)
        {
            currTowerLevel.gameObject.SetActive(false);
        }
        currentLevel = level;

        levels[currentLevel].gameObject.SetActive(true);
        currTowerLevel = levels[currentLevel];
        currTowerLevel.Initialize(this);
        ScaleHealth();
    }

    // If we upgrade a level, the health of it will be sacled upgrade.
    // If it is initialised, it is at max health.
    protected virtual void ScaleHealth()
    {
        // configuration : a Damagable object which deals with health.
        configuration.SetMaxHealth(currTowerLevel.maxHealth);

        if (currentLevel == 0)
        {
            configuration.SetHealth(currTowerLevel.maxHealth);
        }
        else
        {
            int currentHealth = Mathf.FloorToInt(configuration.normalisedHealth * currTowerLevel.maxHealth);
            configuration.SetHealth(currentHealth);
        }
    }
}
