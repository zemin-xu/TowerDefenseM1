using System;
using UnityEngine;
using TMPro;
using Core.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Several state of game.
// Default: not choosing base to build tower.
// Building : is choosing base to build tower.
// NonInteractive : when in pause menu.
// GameFinished : when game ends.
public enum InteractiveState { Default, Building, NonInteractive, GameFinished };
public class GameUI : Singleton<GameUI>
{
    private LevelManager levelManager;
    private WaveManager waveManager;
    public InteractiveState state;

    // When a builded tower is selected.
    public event Action<Tower> selectedTower;

    public TMP_Text moneyText;
    public TMP_Text lifeText;

    public TMP_Text progressText;
    public GameObject winUI;
    public GameObject gameoverUI;
    public GameObject optionUI;
    public GameObject towerOptionUI;
    public GameObject towerInfoUI;

    public GameObject waveProgressUI;
    // The slider of wave progress.
    public Slider slider; 

    [HideInInspector]
    public Tower currentBuildingTower; // the ghost tower chosen to be build

    // When the start wave event is triggered.
    public event Action startWaveActivated;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        state = InteractiveState.Default;
        levelManager = LevelManager.instance;
        waveManager = WaveManager.instance;

        // Activate UI.
        gameoverUI.SetActive(false);
        winUI.SetActive(false);
        optionUI.SetActive(false);
        towerOptionUI.SetActive(false);
        towerInfoUI.SetActive(false);
        waveProgressUI.SetActive(false);

        // Subscribe event.
        levelManager.moneyUpdated += OnMoneyUpdated;
        levelManager.lifeUpdated += OnLifeUpdated;

        levelManager.gameWin += OnGameWin;
        levelManager.gameover += OnGameOver;

        // Initialize info text.
        moneyText.text = levelManager.money + "";
        lifeText.text = levelManager.life + "";

    }

    private void Update()
    {
        // When player press down ESCAPE
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I))
        {
            switch (state)
            {
                // in Pause
                case InteractiveState.NonInteractive:
                    ReturnGame();
                    break;

                
                case InteractiveState.Default:
                    {
                        // if the option ui is displayed.
                        if (towerOptionUI.activeSelf)
                        {
                            DeselectTower();
                        }
                        else
                        {
                            if (optionUI.activeSelf)
                            {
                                ReturnGame();
                            }
                            else
                            {
                                OnOptionButtonClick();
                            }
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
                // display the ghost a bit higher above the mouse position.
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

        // update slider info.
        if (slider != null)
        {
            slider.value = waveManager.waveProgress; 
            progressText.text = "WAVE " + (waveManager.waveNumber - 1 ) + " / " + waveManager.totalWaves;
        }

        DetectSelectingObject();
    }


    // detecting if player is clicking mouse to choose tower or deselect tower.
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

    // if select an existing tower, an tower option ui will be displayed.
    private void SelectTower(Tower t)
    {
        if (t != null)
        {
            if (!t.hasBuilded)
            {
                t.hasBuilded = true;
                return;
            }
            towerOptionUI.SetActive(true);
            if (selectedTower != null)
            {
                selectedTower(t);
            }
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
        if (optionUI.activeSelf)
        {
            optionUI.SetActive(false);
        }
        Time.timeScale = 1;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    // cancel building current selecting tower.
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

    /* The following Functions will be parametered in Editor. */
    public void OnOptionButtonClick()
    {
        if (optionUI.activeSelf)
        {
            ReturnGame();
        }
        else
        {
            Pause();
            optionUI.SetActive(true);
        }
    }

    public void OnActivateStartWaveButtonPressed()
    {
        if (startWaveActivated != null)
        {
            startWaveActivated();
        }
        waveProgressUI.SetActive(true);
    }

    public void OnTowerButtonClicked(Tower t)
    {
        towerInfoUI.SetActive(true);
        towerInfoUI.GetComponent<TowerInfoUI>().UpdateTowerInfo(t);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    // For debug use.
   public void AddMoney()
    {
       levelManager.UpdateMoney(100);
    }
}