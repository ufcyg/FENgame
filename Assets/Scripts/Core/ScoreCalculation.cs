using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GetComponent<WriteRoundOutcome>().AddDataToLogFile();

public class ScoreCalculation : MonoBehaviour
{
    [Header("References")]
    public WriteRoundOutcome writeLog;
    public AccountDisplayController accountDisplayContr;
    public EnergyGrid grid;
    public CreateText statusMessages;

    [Header("Current Round")]
    public int round = 0;

    //Accounts
    private float summer = 100;
    private float winter = 100;
    private float night = 100;
    private float net = 100;
    private float environment = 0;
    private float supply = 100;

    [Header("Installation Values")]
    [Space(10)]//PV
    public float pvInstSummer;
    public float pvInstWinter;
    public float pvInstNight;
    public float pvInstNet;
    public float pvInstEnvironment;
    [Space(10)]//Pinwheel
    public float pinInstSummer;
    public float pinInstWinter;
    public float pinInstNight;
    public float pinInstNet;
    public float pinInstEnvironment;
    [Space(10)]//Storage
    public float storageInstSummer;
    public float storageInstWinter;
    public float storageInstNight;
    public float storageInstNet;
    public float storageInstEnvironment;
    [Space(10)]//eCar
    public float eCarInstSummer;
    public float eCarInstWinter;
    public float eCarInstNight;
    public float eCarInstNet;
    public float eCarInstEnvironment;
    [Space(10)]//Powerplant
    public float ppInstSummer;
    public float ppInstWinter;
    public float ppInstNight;
    public float ppInstNet;
    public float ppInstEnvironment;

    [Header("Connection Type")]
    //AC Close
    public float acCloseSummer;
    public float acCloseWinter;
    public float acCloseNight;
    public float acCloseNet;
    public float acCloseEnvironment;
    [Space(10)]//AC Far
    public float acFarSummer;
    public float acFarWinter;
    public float acFarNight;
    public float acFarNet;
    public float acFarEnvironment;
    [Space(10)]//DC Close
    public float dcCloseSummer;
    public float dcCloseWinter;
    public float dcCloseNight;
    public float dcCloseNet;
    public float dcCloseEnvironment;
    [Space(10)]//DC Far;
    public float dcFarSummer;
    public float dcFarWinter;
    public float dcFarNight;
    public float dcFarNet;
    public float dcFarEnvironment;
    [Space(10)]//Powerplant
    public float ppConSummer;
    public float ppConWinter;
    public float ppConNight;
    public float ppConNet;
    public float ppConEnvironment;

    /* Deprecated
    //Data Threshold
    bool enteredSummerGreen = true;
    bool enteredSummerYellow = false;
    bool enteredSummerRed = false;

    bool enteredWinterGreen = true;
    bool enteredWinterYellow = false;
    bool enteredWinterRed = false;

    bool enteredNightGreen = true;
    bool enteredNightYellow = false;
    bool enteredNightRed = false;

    bool enteredNetGreen = true;
    bool enteredNetYellow = false;
    bool enteredNetRed = false;

    bool enteredEnvironmentGreen = true;
    bool enteredEnvironmentYellow = false;
    bool enteredEnvironmentRed = false;

    bool enteredSupplyGreen = true;
    bool enteredSupplyYellow = false;
    bool enteredSupplyRed = false;
    */

    [Header("Thresholds")]
    //summer
    public float redSummer;
    public float yellowSummer;
    public float greenSummer;
    [Space(10)]//winter
    public float redWinter;
    public float yellowWinter;
    public float greenWinter;
    [Space(10)]//night
    public float redNight;
    public float yellowNight;
    public float greenNight;
    [Space(10)]//netStab
    public float redNet;
    public float yellowNet;
    public float greenNet;
    [Space(10)]//environment
    public float redEnvironment;
    public float yellowEnvironment;
    public float greenEnvironment;
    [Space(10)]//supply
    public float redSupply;
    public float yellowSupply;
    public float greenSupply;

