using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingPP : MonoBehaviour {

    [Header("Use Grey Symbol")]
    public bool useGrey = false;
    public Sprite ppGrey;

    private int counter = 19;

	public void RemovePPpikto()
    {
        Transform powerPlantPictogram = this.gameObject.transform.GetChild(counter);
        if (useGrey)
        {
            powerPlantPictogram.gameObject.GetComponent<Image>().sprite = ppGrey;
        }
        else
        {
            Destroy(powerPlantPictogram.gameObject);
        }
        counter--;
    }
}
