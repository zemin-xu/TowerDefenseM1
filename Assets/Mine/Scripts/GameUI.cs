using System;
using UnityEngine;
using TMPro;
using Core.Utilities;

public enum InteractiveState { Default, Building, NonInteractive, GameFinished };
public class GameUI : Singleton<GameUI>
{
    private LevelManager levelManager;

    public InteractiveState state;

    public bool isGameOver;
    public bool isGameWin;

    public event Action gameover;
    public event Action gameWin;
    public event Action towerPurchased;
    public event Action<int> homeDamaged;
    public event Action<Tower> selectedTower;

    public TMP_Text moneyText;
    public TMP_Text lifeText;
    public GameObject winUI;
    public GameObject gameoverUI;
    public GameObject optionUI;
    public GameObject towerOptionUI;
    public GameObject towerInfoUI;

    [HideInInspector]
    public Tower currentBuildingTower; // the ghost chosen to be build

    protected override void Awake()
    {
        base.Awake();
        isGameOver = false;
        isGameWin = false;
    }

    private void Start()
    {
        state = InteractiveState.Default;
        levelManager = LevelManager.instance;

        // Activate UI.
        gameoverUI.SetActive(false);
        winUI.SetActive(false);
        optionUI.SetActive(false);
        towerOptionUI.SetActive(false);
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
        //DetectSelectingObject();

        // When player press down ESCAPE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (state)
            {
                case InteractiveState.NonInteractive:
                    ReturnGame();
                    break;

                case InteractiveState.Default:
                    {
                        if (towerOptionUI.activeSelf)
                        {
                            DeselectTower();
                        }
                        else
                        {
                            OnOptionButtonClick();
                        }
                    }
                    break;

                case InteractiveState.Building:
                    CancelBuild();
                    break;
            }
        }

        // When we have clicked tower button and is choosing its place.
        if (state == InteractiveState.Building)
        {
            if (currentBuildingTower != null)
            {
                //currentBuildingTower.transform.position = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(
                    Input.mousePosition.x, Input.mousePosition.y, 100.0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 pos = hit.point;
                    pos.y = hit.point.y + 2;
                    currentBuildingTower.gameObject.transform.position = hit.point;
                    currentBuildingTower.gameObject.transform.position = pos;
                }
            }
        }

        DetectSelectingObject();
    }


    private void DetectSelectingObject()
    {
        if (Input.GetMouseButtonDown(0) && state == InteractiveState.Default)
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit && hitInfo.transform.parent && hitInfo.transform.parent.parent)
            {
                Tower t = hitInfo.transform.parent.parent.GetComponent<Tower>();
                SelectTower(t);
                return;
            }
            DeselectTower();
        }
    }

    private void SelectTower(Tower t)
    {
        if (!t.hasBuilded)
        {
            t.hasBuilded = true;
            return ;
        }
        towerOptionUI.SetActive(true);
        if (selectedTower != null)
        {
            selectedTower(t);
        }

    }

    public void DeselectTower()
    {
        towerOptionUI.SetActive(false);
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
        towerInfoUI.SetActive(false);
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
        
        if (currentBuildingTower != null)
        {
            state = InteractiveState.Default;
            currentBuildingTower = null;
            towerInfoUI.SetActive(false);
            towerOptionUI.SetActive(false);
        }
    }

    // Functions parametered in Editor.
    public void OnOptionButtonClick()
    {
        Pause();
        optionUI.SetActive(true);
    }

    public void OnTowerButtonClicked(Tower t)
    {
        towerInfoUI.SetActive(true);
        towerInfoUI.GetComponent<TowerInfoUI>().UpdateTowerInfo(t);
    }





}