    //Data Feedback
    bool powerShortage = false;
    [Header("Feedback Messages")]
    //summer
    public string[] redFeedbackSummer;
    public string[] yellowFeedbackSummer;
    public string[] greenFeedbackSummer;
    [Space(10)]//winter
    public string[] redFeedbackWinter;
    public string[] yellowFeedbackWinter;
    public string[] greenFeedbackWinter;
    [Space(10)]//night
    public string[] redFeedbackNight;
    public string[] yellowFeedbackNight;
    public string[] greenFeedbackNight;
    [Space(10)]//netStab
    public string[] redFeedbackNet;
    public string[] yellowFeedbackNet;
    public string[] greenFeedbackNet;
    [Space(10)]//environment
    public string[] redFeedbackEnvironment;
    public string[] yellowFeedbackEnvironment;
    public string[] greenFeedbackEnvironment;
    [Space(10)]//supply
    public string[] redFeedbackSupply;
    public string[] yellowFeedbackSupply;
    public string[] greenFeedbackSupply;

    [Header("Scaling Factor")]
    public float scaling;

    [Header("Powershortage Penatly Factor")]
    public float powerShortagePenalty;

    //score
    float lastScoreBeforeScaling = 0;
    float scoreAfterScaling;

    public void DebugShowData()
    {
        Debug.Log("Data Installation");
        Debug.Log("PV|| Summer: " + pvInstSummer + " Winter: " + pvInstWinter + " Night: " + pvInstNight + " Net: " + pvInstNet + " Environment: " + pvInstEnvironment);
        Debug.Log("Pinwheel|| Summer: " + pinInstSummer + " Winter: " + pinInstWinter + " Night: " + pinInstNight + " Net: " + pinInstNet + " Environment: " + pinInstEnvironment);
        Debug.Log("Storage|| Summer: " + storageInstSummer + " Winter: " + storageInstWinter + " Night: " + storageInstNight + " Net: " + storageInstNet + " Environment: " + storageInstEnvironment);
        Debug.Log("eCar|| Summer: " + eCarInstSummer + " Winter: " + eCarInstWinter + " Night: " + eCarInstNight + " Net: " + eCarInstNet + " Environment: " + eCarInstEnvironment);
        Debug.Log("Powerplant|| Summer: " + ppInstSummer + " Winter: " + ppInstWinter + " Night: " + ppInstNight + " Net: " + ppInstNet + " Environment: " + ppInstEnvironment);

        Debug.Log("Data Connection");
        Debug.Log("AC Close|| Summer: " + acCloseSummer + " Winter: " + acCloseWinter + " Night: " + acCloseNight + " Net: " + acCloseNet + " Environment: " + acCloseEnvironment);
        Debug.Log("AC Far|| Summer: " + acFarSummer + " Winter: " + acFarWinter + " Night: " + acFarNight + " Net: " + acFarNet + " Environment: " + acFarEnvironment);
        Debug.Log("DC Close|| Summer: " + dcCloseSummer + " Winter: " + dcCloseWinter + " Night: " + dcCloseNight + " Net: " + dcCloseNet + " Environment: " + dcCloseEnvironment);
        Debug.Log("AC Far|| Summer: " + dcFarSummer + " Winter: " + dcFarWinter + " Night: " + dcFarNight + " Net: " + dcFarNet + " Environment: " + dcFarEnvironment);
        Debug.Log("Powerplant|| Summer: " + ppConSummer + " Winter: " + ppConWinter + " Night: " + ppConNight + " Net: " + ppConNet + " Environment: " + ppConEnvironment);

        Debug.Log("Scalingfactor: " + scaling);
        Debug.Log("PowerShortagePenalty: " + powerShortagePenalty);
    }

