using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.Utilities;

public enum InteractiveState { Default, Building, NonInteractive, GameFinished };
public class GameUI : Singleton<GameUI>
{

    public InteractiveState state { get; private set; }

    public bool isGameOver;
    public bool isGameWin;

    public event Action gameover;
    public event Action gameWin;

    protected override void Awake()
    {
        isGameOver = false;
        isGameWin = false;
    }

    public event Action towerPurchased;
    public event Action<int> homeDamaged;
    public TMP_Text moneyText;
    public TMP_Text lifeText;
    public GameObject winUI;
    public GameObject gameoverUI;
    public GameObject optionUI;

    public Tower currentBuildingTower { get; private set; }


    private void Start()
    {
        state = InteractiveState.Default;

        gameoverUI.SetActive(false);
        winUI.SetActive(false);
        optionUI.SetActive(false);
    }

    private void Update()
    {
        // Open or close option window.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == InteractiveState.NonInteractive)
            {
                ReturnGame();
            }
            else if (state == InteractiveState.Default)
            {
                OnOptionButtonClick();
            }
        }

        if (state == InteractiveState.Building)
        {
            if (currentBuildingTower != null)
            {
                currentBuildingTower.transform.position = Input.mousePosition;

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(
                    Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    currentBuildingTower.transform.position = hit.point;
                    Debug.Log(currentBuildingTower.transform.position);
                }
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            }
        }
    }

    public void OnGameOver()
    {
        Pause();
        gameoverUI.SetActive(true);
    }

    public void OnGameWin()
    {
        Pause();
        winUI.SetActive(true);
    }

    public void OnTowerButtonClick(Tower tower)
    {
        if (tower == null)
        {
            Debug.LogWarning("tower prefab not existed");
            return;
        }
        state = InteractiveState.Building;

        GameObject go = Instantiate(tower.gameObject);
        currentBuildingTower = go.GetComponent<Tower>();
    }

    public void Pause()
    {
        state = InteractiveState.NonInteractive;
        Time.timeScale = 0;
    }

    public void ReturnGame()
    {
        state = InteractiveState.Default;
        Time.timeScale = 1;
    }

    public void OnOptionButtonClick()
    {
        Pause();
        optionUI.SetActive(true);

    }

    private void OnTowerPurchased(int money)
    {
        moneyText.text = " " + money.ToString();
    }

    // The life points to get off when get damaged.
    private void OnLifeUpdated(int life)
    {
        lifeText.text = " " + life.ToString();
    }


}