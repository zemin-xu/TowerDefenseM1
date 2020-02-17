using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionGameFramework.Health;
using Core.Utilities;

public class Tower : MonoBehaviour
{
    public TowerLevel[] levels;

    public string towerName;


    public int purchaseCost
    {
        get { return levels[0].cost; }
    }

    private void Start()
    {
		Instantiate(levels[0].gameObject, transform);
    }

}
