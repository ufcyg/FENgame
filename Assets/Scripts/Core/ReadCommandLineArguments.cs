using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadCommandLineArguments : MonoBehaviour {

    public Text args;

    public bool enablePause = true;

    private void Start()
    {
        DontDestroyOnLoad(this);
        string[] commandLineArguments = System.Environment.GetCommandLineArgs();
      
        foreach (string arg in commandLineArguments)
        {
            if (arg == "-np")
            {
                enablePause = false;
            }
        }
    }
}