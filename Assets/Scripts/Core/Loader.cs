using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loader for FEN-SichereStromversorgung
/// </summary>
public class Loader : MonoBehaviour {

    [Header("Other Gameobject References")]
    public RoundManager roundManager;
    public WriteRoundOutcome logFileManager;
    public EnergyGrid grid;
    public GameObject openingPanel;
    public GameObject doeTown;
    public GameObject transmissionLines;
    public GameObject pauseButton;
    public GameObject replayButton;


    private int rounds = 19;

    void Start ()
    {
        roundManager.remainingRounds = rounds;
        logFileManager.GenerateLogFile();
        GetComponent<ReadConfig>().ReadData();
        UpdatePlacedSprites();
        if (GameObject.Find("CommandLineArgs") != null)
        {
            if (!GameObject.Find("CommandLineArgs").GetComponent<ReadCommandLineArguments>().enablePause)
            {
                pauseButton.SetActive(false);
            }
        }
    }



    private void UpdatePlacedSprites()
    {
        Transform reference = null;
        foreach (Transform child in grid.transform)
        {
            reference = child;
            foreach (GameObject gO in child.GetComponent<BuildingNode>().connectedBuildings)
            {
                child.GetComponent<BuildingNode>().SortSprite(gO.transform);
                if (gO.tag == "factory-smoke")
                {
                    gO.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sortingOrder = gO.GetComponent<SpriteRenderer>().sortingOrder;
                }
            }
        }
        foreach (Transform child in transmissionLines.transform)
        {
            reference.GetComponent<BuildingNode>().SortSprite(child.transform);
        }
    }

    public void StartGame()
    {
        Destroy(openingPanel);
        roundManager.enabled = true;
        roundManager.Init();
    }
}