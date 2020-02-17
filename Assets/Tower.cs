using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionGameFramework.Health;
using Core.Utilities;

public class Tower : Targetable 
{
    public TowerLevel[] levels;

		public string towerName;
        public string towerDescription;

        public int purchaseCost
		{
			get { return levels[0].cost; }
		}
}
