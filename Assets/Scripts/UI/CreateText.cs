using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateText : MonoBehaviour
{
    [Header("CoRoutine Status")]
    public bool qCoRunning = false;
    public bool isPaused = false;

    [Header("Font")]
    public Font font;
    
    private LinkedList<string> messageQ = new LinkedList<string>();

    public void QueueMessage(string messageToQ)
    {
        messageQ.AddLast(messageToQ);
    }

    public IEnumerator ShowQueuedMessages()
    {
        qCoRunning = true;
        while (messageQ.Count != 0)
        {
            while (isPaused)
            {
                yield return null;
            }
            AddTextBlock(messageQ.First.Value);
            messageQ.RemoveFirst();
            yield return new WaitForSeconds(1);
        }
        yield return null;
        qCoRunning = false;
    }

    public void AddTextBlock(string textToAdd)
    {
        GameObject floatingText = new GameObject("statusMessage");

        floatingText.AddComponent<Outline>();
        floatingText.GetComponent<Outline>().effectColor = new Color(255, 255, 255, 255);

        floatingText.transform.SetParent(this.transform);
        floatingText.AddComponent<Text>();

        floatingText.GetComponent<RectTransform>().sizeDelta = new Vector2(1080, 50);
        floatingText.GetComponent<Transform>().localPosition = new Vector3(0, -350, 0);

        Color col = new Color(0, 0, 255, 255); // black, full visible
        floatingText.GetComponent<Text>().color = col;

        floatingText.GetComponent<Text>().font = font;
        floatingText.GetComponent<Text>().fontSize = 28;
        floatingText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        floatingText.GetComponent<Text>().text = textToAdd;

        StartCoroutine( FloatingFade(floatingText) );
    }

    IEnumerator FloatingFade(GameObject text)
    {
        float rate = 1f / 7f;
        float progress = 0;
        while (progress < 1)
        {
            while (isPaused)
            {
                yield return null;
            }
            text.transform.localPosition += new Vector3(0, 1.5f, 0);
            if (progress > 0.25)
                text.GetComponent<Text>().CrossFadeAlpha(0, 7.5f, false);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        Destroy(text);
    }

}