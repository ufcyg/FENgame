using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGrid : MonoBehaviour {

    [Header("Energy Nodes")]
    public GameObject[] energyNodes;

    [Header("Exchange Sprites")]
    public Sprite houseLighted;
    public Sprite housePVLighted;
    public Sprite houseNonLighted;
    public Sprite housePVNonLighted;
    public Sprite pinwheelSea;

    public void PowerFailure(bool status)
    {
        foreach (GameObject node in energyNodes)
        {
            foreach (GameObject building in node.GetComponent<BuildingNode>().connectedBuildings)
            {
                ChangePowerStatus(building, status);
            }
        }
    }

    private void ChangePowerStatus(GameObject _building, bool status)
    {
        if (_building.tag == "factory-smoke" && status == false)
        {
            var emission = _building.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            emission.enabled = false;
            _building.tag = "factory-nosmoke";
            return;
        }
        if (_building.tag == "factory-nosmoke" && status == true)
        {
            var emission = _building.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            emission.enabled = true;
            _building.tag = "factory-smoke";
            return;
        }
        if (_building.tag == "house-lighted" && status == false)
        {
            _building.GetComponent<SpriteRenderer>().sprite = houseNonLighted;
            _building.tag = "house-notlighted";
            return;
        }
        if (_building.tag == "house-notlighted" && status == true)
        {
            _building.GetComponent<SpriteRenderer>().sprite = houseLighted;
            _building.tag = "house-lighted";
            return;
        }
        if (_building.tag == "housePV-lighted" && status == false)
        {
            _building.GetComponent<SpriteRenderer>().sprite = housePVNonLighted;
            _building.tag = "housePV-notlighted";
            return;
        }
        if (_building.tag == "housePV-notlighted" && status == true)
        {
            _building.GetComponent<SpriteRenderer>().sprite = housePVLighted;
            _building.tag = "housePV-lighted";
            return;
        }
    }
}