    public float CalcScore(string installation, GameObject node)
    { 
        writeLog.AddDataToLogFile("");
        writeLog.AddDataToLogFile("Round: " + (19 - round + 1));
        writeLog.AddDataToLogFile("Player built installation: " + installation);
        
        if (node != null)
        {
            writeLog.AddDataToLogFile("Connection Type: " + node.GetComponent<BuildingNode>().powerType);
            if (node.GetComponent<BuildingNode>().onLand == true)
            {
                writeLog.AddDataToLogFile("Distance to grid: CLOSE");
            }
            else
            {
                writeLog.AddDataToLogFile("Distance to grid: FAR");
            }
        }
        else
        {
            writeLog.AddDataToLogFile("Connection Type: NONE");
            writeLog.AddDataToLogFile("Distance to grid: INF");
        }

        //scoreAfterScaling = ScoreAfterPPremoval();
        //GetComponent<WriteRoundOutcome>().AddDataToLogFile("");
        //GetComponent<WriteRoundOutcome>().AddDataToLogFile("Scores after powerplant removal");
        //GetComponent<WriteRoundOutcome>().AddDataToLogFile("-------------------------------");
        //WriteLogFile();
        //scoreAfterScaling = ScoreAfterUserInstallation(installation, node);
        //GetComponent<WriteRoundOutcome>().AddDataToLogFile("");
        //GetComponent<WriteRoundOutcome>().AddDataToLogFile("Score after user installation");
        //GetComponent<WriteRoundOutcome>().AddDataToLogFile("-----------------------------");
        //WriteLogFile();

        scoreAfterScaling = ScoreCalc(installation, node);

        writeLog.AddDataToLogFile("");
        writeLog.AddDataToLogFile("Account Values and Score");
        writeLog.AddDataToLogFile("------------------------");

        WriteLogFile();

        accountDisplayContr.SetSliderValues(summer, winter, night, net, supply, environment);
        return scoreAfterScaling;
    }

    private float ScoreCalc(string installation, GameObject node)
    {
        float score = lastScoreBeforeScaling;

        //Remove PP
        float installationVal;
        float connectionType;

        //summer
        installationVal = ppInstSummer;
        connectionType = ppConSummer;
        summer = CalcSummer(installationVal, connectionType);
        //winter
        installationVal = ppInstWinter;
        connectionType = ppConWinter;
        winter = CalcWinter(installationVal, connectionType);
        //night
        installationVal = ppInstNight;
        connectionType = ppConNight;
        night = CalcNight(installationVal, connectionType);
        //net
        installationVal = ppInstNet;
        connectionType = ppConNet;
        net = CalcNet(installationVal, connectionType);
        //environment
        installationVal = ppInstEnvironment;
        connectionType = ppConEnvironment;
        environment = CalcEnvironment(installationVal, connectionType);

        //User installation
        if (installation != "")
        {
            if (installation == "SolarPark-Img(Clone)")
            {
                //summer
                installationVal = pvInstSummer;
                connectionType = CalcConnectionValue(node, "Summer");
                summer = CalcSummer(installationVal, connectionType);
                //winter
                installationVal = pvInstWinter;
                connectionType = CalcConnectionValue(node, "Winter");
                winter = CalcWinter(installationVal, connectionType);
                //night
                installationVal = pvInstNight;
                connectionType = CalcConnectionValue(node, "Night");
                night = CalcNight(installationVal, connectionType);
                //net
                installationVal = pvInstNet;
                connectionType = CalcConnectionValue(node, "Net");
                net = CalcNet(installationVal, connectionType);
                //environment
                installationVal = pvInstEnvironment;
                connectionType = CalcConnectionValue(node, "Enviroment");
                environment = CalcEnvironment(installationVal, connectionType);
            }
            else if (installation == "Pinwheel-Img(Clone)")
            {
                //summer
                installationVal = pinInstSummer;
                connectionType = CalcConnectionValue(node, "Summer");
                summer = CalcSummer(installationVal, connectionType);
                //winter
                installationVal = pinInstWinter;
                connectionType = CalcConnectionValue(node, "Winter");
                winter = CalcWinter(installationVal, connectionType);
                //night
                installationVal = pinInstNight;
                connectionType = CalcConnectionValue(node, "Night");
                night = CalcNight(installationVal, connectionType);
                //net
                installationVal = pinInstNet;
                connectionType = CalcConnectionValue(node, "Net");
                net = CalcNet(installationVal, connectionType);
                //environment
                installationVal = pinInstEnvironment;
                connectionType = CalcConnectionValue(node, "Enviroment");
                environment = CalcEnvironment(installationVal, connectionType);
            }
            else if (installation == "Energystorage-Img(Clone)")
            {
                //summer
                installationVal = storageInstSummer;
                connectionType = CalcConnectionValue(node, "Summer");
                summer = CalcSummer(installationVal, connectionType);
                //winter
                installationVal = storageInstWinter;
                connectionType = CalcConnectionValue(node, "Winter");
                winter = CalcWinter(installationVal, connectionType);
                //night
                installationVal = storageInstNight;
                connectionType = CalcConnectionValue(node, "Night");
                night = CalcNight(installationVal, connectionType);
                //net
                installationVal = storageInstNet;
                connectionType = CalcConnectionValue(node, "Net");
                net = CalcNet(installationVal, connectionType);
                //environment
                installationVal = storageInstEnvironment;
                connectionType = CalcConnectionValue(node, "Enviroment");
                environment = CalcEnvironment(installationVal, connectionType);
            }
            else if (installation == "E-Car-Img(Clone)")
            {
                //summer
                installationVal = eCarInstSummer;
                connectionType = CalcConnectionValue(node, "Summer");
                summer = CalcSummer(installationVal, connectionType);
                //winter
                installationVal = eCarInstWinter;
                connectionType = CalcConnectionValue(node, "Winter");
                winter = CalcWinter(installationVal, connectionType);
                //night
                installationVal = eCarInstNight;
                connectionType = CalcConnectionValue(node, "Night");
                night = CalcNight(installationVal, connectionType);
                //net
                installationVal = eCarInstNet;
                connectionType = CalcConnectionValue(node, "Net");
                net = CalcNet(installationVal, connectionType);
                //environment
                installationVal = eCarInstEnvironment;
                connectionType = CalcConnectionValue(node, "Enviroment");
                environment = CalcEnvironment(installationVal, connectionType);
            }
        }
        else
        {
            supply = Mathf.Min(summer, winter, night);
            score = ApplyPenalty();
            InformUser();
            return score / scaling;
        }


        supply = Mathf.Min(summer, winter, night);
        lastScoreBeforeScaling = score + (Mathf.Min(supply, 100) + Mathf.Min(net, 100)) * environment;
        score = ApplyPenalty();
        InformUser();

        return score / scaling;
    }

