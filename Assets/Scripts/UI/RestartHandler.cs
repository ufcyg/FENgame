using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartHandler : MonoBehaviour
{
    public RoundManager roundManager;

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
	
    public void RestartGame()
    {
        SceneManager.LoadScene("FENGameMain");
    }

    public void BackToGame()
    {
        roundManager.gameCanBePaused = true;
        roundManager.PauseGame();
        this.gameObject.SetActive(false);
    }

}