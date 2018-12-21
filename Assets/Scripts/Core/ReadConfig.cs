using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ReadConfig : MonoBehaviour
{
    public TextAsset csv;
    [Header("Other Gameobject References")]
    public ScoreCalculation scoreCalc;
    public AccountDisplayController accountDisplayContr;

    private string csvFile;

    private string[] records;

    //seperators
    private char lineSeperater = '\n'; // defines line seperation character
    private char fieldSeperator = ';'; // defines field seperation character
    private char feedbackSeperator = '|'; // defines item seperation character

    public void ReadData()
    {
        //if (false)
        //{
        //    records = csv.text.Split(lineSeperater);
        //}
        //else
        //{
            string configPath = Application.dataPath + "/Resources/penergeticon-data.csv";

            var configReader = new StreamReader(configPath, System.Text.Encoding.UTF7);
            csvFile = configReader.ReadToEnd();

            records = csvFile.Split(lineSeperater);
        //}

        foreach (string record in records)
        {
            string[] fields = record.Split(fieldSeperator);

            if (fields[0] == "Data-Installation")
            {
                for (int i = 1; i < 6; i++)
                {
                    fields = records[i].Split(fieldSeperator);
                    DataInstallation(fields);
                }
                
            }

            if (fields[0] == "Data-Connection")
            {
                for (int i = 7; i < 12; i++)
                {
                    fields = records[i].Split(fieldSeperator);
                    DataConnection(fields);
                }
            }

            if (fields[0] == "Data-Threshold")
            {
                for (int i = 13; i < 16; i++)
                {
                    fields = records[i].Split(fieldSeperator);
                    DataThreshold(fields);
                }
                accountDisplayContr.Init();
            }

            if (fields[0] == "Data-Feedback")
            {
                for (int i = 17; i < 20; i++)
                {
                    fields = records[i].Split(fieldSeperator);
                    DataFeedback(fields);
                }
            }

            if (fields[0] == "scaling")
            {
                scoreCalc.scaling = float.Parse(fields[1]);
            }

            if (fields[0] == "PowerShortagePenalty")
            {
                float penalty = float.Parse(fields[1]) / 100;
                scoreCalc.powerShortagePenalty = penalty;
            }
        }
        //scoreCalc.GetComponent<ScoreCalculation>().DebugShowData();
    }

    private void DataInstallation(string[] _fields)
    {
        if (_fields[0] == "PV")
        {
            scoreCalc.pvInstSummer = float.Parse(_fields[1]);
            scoreCalc.pvInstWinter = float.Parse(_fields[2]);
            scoreCalc.pvInstNight = float.Parse(_fields[3]);
            scoreCalc.pvInstNet = float.Parse(_fields[4]);
            scoreCalc.pvInstEnvironment = float.Parse(_fields[5]);
        }

        if (_fields[0] == "Pinwheel")
        {
            scoreCalc.pinInstSummer = float.Parse(_fields[1]);
            scoreCalc.pinInstWinter = float.Parse(_fields[2]);
            scoreCalc.pinInstNight = float.Parse(_fields[3]);
            scoreCalc.pinInstNet = float.Parse(_fields[4]);
            scoreCalc.pinInstEnvironment = float.Parse(_fields[5]);
        }

        if (_fields[0] == "Storage")
        {
            scoreCalc.storageInstSummer = float.Parse(_fields[1]);
            scoreCalc.storageInstWinter = float.Parse(_fields[2]);
            scoreCalc.storageInstNight = float.Parse(_fields[3]);
            scoreCalc.storageInstNet = float.Parse(_fields[4]);
            scoreCalc.storageInstEnvironment = float.Parse(_fields[5]);
        }

        if (_fields[0] == "eCar")
        {
            scoreCalc.eCarInstSummer = float.Parse(_fields[1]);
            scoreCalc.eCarInstWinter = float.Parse(_fields[2]);
            scoreCalc.eCarInstNight = float.Parse(_fields[3]);
            scoreCalc.eCarInstNet = float.Parse(_fields[4]);
            scoreCalc.eCarInstEnvironment = float.Parse(_fields[5]);
        }

        if (_fields[0] == "Powerplant")
        {
            scoreCalc.ppInstSummer = float.Parse(_fields[1]);
            scoreCalc.ppInstWinter = float.Parse(_fields[2]);
            scoreCalc.ppInstNight = float.Parse(_fields[3]);
            scoreCalc.ppInstNet = float.Parse(_fields[4]);
            scoreCalc.ppInstEnvironment = float.Parse(_fields[5]);
        }
    }
    private void DataConnection(string[] _fields)
    {
        if (_fields[0] == "AC-Close")
        {
            scoreCalc.acCloseSummer = float.Parse(_fields[1]);
            scoreCalc.acCloseWinter = float.Parse(_fields[2]);
            scoreCalc.acCloseNight = float.Parse(_fields[3]);
            scoreCalc.acCloseNet = float.Parse(_fields[4]);
            scoreCalc.acCloseEnvironment = float.Parse(_fields[5]);
        }

        if (_fields[0] == "AC-Far")
        {
            scoreCalc.acFarSummer = float.Parse(_fields[1]);
            scoreCalc.acFarWinter = float.Parse(_fields[2]);
            scoreCalc.acFarNight = float.Parse(_fields[3]);
            scoreCalc.acFarNet = float.Parse(_fields[4]);
            scoreCalc.acFarEnvironment = float.Parse(_fields[5]);
        }

        if (_fields[0] == "DC-Close")
        {
            scoreCalc.dcCloseSummer = float.Parse(_fields[1]);
            scoreCalc.dcCloseWinter = float.Parse(_fields[2]);
            scoreCalc.dcCloseNight = float.Parse(_fields[3]);
            scoreCalc.dcCloseNet = float.Parse(_fields[4]);
            scoreCalc.dcCloseEnvironment = float.Parse(_fields[5]);
        }

        if (_fields[0] == "DC-Far")
        {
            scoreCalc.dcFarSummer = float.Parse(_fields[1]);
            scoreCalc.dcFarWinter = float.Parse(_fields[2]);
            scoreCalc.dcFarNight = float.Parse(_fields[3]);
            scoreCalc.dcFarNet = float.Parse(_fields[4]);
            scoreCalc.dcFarEnvironment = float.Parse(_fields[5]);
        }

        if (_fields[0] == "Powerplant")
        {
            scoreCalc.ppConSummer = float.Parse(_fields[1]);
            scoreCalc.ppConWinter = float.Parse(_fields[2]);
            scoreCalc.ppConNight = float.Parse(_fields[3]);
            scoreCalc.ppConNet = float.Parse(_fields[4]);
            scoreCalc.ppConEnvironment = float.Parse(_fields[5]);
        }
    }
    private void DataThreshold(string[] _fields)
    {
        if (_fields[0] == "red")
        {
            scoreCalc.redSummer = float.Parse(_fields[1]);
            accountDisplayContr.summerRed = float.Parse(_fields[1]);
            scoreCalc.redWinter = float.Parse(_fields[2]);
            accountDisplayContr.winterRed = float.Parse(_fields[2]);
            scoreCalc.redNight = float.Parse(_fields[3]);
            accountDisplayContr.nightRed = float.Parse(_fields[3]);
            scoreCalc.redNet = float.Parse(_fields[4]);
            accountDisplayContr.netRed = float.Parse(_fields[4]);
            scoreCalc.redEnvironment = float.Parse(_fields[5]);
            accountDisplayContr.environmentRed = float.Parse(_fields[5]);
            scoreCalc.redSupply = float.Parse(_fields[6]);
            accountDisplayContr.supplyRed = float.Parse(_fields[6]);
        }

        if (_fields[0] == "yellow")
        {
            scoreCalc.yellowSummer = float.Parse(_fields[1]);
            accountDisplayContr.summerYellow = float.Parse(_fields[1]);
            scoreCalc.yellowWinter = float.Parse(_fields[2]);
            accountDisplayContr.winterYellow = float.Parse(_fields[2]);
            scoreCalc.yellowNight = float.Parse(_fields[3]);
            accountDisplayContr.nightYellow = float.Parse(_fields[3]);
            scoreCalc.yellowNet = float.Parse(_fields[4]);
            accountDisplayContr.netYellow = float.Parse(_fields[4]);
            scoreCalc.yellowEnvironment = float.Parse(_fields[5]);
            accountDisplayContr.environmentYellow = float.Parse(_fields[5]);
            scoreCalc.yellowSupply = float.Parse(_fields[6]);
            accountDisplayContr.supplyYellow = float.Parse(_fields[6]);
        }

        if (_fields[0] == "green")
        {
            scoreCalc.greenSummer = float.Parse(_fields[1]);
            accountDisplayContr.summerGreen = float.Parse(_fields[1]);
            scoreCalc.greenWinter = float.Parse(_fields[2]);
            accountDisplayContr.winterGreen = float.Parse(_fields[2]);
            scoreCalc.greenNight = float.Parse(_fields[3]);
            accountDisplayContr.nightGreen = float.Parse(_fields[3]);
            scoreCalc.greenNet = float.Parse(_fields[4]);
            accountDisplayContr.netGreen = float.Parse(_fields[4]);
            scoreCalc.greenEnvironment = float.Parse(_fields[5]);
            accountDisplayContr.environmentGreen = float.Parse(_fields[5]);
            scoreCalc.greenSupply = float.Parse(_fields[6]);
            accountDisplayContr.supplyGreen = float.Parse(_fields[6]);
        }
    }

    private void DataFeedback(string[] _fields)
    {
        if (_fields[0] == "red")
        {
            scoreCalc.redFeedbackSummer = _fields[1].Split(feedbackSeperator);
            scoreCalc.redFeedbackWinter = _fields[2].Split(feedbackSeperator);
            scoreCalc.redFeedbackNight = _fields[3].Split(feedbackSeperator);
            scoreCalc.redFeedbackNet = _fields[4].Split(feedbackSeperator);
            scoreCalc.redFeedbackEnvironment = _fields[5].Split(feedbackSeperator);
            scoreCalc.redFeedbackSupply = _fields[6].Split(feedbackSeperator);
        }

        if (_fields[0] == "yellow")
        {
            scoreCalc.yellowFeedbackSummer = _fields[1].Split(feedbackSeperator);
            scoreCalc.yellowFeedbackWinter = _fields[2].Split(feedbackSeperator);
            scoreCalc.yellowFeedbackNight = _fields[3].Split(feedbackSeperator);
            scoreCalc.yellowFeedbackNet = _fields[4].Split(feedbackSeperator);
            scoreCalc.yellowFeedbackEnvironment = _fields[5].Split(feedbackSeperator);
            scoreCalc.yellowFeedbackSupply = _fields[6].Split(feedbackSeperator);
        }

        if (_fields[0] == "green")
        {
            scoreCalc.greenFeedbackSummer = _fields[1].Split(feedbackSeperator);
            scoreCalc.greenFeedbackWinter = _fields[2].Split(feedbackSeperator);
            scoreCalc.greenFeedbackNight = _fields[3].Split(feedbackSeperator);
            scoreCalc.greenFeedbackNet = _fields[4].Split(feedbackSeperator);
            scoreCalc.greenFeedbackEnvironment = _fields[5].Split(feedbackSeperator);
            scoreCalc.greenFeedbackSupply = _fields[6].Split(feedbackSeperator);
        }
    }
}