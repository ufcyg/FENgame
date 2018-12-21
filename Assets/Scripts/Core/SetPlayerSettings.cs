using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerSettings : MonoBehaviour
{
    public GameObject music;
    public Text adjustscriptWidth;
    public Text adjustscriptHeight;

    public Text usedRes;
    public GameObject resolutionPanel;

    string versionNumber = "1.0";

    void Start()
    {
        SetupGame();
    }

    public void SetupGame()
    {
        QualitySettings.SetQualityLevel(5);
        if (PlayerPrefs.HasKey("Version") && PlayerPrefs.HasKey("LastResolution"))
        {
            string currentRes = (Screen.currentResolution.width.ToString() + ", " + Screen.currentResolution.height.ToString());
            if (PlayerPrefs.GetString("ResolutionAdjusted") == "no" || PlayerPrefs.GetString("LastResolution") != currentRes || PlayerPrefs.GetString("Version") != versionNumber)
            {
                SetResolution(640, 480, false);
            }
            else
            {
                usedRes.text = "Used Resolution: " + PlayerPrefs.GetString("LastUsedResolution");
            }
        }
        else
        {
            if (!PlayerPrefs.HasKey("LastResolution"))
            {
                PlayerPrefs.SetString("LastResolution", "");
            }
            PlayerPrefs.SetString("Version", versionNumber);
            PlayerPrefs.SetString("ResolutionAdjusted", "no");
            SetupGame();
        }
    }

    void AdjustResolution()
    {
        Resolution currentScreenRes = Screen.currentResolution;
        PlayerPrefs.SetString("LastResolution", currentScreenRes.width.ToString() + ", " + currentScreenRes.height.ToString());
        int currentWidth = currentScreenRes.width;
        int currentHeight = currentScreenRes.height;
        adjustscriptWidth.text = "AdjustRes Script width: " + currentScreenRes.width.ToString();
        adjustscriptHeight.text = "AdjustRes Script height: " + currentScreenRes.height.ToString();

        int refreshR = 0;
        int width = 0;
        int height = 0;
        Resolution[] resolutions = Screen.resolutions;
        foreach (Resolution res in resolutions)
        {
            float _width = (float)res.width;
            float _height = (float)res.height;
            if (_width > currentWidth || _height > currentHeight)
            {
                continue;
            }
            if ((_width / _height) > 1.76f && (_width / _height) < 1.8f)
            {
                width = res.width;
                height = res.height;
                refreshR = res.refreshRate;
            }
        }
        if (width == 0 || height == 0)
        {
            music.SetActive(false);
            PlayerPrefs.SetString("ResolutionAdjusted", "no");
            Screen.SetResolution(640, 480, true);
            resolutionPanel.SetActive(true);
            return;
        }
        resolutionPanel.SetActive(false);
        Screen.SetResolution(width, height, true, refreshR);
        usedRes.text = "Used Resolution: " + width + ", " + height + " @" + refreshR;
        PlayerPrefs.SetString("LastUsedResolution", usedRes.text);
        PlayerPrefs.SetString("ResolutionAdjusted", "yes");
    }

    private void SetResolution(int width, int height, bool fullscreen)
    {
        StartCoroutine(ChangeResolution(width, height, fullscreen));
    }

    IEnumerator ChangeResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
        
        yield return new WaitForSeconds(.1f);
        AdjustResolution();
    }

    public void ErrorQuit()
    {
        Application.Quit();
    }
}