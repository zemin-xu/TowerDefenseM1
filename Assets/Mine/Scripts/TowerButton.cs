using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerButton : MonoBehaviour
{
    public Tower tower;
    public TMP_Text purchasePriceText;

    public event Action<Tower> towerButtonClick;


    void Start()
    {
        // Get the cost for the lv1 of this tower.
        if (tower != null && tower.levels[0] != null)
        {
            purchasePriceText.text = tower.levels[0].cost.ToString();
        }

    }

    // Action when clicking tower creating button.
    public void OnTowerButtonClick()
    {
        if (tower == null)
        {
            Debug.LogWarning("tower prefab not existed");
            return;
        }
        GameUI.instance.state = InteractiveState.Building;

        GameObject go = Instantiate(tower.gameObject);
        GameUI.instance.currentBuildingTower = go.GetComponent<Tower>();
        GameUI.instance.OnTowerButtonClicked(tower);
    }



}
