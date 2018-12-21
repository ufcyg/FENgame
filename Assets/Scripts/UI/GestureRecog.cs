using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.
using System;

public class GestureRecog : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float delay;
    public GameObject dialog;
    bool countdownStarted = false;
    float startTime;
    float timeNow;

    private void Update()
    {
        if (countdownStarted && (timeNow - startTime)>delay)
        {
            dialog.SetActive(true);
        }
        else if (!countdownStarted)
        {
            startTime = Time.time;
            timeNow = Time.time;
        }
        else
        {
            timeNow = Time.time;
        }

        //if (Input.anyKeyDown)
        //{
        //    Screen.SetResolution(800, 600, false);
        //}
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        countdownStarted = true;
        startTime = Time.time;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        countdownStarted = false;
    }
}