    private float ScoreAfterPPremoval()
    {
        float score = lastScoreBeforeScaling;

        float installation;
        float connectionType;

        //summer
        installation = ppInstSummer;
        connectionType = ppConSummer;
        summer = CalcSummer(installation, connectionType);
        //winter
        installation = ppInstWinter;
        connectionType = ppConWinter;
        winter = CalcWinter(installation, connectionType);
        //night
        installation = ppInstNight;
        connectionType = ppConNight;
        night = CalcNight(installation, connectionType);
        //net
        installation = ppInstNet;
        connectionType = ppConNet;
        net = CalcNet(installation, connectionType);
        //environment
        installation = ppInstEnvironment;
        connectionType = ppConEnvironment;
        environment = CalcEnvironment(installation, connectionType);

        supply = Mathf.Min(summer, winter, night);

        lastScoreBeforeScaling = lastScoreBeforeScaling + (Mathf.Min(supply, 100) + Mathf.Min(net, 100)) * environment;

        //InformUser();

        //score = ApplyPenalty();

        //Debug.Log("Score after PP ex, before scaling: " + lastScoreBeforeScaling);

        return score / scaling;
    }

    private float ScoreAfterUserInstallation(string installation, GameObject node)
    {
        float score = lastScoreBeforeScaling;

        float installationVal;
        float connectionType;

        if (installation != "")
        {
            if (installation == "SolarPark-Img(Clone)")
            {
                //summer
                installationVal = pvInstSummer;
                connectionType = CalcConnectionValue(node, "Summer");
                summer = CalcSummer(installationVal, connectionType);
                //winter
                installationVal = pvInstWinter;
                connectionType = CalcConnectionValue(node, "Winter");
                winter = CalcWinter(installationVal, connectionType);
                //night
                installationVal = pvInstNight;
                connectionType = CalcConnectionValue(node, "Night");
                night = CalcNight(installationVal, connectionType);
                //net
                installationVal = pvInstNet;
                connectionType = CalcConnectionValue(node, "Net");
                net = CalcNet(installationVal, connectionType);
                //environment
                installationVal = pvInstEnvironment;
                connectionType = CalcConnectionValue(node, "Enviroment");
                environment = CalcEnvironment(installationVal, connectionType);
            }
            else if (installation == "Pinwheel-Img(Clone)")
            {
                //summer
                installationVal = pinInstSummer;
                connectionType = CalcConnectionValue(node, "Summer");
                summer = CalcSummer(installationVal, connectionType);
                //winter
                installationVal = pinInstWinter;
                connectionType = CalcConnectionValue(node, "Winter");
                winter = CalcWinter(installationVal, connectionType);
                //night
                installationVal = pinInstNight;
                connectionType = CalcConnectionValue(node, "Night");
                night = CalcNight(installationVal, connectionType);
                //net
                installationVal = pinInstNet;
                connectionType = CalcConnectionValue(node, "Net");
                net = CalcNet(installationVal, connectionType);
                //environment
                installationVal = pinInstEnvironment;
                connectionType = CalcConnectionValue(node, "Enviroment");
                environment = CalcEnvironment(installationVal, connectionType);
            }
            else if (installation == "Energystorage-Img(Clone)")
            {
                //summer
                installationVal = storageInstSummer;
                connectionType = CalcConnectionValue(node, "Summer");
                summer = CalcSummer(installationVal, connectionType);
                //winter
                installationVal = storageInstWinter;
                connectionType = CalcConnectionValue(node, "Winter");
                winter = CalcWinter(installationVal, connectionType);
                //night
                installationVal = storageInstNight;
                connectionType = CalcConnectionValue(node, "Night");
                night = CalcNight(installationVal, connectionType);
                //net
                installationVal = storageInstNet;
                connectionType = CalcConnectionValue(node, "Net");
                net = CalcNet(installationVal, connectionType);
                //environment
                installationVal = storageInstEnvironment;
                connectionType = CalcConnectionValue(node, "Enviroment");
                environment = CalcEnvironment(installationVal, connectionType);
            }
            else if (installation == "E-Car-Img(Clone)")
            {
                //summer
                installationVal = eCarInstSummer;
                connectionType = CalcConnectionValue(node, "Summer");
                summer = CalcSummer(installationVal, connectionType);
                //winter
                installationVal = eCarInstWinter;
                connectionType = CalcConnectionValue(node, "Winter");
                winter = CalcWinter(installationVal, connectionType);
                //night
                installationVal = eCarInstNight;
                connectionType = CalcConnectionValue(node, "Night");
                night = CalcNight(installationVal, connectionType);
                //net
                installationVal = eCarInstNet;
                connectionType = CalcConnectionValue(node, "Net");
                net = CalcNet(installationVal, connectionType);
                //environment
                installationVal = eCarInstEnvironment;
                connectionType = CalcConnectionValue(node, "Enviroment");
                environment = CalcEnvironment(installationVal, connectionType);
            }
        }
        else
        {

            score = ApplyPenalty();
            InformUser();
            return score / scaling;
        }


        supply = Mathf.Min(summer, winter, night);

        lastScoreBeforeScaling = score + (Mathf.Min(supply, 100) + Mathf.Min(net, 100)) * environment;

        InformUser();

        score = ApplyPenalty();

        //Debug.Log("Score after player building, before scaling: " + lastScoreBeforeScaling);

        return score / scaling;
    }

