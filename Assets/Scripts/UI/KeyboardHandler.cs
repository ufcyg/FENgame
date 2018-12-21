using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyboardHandler : MonoBehaviour
{
    [Header("References")]
    public InputField inputField;
    public GameObject[] keys;

    bool capitalized = false;


    public void ToggleKeyboard(bool status)
    {
        this.gameObject.SetActive(status);
    }

    public void Capslock()
    {
        capitalized = !capitalized;
        if (capitalized)
        {
            keys[0].GetComponent<Image>().color = new Color32(200, 200, 200, 255);
        }
        else
        {
            keys[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        foreach (GameObject key in keys)
        {
            if (capitalized)
            {
                key.GetComponentInChildren<Text>().text = key.GetComponentInChildren<Text>().text.ToUpper();
            }
            else
            {
                key.GetComponentInChildren<Text>().text = key.GetComponentInChildren<Text>().text.ToLower();
            }

        }
    }

    public void Backspace()
    {
        if (inputField.text.Length > 0)
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }

    public void KeyDown()
    {
        string keyName = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
        inputField.text = inputField.text + keyName;
    }
}