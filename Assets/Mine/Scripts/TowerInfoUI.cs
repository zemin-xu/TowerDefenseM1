using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Displaying the selecting building tower info.
public class TowerInfoUI : MonoBehaviour
{
    public TMP_Text towerNameText;
    public TMP_Text towerDescriptionText;

    public void UpdateTowerInfo(Tower tower)
    {
        towerNameText.text = tower.towerName;
        towerDescriptionText.text = tower.levels[0].towerDescription;
    }
}
