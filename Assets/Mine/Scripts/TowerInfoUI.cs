using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerInfoUI : MonoBehaviour
{
    public TMP_Text towerName;
    public TMP_Text towerDescription;
    public TMP_Text upgradeCost;
    public TMP_Text sellCost;

    public void DisplayTowerInfo(Tower tower)
    {
        towerName.text = tower.towerName;
        towerDescription.text = tower.currTowerLevel.towerDescription;

        int cost = tower.GetCostForNextLevel();
        if (cost != -1)
        {
            upgradeCost.text = cost.ToString();
        }
        else
        {
            upgradeCost.text = "0";
        }
        sellCost.text = (tower.currentPrice / 2).ToString();
    }
}