    private float ApplyPenalty()
    {
        string statusMessage = "";
        if (supply < redSupply || net < redNet)
        {

            lastScoreBeforeScaling = lastScoreBeforeScaling * (1 - powerShortagePenalty);
            grid.PowerFailure(false);
            if (!powerShortage)
            {
                statusMessages.QueueMessage(statusMessage);
                powerShortage = true;
            }
        }
        else
        {
            if (powerShortage)
            {
                powerShortage = false;
                grid.PowerFailure(true);
                statusMessages.QueueMessage("Strom wieder verfügbar!");
                writeLog.AddMessageToLog("Strom wieder verfügbar!");
            }
        }

        return lastScoreBeforeScaling;
    }

    private void InformUser()
    {
        string statusMessage = "";
        //red messages
        if (summer < redSummer)
        {
            statusMessage = redFeedbackSummer[Random.Range(0, redFeedbackSummer.Length)];
            PassStatusMessage(statusMessage);
        }
        if (winter < redWinter)
        {
            statusMessage = redFeedbackWinter[Random.Range(0, redFeedbackWinter.Length)];
            PassStatusMessage(statusMessage);
        }
        if (night < redNight)
        {
            statusMessage = redFeedbackNight[Random.Range(0, redFeedbackNight.Length)];
            PassStatusMessage(statusMessage);
        }

        if (net < redNet)
        {
            statusMessage = redFeedbackNet[Random.Range(0, redFeedbackNet.Length)];
            PassStatusMessage(statusMessage);
        }

        if (environment < redEnvironment)
        {
            statusMessage = redFeedbackEnvironment[Random.Range(0, redFeedbackEnvironment.Length)];
            PassStatusMessage(statusMessage);
        }

        if (supply < redSupply)
        {
            statusMessage = redFeedbackSupply[Random.Range(0, redFeedbackSupply.Length)];
            PassStatusMessage(statusMessage);
        }

        //yellow messages
        if (summer < greenSummer && summer >= yellowSummer)
        {
            statusMessage = yellowFeedbackSummer[Random.Range(0, yellowFeedbackSummer.Length)];
            PassStatusMessage(statusMessage);
        }
        if (winter < greenWinter && winter >= yellowWinter)
        {
            statusMessage = yellowFeedbackWinter[Random.Range(0, yellowFeedbackWinter.Length)];
            PassStatusMessage(statusMessage);
        }
        if (night < greenNight && night >= yellowNight)
        {
            statusMessage = yellowFeedbackNight[Random.Range(0, yellowFeedbackNight.Length)];
            PassStatusMessage(statusMessage);
        }

        if (net < greenNet && net >= yellowNet)
        {
            statusMessage = yellowFeedbackNet[Random.Range(0, yellowFeedbackNet.Length)];
            PassStatusMessage(statusMessage);
        }

        if (environment < greenEnvironment && environment >= yellowEnvironment)
        {
            statusMessage = yellowFeedbackEnvironment[Random.Range(0, yellowFeedbackEnvironment.Length)];
            PassStatusMessage(statusMessage);
        }

        if (supply < greenSupply && supply >= yellowSupply)
        {
            statusMessage = yellowFeedbackSupply[Random.Range(0, yellowFeedbackSupply.Length)];
            PassStatusMessage(statusMessage);
        }

        //green messages
        if (summer > greenSummer)
        {
            statusMessage = greenFeedbackSummer[Random.Range(0, greenFeedbackSummer.Length)];
            PassStatusMessage(statusMessage);
        }
        if (winter > greenWinter)
        {
            statusMessage = greenFeedbackWinter[Random.Range(0, greenFeedbackWinter.Length)];
            PassStatusMessage(statusMessage);
        }
        if (night > greenNight)
        {
            statusMessage = greenFeedbackNight[Random.Range(0, greenFeedbackNight.Length)];
            PassStatusMessage(statusMessage);
        }

        if (net > greenNet)
        {
            statusMessage = greenFeedbackNet[Random.Range(0, greenFeedbackNet.Length)];
            PassStatusMessage(statusMessage);
        }

        if (environment > greenEnvironment)
        {
            statusMessage = greenFeedbackEnvironment[Random.Range(0, greenFeedbackEnvironment.Length)];
            PassStatusMessage(statusMessage);
        }

        if (supply > greenSupply)
        {
            statusMessage = greenFeedbackSupply[Random.Range(0, greenFeedbackSupply.Length)];
            PassStatusMessage(statusMessage);
        }
    }

