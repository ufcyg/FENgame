using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class WriteRoundOutcome : MonoBehaviour {

    [Header("File Path")]
    public string path = "";

    [Header("Message Log")]
    public LinkedList<string> messageLog = new LinkedList<string>();

    StreamWriter sw = null;
    public void GenerateLogFile()
    {
        string currentTime = FormatDateTime(System.DateTime.Now.ToString());
        path = Application.persistentDataPath + "/" + currentTime + ".txt";
        
        using (sw = new StreamWriter(path))
        {
            sw.WriteLine("FEN-Spiel - Sichere die Stromversorgung GameLog - " + PlayerPrefs.GetFloat("Version"));
        }
    }

    public void AddDataToLogFile(string data)
    {
        using (sw = File.AppendText(path))
        {
            sw.WriteLine(data);
        }
    }

    public void RenameLogFile(string playerName)
    {
        string newPath = "";

        for (int i = 0; i<path.Length-4; i++)
        {
            newPath += path[i];
        }

        newPath += '_';
        newPath += playerName;
        newPath += ".txt";

        System.IO.File.Move(path, newPath);
    }

    public void AddMessageToLog(string logMessage)
    {
        messageLog.AddLast(logMessage);
    }

    public void WriteQueuedMessagesToLog()
    {
        AddDataToLogFile("");
        AddDataToLogFile("Status Messages:");

        while (messageLog.Count > 0)
        {
            AddDataToLogFile(messageLog.First.Value);
            messageLog.RemoveFirst();
        }
    }

    private string FormatDateTime(string _currentTime)
    {
        string path = "";

        for (int i = 0; i < _currentTime.Length; i++)
        {
            if (_currentTime[i] == '/')
            {
                path += '-';
            }
            else if (_currentTime[i] == ' ')
            {
                path += '_';
            }
            else if (_currentTime[i] == ':')
            {
                path += '.';
            }
            else
            {
                path += _currentTime[i];
            }
        }

        return path;
    }
}