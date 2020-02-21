using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.Utilities;

public enum InteractiveState { Default, Building, NonInteractive, GameFinished };
public class GameUI : Singleton<GameUI>
{

    [HideInInspector]
    public InteractiveState state;

    public bool isGameOver;
    public bool isGameWin;

    public event Action gameover;
    public event Action gameWin;

    private LevelManager levelManager;

    protected override void Awake()
    {
        base.Awake();
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
    public GameObject towerInfoUI;

    [HideInInspector]
    public Tower currentBuildingTower;


    private void Start()
    {
        state = InteractiveState.Default;
        levelManager = LevelManager.instance;

        // Activate UI.
        gameoverUI.SetActive(false);
        winUI.SetActive(false);
        optionUI.SetActive(false);
        towerInfoUI.SetActive(false);

        // Subscribe event.
        levelManager.moneyUpdated += OnMoneyUpdated;
        levelManager.lifeUpdated += OnLifeUpdated;

        // Initialize text.
        moneyText.text = levelManager.money + "";
        lifeText.text = levelManager.life + "";

    }

    private void Update()
    {
        // When player press down ESCAPE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch(state)
            {
                case InteractiveState.NonInteractive:
                ReturnGame();
                break;

                case InteractiveState.Default:
                OnOptionButtonClick();
                break;

                case InteractiveState.Building:
                CancelBuild();
                break;
            }
        }

        if (state == InteractiveState.Building)
        {
            if (currentBuildingTower != null)
            {
                currentBuildingTower.placementSucceeded += OnBuildFinished;
                currentBuildingTower.placementFailed += OnBuildFinished;

                currentBuildingTower.transform.position = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(
                    Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    currentBuildingTower.transform.position = hit.point;
                }
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

    public void CancelBuild()
    {
        if (currentBuildingTower != null)
            Destroy(currentBuildingTower.gameObject);
        OnBuildFinished();
    }

    private void OnMoneyUpdated()
    {
        moneyText.text = levelManager.money.ToString();
    }

    // The life points to get off when get damaged.
    private void OnLifeUpdated()
    {
        lifeText.text = " " + levelManager.life.ToString();
    }

    private void OnBuildFinished()
    {
        state = InteractiveState.Default;
        currentBuildingTower = null;
    }

    // Functions parametered in Editor.
    public void OnOptionButtonClick()
    {
        Pause();
        optionUI.SetActive(true);
    }

    public void OnTowerButtonClicked()
    {
        
        towerInfoUI.SetActive(true);
    }

 

}