    private void PassStatusMessage(string statusMessage)
    {
        if (statusMessage != "" && statusMessage != "\r")
        {
            statusMessages.QueueMessage(statusMessage);
            writeLog.AddMessageToLog(statusMessage);
        }
    }

    /*
    private void InformUserOld()
    {
        string statusMessage = "";
        //red messages
        if (summer < redSummer && enteredSummerRed == false)
        {
            enteredSummerRed = true;
            enteredSummerYellow = false;
            enteredSummerGreen = false;
            statusMessage = redFeedbackSummer[Random.Range(0, redFeedbackSummer.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }
        if (winter < redWinter && enteredWinterRed == false)
        {
            enteredWinterRed = true;
            enteredWinterYellow = false;
            enteredWinterGreen = false;
            statusMessage = redFeedbackWinter[Random.Range(0, redFeedbackWinter.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }
        if (night < redNight && enteredNightRed == false)
        {
            enteredNightRed = true;
            enteredNightYellow = false;
            enteredNightGreen = false;
            statusMessage = redFeedbackNight[Random.Range(0, redFeedbackNight.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }

        if (net < redNet && enteredNetRed == false)
        {
            enteredNetRed = true;
            enteredNetYellow = false;
            enteredNetGreen = false;
            statusMessage = redFeedbackNet[Random.Range(0, redFeedbackNet.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }

        if (environment < redEnvironment && enteredEnvironmentRed == false)
        {
            enteredEnvironmentRed = true;
            enteredEnvironmentYellow = false;
            enteredEnvironmentGreen = false;
            statusMessage = redFeedbackEnvironment[Random.Range(0, redFeedbackEnvironment.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }

        if (supply < redSupply && enteredSupplyRed == false)
        {
            enteredSupplyRed = true;
            enteredSupplyYellow = false;
            enteredSupplyGreen = false;
            statusMessage = redFeedbackSupply[Random.Range(0, redFeedbackSupply.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }

        //yellow messages
        if (summer < greenSummer && summer > yellowSummer && enteredSummerYellow == false)
        {
            if (enteredSummerGreen)
            {
                statusMessage = yellowFeedbackSummer[Random.Range(0, yellowFeedbackSummer.Length)];
                GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
            }
            enteredSummerRed = false;
            enteredSummerYellow = true;
            enteredSummerGreen = false;
        }
        if (winter < greenWinter && winter > yellowWinter && enteredWinterYellow == false)
        {
            if (enteredWinterGreen)
            {
                statusMessage = yellowFeedbackWinter[Random.Range(0, yellowFeedbackWinter.Length)];
                GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
            }
            enteredWinterRed = false;
            enteredWinterYellow = true;
            enteredWinterGreen = false;
        }
        if (night < greenNight && night > yellowNight && enteredNightYellow == false)
        {
            if (enteredNightGreen)
            {
                statusMessage = yellowFeedbackNight[Random.Range(0, yellowFeedbackNight.Length)];
                GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
            }
            enteredNightRed = false;
            enteredNightYellow = true;
            enteredNightGreen = false;
        }

        if (net < greenNet && net > yellowNet && enteredNetYellow == false)
        {
            if (enteredNetGreen)
            {
                statusMessage = yellowFeedbackNet[Random.Range(0, yellowFeedbackNet.Length)];
                GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
            }
            enteredNetRed = false;
            enteredNetYellow = true;
            enteredNetGreen = false;
        }

        if (environment < greenEnvironment && environment > yellowEnvironment && enteredEnvironmentYellow == false)
        {
            if (enteredEnvironmentGreen)
            {
                statusMessage = yellowFeedbackEnvironment[Random.Range(0, yellowFeedbackEnvironment.Length)];
                GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
            }
            enteredEnvironmentRed = false;
            enteredEnvironmentYellow = true;
            enteredEnvironmentGreen = false;
        }

        if (supply < greenSupply && supply > yellowSupply && enteredSupplyYellow == false)
        {
            if (enteredSupplyGreen)
            {
                statusMessage = yellowFeedbackSupply[Random.Range(0, yellowFeedbackSupply.Length)];
                GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
            }
            enteredSupplyRed = false;
            enteredSupplyYellow = true;
            enteredSupplyGreen = false;
        }

        //green messages
        if (summer > greenSummer && enteredSummerGreen == false)
        {
            enteredSummerRed = false;
            enteredSummerYellow = true;
            enteredSummerGreen = false;
            statusMessage = greenFeedbackSummer[Random.Range(0, greenFeedbackSummer.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }
        if (winter > greenWinter && enteredWinterGreen == false)
        {
            enteredWinterRed = false;
            enteredWinterYellow = false;
            enteredWinterGreen = true;
            statusMessage = greenFeedbackWinter[Random.Range(0, greenFeedbackWinter.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }
        if (night > greenNight && enteredNightGreen == false)
        {
            enteredNightRed = false;
            enteredNightYellow = false;
            enteredNightGreen = true;
            statusMessage = greenFeedbackNight[Random.Range(0, greenFeedbackNight.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }

        if (net > greenNet && enteredNetGreen == false)
        {
            enteredNetRed = false;
            enteredNetYellow = false;
            enteredNetGreen = true;
            statusMessage = greenFeedbackNet[Random.Range(0, greenFeedbackNet.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }

        if (environment > greenEnvironment && enteredEnvironmentGreen == false)
        {
            enteredEnvironmentRed = false;
            enteredEnvironmentYellow = false;
            enteredEnvironmentGreen = true;
            statusMessage = greenFeedbackEnvironment[Random.Range(0, greenFeedbackEnvironment.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }

        if (supply > greenSupply && enteredSupplyGreen == false)
        {
            enteredSupplyRed = false;
            enteredSupplyYellow = false;
            enteredSupplyGreen = true;
            statusMessage = greenFeedbackSupply[Random.Range(0, greenFeedbackSupply.Length)];
            GameObject.Find("StatusMessages").GetComponent<CreateText>().QueueMessage(statusMessage);
        }
    }
    */

