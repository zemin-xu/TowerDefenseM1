using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerOptionUI : MonoBehaviour
{
    public TMP_Text towerNameText;
    public TMP_Text towerDescriptionText;
    public TMP_Text upgradeCostText;
    public TMP_Text sellCostText;

    public void UpdateTowerOption(Tower tower)
    {
        towerNameText.text = tower.towerName;
        towerDescriptionText.text = tower.levels[0].towerDescription;

        int cost = tower.GetCostForNextLevel();
        if (cost != -1)
        {
            upgradeCostText.text = "Upgrade: " + cost.ToString();
        }
        else
        {
            upgradeCostText.text = "0";
        }
        sellCostText.text ="Sell: " + (tower.currentPrice / 2).ToString();
    }

}
