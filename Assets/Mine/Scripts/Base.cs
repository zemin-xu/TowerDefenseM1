using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    public Transform towerPoint;

    public bool isOccupied;

    public Material hoverMat;
    private Material defaultMat;

    private Renderer rend;
    private GameUI gameUI;
    // Start is called before the first frame update

    void Start()
    {
        rend = transform.GetChild(1).GetComponent<Renderer>();
        defaultMat = rend.material;
        isOccupied = false;
        gameUI = GameUI.instance;
    }

    private void OnMouseEnter()
    {
        if (gameUI.state == InteractiveState.Building)
        {
            if (!isOccupied)
            {
                rend.material = hoverMat;
            }
        }
    }

    private void OnMouseDown()
    {
        if (gameUI.state == InteractiveState.Building)
        {
            Tower tower;
            if ((tower = gameUI.currentBuildingTower) == null)
            {
                Debug.LogWarning("You should choose the tower icon to indicate what to build.");
                return;
            }
            tower.TryConfirmPlacement(this);

        }

    }

    private void OnMouseExit()
    {
        rend.material = defaultMat;
    }

}