    private float CalcConnectionValue(GameObject node, string accountType)
    {
        float connectionValue = 0;

        if (node.GetComponent<BuildingNode>().far == false)
        {
            if (node.GetComponent<BuildingNode>().powerType == BuildingNode.PowerSupply.AC)
            {
                if (accountType == "Summer")
                {
                    connectionValue = acCloseSummer;
                }
                else if (accountType == "Winter")
                {
                    connectionValue = acCloseWinter;
                }
                else if (accountType == "Night")
                {
                    connectionValue = acCloseNight;
                }
                else if (accountType == "Net")
                {
                    connectionValue = acCloseNet;
                }
                else if (accountType == "Environment")
                {
                    connectionValue = acCloseEnvironment;
                }
            }
            if (node.GetComponent<BuildingNode>().powerType == BuildingNode.PowerSupply.DC)
            {
                if (accountType == "Summer")
                {
                    connectionValue = dcCloseSummer;
                }
                else if (accountType == "Winter")
                {
                    connectionValue = dcCloseWinter;
                }
                else if (accountType == "Night")
                {
                    connectionValue = dcCloseNight;
                }
                else if (accountType == "Net")
                {
                    connectionValue = dcCloseNet;
                }
                else if (accountType == "Environment")
                {
                    connectionValue = dcCloseEnvironment;
                }
            }
        }
        else
        {
            if (accountType == "Summer")
            {
                connectionValue = dcFarSummer;
            }
            else if (accountType == "Winter")
            {
                connectionValue = dcFarWinter;
            }
            else if (accountType == "Night")
            {
                connectionValue = dcFarNight;
            }
            else if (accountType == "Net")
            {
                connectionValue = dcFarNet;
            }
            else if (accountType == "Environment")
            {
                connectionValue = dcFarEnvironment;
            }
        }


        return connectionValue;
    }

