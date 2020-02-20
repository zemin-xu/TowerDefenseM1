using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionGameFramework.Health;
using Core.Utilities;

public class Tower : Targetable
{
    public TowerLevel[] levels;

    public string towerName;

    public int currentLevel { get; protected set; }

    public TowerLevel currTowerLevel { get; protected set; }

    public GameObject ghost;

    public int purchaseCost
    {
        get { return levels[0].cost; }
    }

    public bool isAtMaxLevel
    {
        get { return currentLevel == levels.Length - 1; }
    }

    public void OnConfirmedTower()
    {
        SetLevel(0);
        if (ghost != null)
        {
            Destroy(ghost);
        }
    }

    private void Update()
     {
        
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
        currentLevel = level;
        if (currTowerLevel != null)
        {
            Destroy(currTowerLevel.gameObject);
        }

        currTowerLevel = Instantiate(levels[currentLevel], transform);
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
