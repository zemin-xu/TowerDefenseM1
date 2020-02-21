using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    private Vector3 towerPos;

    public bool isOccupied;

    public Material hoverMat;
    private Material defaultMat;

    private Renderer rend;
    private GameUI gameUI;
    // Start is called before the first frame update

    void Start()
    {
        towerPos = transform.GetChild(0).transform.position;
        rend = transform.GetChild(1).GetComponent<Renderer>();
        defaultMat= rend.material;
        isOccupied = false;
        gameUI = FindObjectOfType<GameUI>();
    }

    private void OnMouseEnter()
    {
        if (gameUI.state == InteractiveState.Building)
        {
            if (!isOccupied)
            {
                rend.material= hoverMat;
            }
        }
    }

    private void OnMouseDown()
    {
        Tower tower;
        if ((tower = gameUI.currentBuildingTower) == null)
        {
            Debug.LogWarning("You should choose the tower icon to indicate what to build.");
            return;
        }
        TryConfirmTower(tower);
        
    }

    private void OnMouseExit()
    {
        rend.material = defaultMat;
    }

    public bool TryConfirmTower(Tower tower)
    {
        gameUI.OnBuildFinished();
        if (!isOccupied)
        {
            tower.OnConfirmedTower(towerPos);
            isOccupied = true;
            return (true);
        }
        else
        {
            Debug.LogWarning("this place is occupied");
        }
        return (false);

    }

 
}
