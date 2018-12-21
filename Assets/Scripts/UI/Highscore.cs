using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    [Header("Status")]
    public bool levelComplete;

    [Header("HighscoreData")]
    public Text[] texts;

    [Header("Passed Data")]
    public int score;
    public string playerName;

    private int tempScore;
    private string tempName;
    private bool scoreAdded = false;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void ShiftDownHighscore(int shiftStart)
    {
        int tempScore2;
        string tempName2;
        
        for (int i = shiftStart; i < 20; i++)
        {
            tempScore2 = PlayerPrefs.GetInt("hsPosVal" + i);
            tempName2 = PlayerPrefs.GetString("hsPosName" + i);

            PlayerPrefs.SetInt("hsPosVal" + i, tempScore);
            PlayerPrefs.SetString("hsPosName" + i, tempName);

            if (tempScore != 0)
            {
                tempScore = tempScore2;
                tempName = tempName2;
            }
        }
    }

    public void OnLevelComplete()
    {
        int x = 0;
        for (int i = 0; i < 20; i++)
        {
            if (PlayerPrefs.GetInt("hsPosVal" + i) < score)
            {
                tempScore = PlayerPrefs.GetInt("hsPosVal" + i);
                tempName = PlayerPrefs.GetString("hsPosName" + i);

                PlayerPrefs.SetInt("hsPosVal" + i, score);
                PlayerPrefs.SetString("hsPosName" + i, playerName);

                scoreAdded = true;

                x = i;
            }
            
            if (scoreAdded)
                break;
        }
        if (scoreAdded && tempName != "")
            ShiftDownHighscore(x+1);

        int c = 0;
        foreach (Text text in texts)
        {
            text.text = c + 1 + ". " + PlayerPrefs.GetString("hsPosName" + c) + " - " + PlayerPrefs.GetInt("hsPosVal" + c);
            c++;
        }

        this.gameObject.SetActive(true);
    }
}