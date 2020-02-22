using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionGameFramework.Health;
using Core.Utilities;

public class Tower : Targetable
{
    public TowerLevel[] levels;

    public string towerName;

    public int currentPrice { get; protected set; }

    public int currentLevel { get; protected set; }

    public TowerLevel currTowerLevel { get; protected set; }

    public GameObject ghost;



    public event Action placementFailed;
    public event Action placementSucceeded;

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

    public void TryConfirmPlacement(Base baseModel)
    {
        if (!baseModel.isOccupied && TryPurchase())
        {
            ConfirmPlacement(baseModel.towerPoint.position);
            baseModel.isOccupied = true;
            GameUI.instance.state = InteractiveState.Default;
            GameUI.instance.currentBuildingTower = null;
        }
        else
        {
            GameUI.instance.state = InteractiveState.Default;
            GameUI.instance.currentBuildingTower = null;
            Debug.LogWarning("this place is occupied or you are lack of money.");
            Destroy(gameObject);
        }
    }

    public bool TryPurchase()
    {
        if (LevelManager.instance.money - purchaseCost >= 0)
        {
            LevelManager.instance.UpdateMoney(-purchaseCost);
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
        SetLevel(currentLevel + 1);
        return true;
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
