using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// TowerOption when clicking on a built tower.
public class TowerOptionUI : MonoBehaviour
{
    public TMP_Text towerNameText;
    public TMP_Text towerDescriptionText;
    public TMP_Text upgradeCostText;
    public TMP_Text sellCostText;

    private Tower currentSelectingTower;
    private Camera maincamera;

    private void OnEnable()
    {
        maincamera = Camera.main;
        if (GameUI.instance != null)
            GameUI.instance.selectedTower += OnSelectedTower;
    }

    private void OnDisable()
    {
        GameUI.instance.selectedTower -= OnSelectedTower;
        currentSelectingTower = null;
    }

    private void Update()
    {
        AdjustPosition();
    }

    // Modify the position of UI based on tower's position.
    private void AdjustPosition()
    {
        if (currentSelectingTower == null)
        {
            return;
        }
        Vector3 point = maincamera.WorldToScreenPoint(currentSelectingTower.position);
        point.z = 0;
        transform.position = point;
    }

    // update tower sell price and upgrade price.
    public void UpdateTowerOption(Tower tower)
    {
        if (tower != null)
        {
            towerNameText.text = tower.towerName;
            towerDescriptionText.text = tower.levels[tower.currentLevel].towerDescription;
            currentSelectingTower = tower;

            int cost = tower.GetCostForNextLevel();
            if (cost != -1)
            {
                upgradeCostText.text = cost.ToString();
            }
            else
            {
                upgradeCostText.text = "0";
            }
            sellCostText.text = (tower.currentPrice / 2).ToString();
        }
    }

    public void OnClickSellButton()
    {
        currentSelectingTower.SellTower();
        gameObject.SetActive(false);

    }

    public void OnClickUpgradeButton()
    {
        currentSelectingTower.UpgradeTower();
        UpdateTowerOption(currentSelectingTower);
    }

    private void OnSelectedTower(Tower tower)
    {
        UpdateTowerOption(tower);
    }
}