    private float CalcSummer(float installation, float connectionType)
    {
        float summer;

        summer = this.summer + installation + connectionType;

        return summer;
    }

    private float CalcWinter(float installation, float connectionType)
    {
        float winter;

        winter = this.winter + installation + connectionType;

        return winter;
    }

    private float CalcNight(float installation, float connectionType)
    {
        float night;

        night = this.night + installation + connectionType;

        return night;
    }

    private float CalcNet(float installation, float connectionType)
    {
        float net;

        net = this.net + installation + connectionType;

        return net;
    }

    private float CalcEnvironment(float installation, float connectionType)
    {
        float environment;

        environment = this.environment + installation + connectionType;

        return environment;
    }

    private float CalcSupply(float installation, float connectionType)
    {
        float supply;

        supply = this.supply + installation + connectionType;

        return supply;
    }

    private void WriteLogFile()
    {
        writeLog.AddDataToLogFile("Summer: " + summer);
        writeLog.AddDataToLogFile("Winter: " + winter);
        writeLog.AddDataToLogFile("Night: " + night);
        writeLog.AddDataToLogFile("Net: " + net);
        writeLog.AddDataToLogFile("Environment: " + environment);
        writeLog.AddDataToLogFile("Supply: " + supply);
        writeLog.AddDataToLogFile("Score before scaling: " + lastScoreBeforeScaling);
        writeLog.AddDataToLogFile("Score after scaling: " + (lastScoreBeforeScaling / scaling));
    }
}
