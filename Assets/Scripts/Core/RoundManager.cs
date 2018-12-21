using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [Header("References")]
    public GameObject restartHandlePanel;
    public GameObject pausePanel;
    public GameObject closestNode;
    public EnergyGrid grid;
    public InputField userNameInputField;
    public Highscore highScore;
    public GameObject remainingPPUI;
    public Slider slider;
    public Text scoreText;
    public CreateText messageSystem;
    public ScoreCalculation scoreCalculator;
    [Space(10)]
    public BuildingDragNDrop pv;
    public BuildingDragNDrop pin;
    public BuildingDragNDrop storage;
    public BuildingDragNDrop eCar;

    [Header("Game Status")]
    public float roundDuration;
    public bool gamePaused = false;
    public bool skipUpdate = false;
    public bool gameCanBePaused = false;
    public bool buildingPlaced;
    public int remainingRounds;
    public string installation;

    [Header("Pause Button")]
    public Image pauseButtonImageScript;
    public Sprite pauseButton;
    public Sprite playButton;

    private bool gameEnd = false;
    private bool dispHighscore = false;
    private int score;
    private int maxRounds;

    [Header("TouchScreenKeyboard")]
    public Text nameInputFieldText;
    public KeyboardHandler keyboard;

    /// <summary>
    /// Round manager "Start()" Function. Sets maxValue of the slider and the count of remaining rounds 
    /// accordingly to the setup choosen in Unity worldspace.
    /// </summary>
    public void Init()
    {
        gameCanBePaused = true;
        slider.maxValue = roundDuration;
        slider.value = slider.maxValue;
        buildingPlaced = false;
        maxRounds = remainingRounds;
    }

    private void Update()
    {
        if (!skipUpdate)
        {
            if (!gamePaused)
            {
                if (!gameEnd)
                {
                    slider.value -= Time.deltaTime;
                }
                
                if (slider.value <= 0 || buildingPlaced == true) //end of round
                {
                    slider.value = roundDuration; // reset round timer
                    buildingPlaced = false; // reset if a building was placed
                    remainingPPUI.GetComponent<RemainingPP>().RemovePPpikto(); // kill regular power plant
                    messageSystem.QueueMessage("Runde: " + (maxRounds - remainingRounds + 1));
                    scoreCalculator.writeLog.AddMessageToLog("Runde: " + (maxRounds - remainingRounds + 1));

                    scoreCalculator.round = remainingRounds;
                    score = Mathf.CeilToInt(scoreCalculator.CalcScore(installation, closestNode));
                    scoreText.text = score.ToString();

                    installation = ""; // reset placed building
                    closestNode = null; // reset node on grid
                    remainingRounds--; //decrement round number

                    if (remainingRounds == 0)
                    {
                        messageSystem.QueueMessage("Letzte Runde!");
                        scoreCalculator.writeLog.AddMessageToLog("Letzte Runde!");
                    }

                    scoreCalculator.writeLog.WriteQueuedMessagesToLog();

                    if (!messageSystem.qCoRunning)
                    {
                        StartCoroutine(messageSystem.ShowQueuedMessages());
                    }
                    else
                    {
                        StartCoroutine(DelayNewMessagesCo());
                    }

                    if (remainingRounds < 0) // stop game after last round
                    {
                        StopAllCoroutines();
                        pauseButtonImageScript.gameObject.GetComponent<Button>().enabled = false;
                        gameEnd = true;
                        CrucialSystemsToggle(false);
                    }
                }

                if (gameEnd && !dispHighscore) // show highscore
                {
                    dispHighscore = true;
                    skipUpdate = true;

                    float y = Screen.height / 2;
                    float x = Screen.width / 2;

                    userNameInputField.transform.position = new Vector3(x, y, 0);
                    keyboard.ToggleKeyboard(true);
                    gameCanBePaused = false;
                }
            }
        }
    }
    
    IEnumerator DelayNewMessagesCo()
    {
        while (messageSystem.qCoRunning)
        {
            yield return null;
        }
        StartCoroutine(messageSystem.ShowQueuedMessages());
        yield return null;
    }

    public void CrucialSystemsToggle(bool status)
    {
        if (status == false)
        {
            pv.transform.position = pv.startPosition;
            pv.RemoveHighlight();
            pin.transform.position = pin.startPosition;
            pin.RemoveHighlight();
            storage.transform.position = storage.startPosition;
            storage.RemoveHighlight();
            eCar.transform.position = eCar.startPosition;
            eCar.RemoveHighlight();
        }
        pv.enabled = status;
        pin.enabled = status;
        storage.enabled = status;
        eCar.enabled = status;
    }

    private void UserNameInput()
    {
        highScore.score = score;
        highScore.playerName = userNameInputField.text;
        scoreCalculator.writeLog.RenameLogFile(highScore.playerName);
        highScore.OnLevelComplete();
    }

    public void UserNameInputButtonOK()
    {
        userNameInputField.transform.position = new Vector3(0, -9999, 0);
        UserNameInput();
        keyboard.ToggleKeyboard(false);
    }

    public void ReloadLevel()
    {
        if (gameCanBePaused)
        {
            if (gamePaused == false)
            {
                PauseGame();
            }
            restartHandlePanel.SetActive(true);
            gameCanBePaused = false;
        }
    }

    public void PauseGame()
    {
        if (gameCanBePaused)
        {
            if (gamePaused == false)
            {
                gamePaused = true;
                pauseButtonImageScript.sprite = playButton;
                messageSystem.isPaused = true;
                pausePanel.SetActive(true);
                CrucialSystemsToggle(false);
            }
            else
            {
                gamePaused = false;
                pauseButtonImageScript.sprite = pauseButton;
                messageSystem.isPaused = false;
                pausePanel.SetActive(false);
                CrucialSystemsToggle(true);
            }
        }
    }
}