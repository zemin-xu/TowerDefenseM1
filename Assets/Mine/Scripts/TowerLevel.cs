using System.Collections.Generic;
using Core.Health;
using UnityEngine;

[DisallowMultipleComponent]
public class TowerLevel : MonoBehaviour
{
    public int cost;
    public int maxHealth;
    public int startingHealth;
    [Multiline]
    public string towerDescription;
    
	private Tower parent;

    public void Initialize(Tower tower)
    {
        parent = tower;

    }
}