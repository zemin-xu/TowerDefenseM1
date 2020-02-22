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

    private Tower currentSelectingTower;
    private Camera maincamera;

  

    private void OnEnable() {
        maincamera = Camera.main;
        GameUI.instance.selectedTower += OnSelectedTower;
    }

    private void OnDisable() {
        GameUI.instance.selectedTower -= OnSelectedTower;
        currentSelectingTower = null;
    }

    private void Update() {
        AdjustPosition();
    }

    // Modify the position of UI based on tower's position.
    private void AdjustPosition()
    {
        if (currentSelectingTower == null)
        {
            return ;
        }
        Vector3 point = maincamera.WorldToScreenPoint(currentSelectingTower.position);
        point.z = 0;
        transform.position = point;
    }

      public void UpdateTowerOption(Tower tower)
    {
        towerNameText.text = tower.towerName;
        towerDescriptionText.text = tower.levels[0].towerDescription;
        currentSelectingTower = tower;

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

    private void OnSelectedTower(Tower tower)
    {
        UpdateTowerOption(tower);
    }


}
