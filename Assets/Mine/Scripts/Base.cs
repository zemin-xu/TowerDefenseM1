using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to provide the Base some functions like being selected to place tower and so on.
public class Base : MonoBehaviour
{

    // The point where a tower will be instantiated.
    public Transform towerPoint;

    public bool isOccupied;

    public Material hoverMat;
    private Material defaultMat;
    private Renderer rend;
    private GameUI gameUI;
    void Start()
    {
        rend = transform.GetChild(1).GetComponent<Renderer>();
        defaultMat = rend.material;
        isOccupied = false;
        gameUI = GameUI.instance;
    }

    // When mouse is in Base object, its color will get changed.

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

    // The logic when click to confirm placing tower.
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
