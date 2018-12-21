using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour
{
    public SetPlayerSettings setPlayerSettings;

    public void StartRound()
    {
        SceneManager.LoadScene("FENGameMain");
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void CloseDialog()
    {
        GameObject.Find("CloseApp").SetActive(false);
    }

    public void ResetHighscore()
    {
        for (int i = 0; i < 20; i++)
        {
            PlayerPrefs.DeleteKey("hsPosVal" + i);
            PlayerPrefs.DeleteKey("hsPosName" + i);
        }
    }
}
