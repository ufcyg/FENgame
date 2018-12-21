using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountDisplayController : MonoBehaviour
{
    [Header("Slider")]
    public Slider summerSlider;
    public Slider winterSlider;
    public Slider nightSlider;
    public Slider netSlider;
    public Slider supplySlider;
    public Slider environmentSlider;

    [Header("Fillarea Handler")]
    public Image summerFill;
    public Image winterFill;
    public Image nightFill;
    public Image netFill;
    public Image supplyFill;
    public Image environmentFill;

    [Header("Thresholds")]
    public float summerRed;
    public float winterRed;
    public float nightRed;
    public float netRed;
    public float supplyRed;
    public float environmentRed;
    [Space(10)]
    public float summerYellow;
    public float winterYellow;
    public float nightYellow;
    public float netYellow;
    public float supplyYellow;
    public float environmentYellow;
    [Space(10)]
    public float summerGreen;
    public float winterGreen;
    public float nightGreen;
    public float netGreen;
    public float supplyGreen;
    public float environmentGreen;

    private Color green = new Color32(10, 150, 10, 255);
    private Color yellow = new Color32(200, 200, 0, 255);
    private Color red = new Color32(150, 10, 10, 255);

    public void Init()
    {
        float minSliderValSummer;
        float minSliderValWinter;
        float minSliderValNight;
        float minSliderValNet;
        float minSliderValSupply;
        // calc minimum values
        minSliderValSummer = summerRed / 2;
        minSliderValWinter = winterRed / 2;
        minSliderValNight = nightRed / 2;
        minSliderValNet = netRed / 2;
        minSliderValSupply = supplyRed / 2;
        // Set minimum values
        summerSlider.minValue = minSliderValSummer;
        winterSlider.minValue = minSliderValWinter;
        nightSlider.minValue = minSliderValNight;
        netSlider.minValue = minSliderValNet;
        supplySlider.minValue = minSliderValSupply;
    }

    public void SetSliderValues(float summer, float winter, float night, float net, float supply, float environment)
    {
        SetSlider(summerSlider, summerFill, summer, summerGreen, summerYellow, summerRed);
        SetSlider(winterSlider, winterFill, winter, winterGreen, winterYellow, winterRed);
        SetSlider(nightSlider, nightFill, night, nightGreen, nightYellow, nightRed);
        SetSlider(netSlider, netFill, net, netGreen, netYellow, netRed);
        SetSlider(supplySlider, supplyFill, supply, supplyGreen, supplyYellow, supplyRed);
        SetSlider(environmentSlider, environmentFill, environment, environmentGreen, environmentYellow, environmentRed);
    }

    private void SetSlider(Slider _slider, Image _fill, float newVal, float thresholdGreen, float thresholdYellow, float thresholdRed)
    {
        if (newVal > thresholdGreen)
        {
            _fill.color = green;
        }
        else if (newVal < thresholdGreen && newVal > thresholdRed)
        {
            _fill.color = yellow;
        }
        else if (newVal < thresholdRed)
        {
            _fill.color = red;
        }
        _slider.value = newVal;
    }
}
