using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    private Vector3 towerPos;
    private Color defaultColor;
    private Light lighting;

    [HideInInspector]
    public bool isOccupied;

    public Color hoverColor;

    private Transform platform;
    private Renderer rend;
    // Start is called before the first frame update

    void Start()
    {
        towerPos = transform.GetChild(0).transform.position;
        rend = transform.GetChild(1).GetComponent<Renderer>();
        defaultColor = rend.material.color;
        lighting = transform.GetComponentInChildren<Light>();
        isOccupied = false;
    }

    private void OnMouseEnter()
    {
        if (!isOccupied)
        {
            /*
            if (GameUI.Instance.inputState == InputState.ChoosingBase)
            {
                rend.material.color = hoverColor;
            }
            */
        }
    }

    private void OnMouseDown()
    {
        Tower tower;
        if ((tower = GameUI.instance.currentBuildingTower) == null)
        {
            Debug.LogWarning("You should choose the tower icon to indicate what to build.");
            return;
        }
        TryInstantiateTower(tower.gameObject);
    }

    private void OnMouseExit()
    {
        rend.material.color = defaultColor;
    }

    public bool TryInstantiateTower(GameObject tower)
    {
        if (!isOccupied)
        {
            /*
            if (GameUI.instance.TryPurchaseTower(tower.GetComponent<Tower>()))
            {
                Instantiate(tower, towerPos, Quaternion.identity);
                isOccupied = true;
                OnBuildFinished();
                lighting.transform.position = new Vector3(lighting.transform.position.x, lighting.transform.position.y + 10, lighting.transform.position.z);
                return (true);
            }
            */
        }
        Debug.LogWarning("this place is occupied");
        return (false);
    }

    private void OnBuildFinished()
    {
        //GameUI.Instance.inputState = InputState.Default;
        //GameUI.instance.currTowerPrefab = null;
    }
}
