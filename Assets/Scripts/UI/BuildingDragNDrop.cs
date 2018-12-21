using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingDragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Building Sounds")]
    public AudioSource failedToBuild;
    public AudioSource buildingSound;

    [Header("GameObject References")]
    public EnergyGrid grid;
    public RoundManager roundManager;
    public Transform playersBuildings;

    [Header("Drag'n'Drop")]
    public Vector3 startPosition;
    public static GameObject itemDragged;
    public Material highlightMaterial;

    [Header("Message System")]
    public CreateText addMessage;

    //highlight
    private GameObject nodeToHighlight;
    private GameObject prevNodeToHighlight;
    private Material prevMaterial;
    private bool highlighted = false;

    private Transform sprite;

    private void Start()
    {
        startPosition = transform.position;
    }

    /// <summary>
    /// Unity predefined Function for the event system.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemDragged = Object.Instantiate(gameObject);
        sprite = gameObject.transform.parent.GetChild(1);
    }

    /// <summary>
    /// Unity predefined Function for the event system.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        HighlightClosestNode(transform.position);
    }

    /// <summary>
    /// Unity predefined Function for the event system.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!roundManager.buildingPlaced)
        {
            if (nodeToHighlight != null)
                AddSprite(nodeToHighlight);
        }
        transform.position = startPosition;
        RemoveHighlight();
        if (nodeToHighlight != null)
        {
            roundManager.closestNode = nodeToHighlight;
            roundManager.installation = itemDragged.name;
        }

        Destroy(itemDragged);
    }

    public void RemoveHighlight()
    {
        if (prevNodeToHighlight != null)
        {
            prevNodeToHighlight.GetComponentInChildren<Renderer>().sharedMaterial = prevMaterial;
            highlighted = false;
        }
    }
    private void AddHighlight()
    {
        prevMaterial = nodeToHighlight.GetComponentInChildren<Renderer>().sharedMaterial;
        nodeToHighlight.GetComponentInChildren<Renderer>().sharedMaterial = highlightMaterial;
        prevNodeToHighlight = nodeToHighlight;
        highlighted = true;
    }

    private void HighlightClosestNode(Vector3 mousePos)
    {
        nodeToHighlight = CalcClosestNode(mousePos);

        //if highlighted == false -> nothing is highlighted

        if (!highlighted) //-> nothing is highlighted
        {
            if (nodeToHighlight != null) //-> there is a node to be highlighted
            {
                AddHighlight();
            }
        }
        else //-> smth is already highlighted
        {
            RemoveHighlight();
            if (nodeToHighlight != null) //-> there is a node to be highlighted
            {
                AddHighlight();
            }
        }
    }

    /// <summary>
    /// Checks if the given EnergyNode has LivingHouses
    /// </summary>
    /// <returns></returns>
    private bool HasLivingHouses(GameObject closestNode)
    {
        bool hasHouses = false;
        foreach (GameObject go in closestNode.GetComponent<BuildingNode>().connectedBuildings)
        {
            if (go.tag == "house-lighted" || go.tag == "house-notlighted")
            {
                hasHouses = true;
            }
        }
        return hasHouses;
    }

    /// <summary>
    /// Replaces existing living houses with living houses w/ PV installed.
    /// </summary>
    private void ReplaceLivingWithPV(GameObject closestNode)
    {
        foreach (GameObject go in closestNode.GetComponent<BuildingNode>().connectedBuildings)
        {
            if (go.tag == "house-notlighted")
            {
                go.GetComponent<SpriteRenderer>().sprite = grid.housePVNonLighted;
                go.tag = "housePV-notlighted";
            }
            else if (go.tag == "house-lighted")
            {
                go.GetComponent<SpriteRenderer>().sprite = grid.housePVLighted;
                go.tag = "housePV-lighted";
            }
            else
            {
                continue;
            }
        }
    }

    private void UpgradeCars(GameObject closestNode)
    {
        closestNode.GetComponent<BuildingNode>().carsUpgraded = true;
        closestNode.GetComponent<BuildingNode>().neighbour.GetComponent<BuildingNode>().carsUpgraded = true;
        foreach (GameObject go in closestNode.GetComponent<BuildingNode>().connectedBuildings)
        {
            Transform newCar = GameObject.Instantiate(sprite);
            newCar.parent = go.transform;
            newCar.localPosition = new Vector3(0, -1, 0);
            newCar.GetComponent<SpriteRenderer>().sortingLayerName = "Buildings";
            newCar.GetComponent<SpriteRenderer>().sortingOrder = (newCar.parent.GetComponent<SpriteRenderer>().sortingOrder + 1);
            newCar.localScale = new Vector3(1, 1, 1);
        }
    }

    private void SpritePositioning(Transform newPP, GameObject closestNode)
    {
        PlayBuildSound();
        newPP.parent = playersBuildings;
        newPP.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        closestNode.GetComponent<BuildingNode>().ScaleSprite(newPP);
        closestNode.GetComponent<BuildingNode>().SortSprite(newPP);
    }

    /// <summary>
    /// Adds a Sprite to the given Node, Sprite is predefined in ObjectStructure 
    /// as 2nd child of the equivalent building panel
    /// </summary>
    /// <param name="closestNode"></param>
    private void AddSprite(GameObject closestNode)
    {
        Transform newPP = GameObject.Instantiate(sprite);
        newPP.localScale = new Vector3(1, 1, 1);

        if (newPP.name == "SolarPark-Sprt(Clone)")
        {
            if (closestNode.GetComponent<BuildingNode>().onLand == false)
            {
                PlayFailedToBuild();
                Destroy(newPP.gameObject);
                addMessage.AddTextBlock("Photovoltaik Anlagen können nicht im Wasser errichtet werden!");
                return;
            }

            if (closestNode.GetComponent<BuildingNode>().pvUpgraded == false && HasLivingHouses(closestNode))
            {
                PlayBuildSound();
                ReplaceLivingWithPV(closestNode);
                Destroy(newPP.gameObject);
                closestNode.GetComponent<BuildingNode>().pvUpgraded = true;
            }
            else
            {
                SpritePositioning(newPP, closestNode);
            }
        }

        if (newPP.name == "E-Car-Sprt(Clone)")
        {
            if (closestNode.GetComponent<BuildingNode>().onLand == false)
            {
                PlayFailedToBuild();
                Destroy(newPP.gameObject);
                addMessage.AddTextBlock("Diese e-Autos sind keine Amphibienfahrzeuge!");
                return;
            }

            if (closestNode.GetComponent<BuildingNode>().carsUpgraded == false)
            {
                PlayBuildSound();
                UpgradeCars(closestNode);
                Destroy(newPP.gameObject);
            }
            else
            {
                PlayFailedToBuild();
                Destroy(newPP.gameObject);
                addMessage.AddTextBlock("Hier gibt es bereits e-Autos!");
                return;
            }
        }

        if (newPP.name == "Pinwheel-Sprt(Clone)")
        {
            if (closestNode.GetComponent<BuildingNode>().onLand == true)
            {
                SpritePositioning(newPP, closestNode);
            }
            else if (closestNode.GetComponent<BuildingNode>().onLand == false)
            {
                newPP.GetComponent<SpriteRenderer>().sprite = grid.pinwheelSea;

                SpritePositioning(newPP, closestNode);
            }
        }

        if (newPP.name == "Energystorage-Sprt(Clone)")
        {
            if (closestNode.GetComponent<BuildingNode>().onLand == false)
            {
                PlayFailedToBuild();
                Destroy(newPP.gameObject);
                addMessage.AddTextBlock("Das sind Stromspeicher... keine Wasserspeicher!");
                return;
            }

            SpritePositioning(newPP, closestNode);
        }

        roundManager.buildingPlaced = true;
    }

   /* Deprecated
    private Vector3 RandomizePlacement(GameObject closestNode)
    {
        Vector3 placingVector = new Vector3(0, 0, 0);

        if (closestNode.GetComponent<BuildingNode>().powerType == BuildingNode.PowerSupply.AC) //right
        {
            placingVector = new Vector3(Random.Range(0f, 1f), Random.Range(-.1f, 1), 0);
        }

        if (closestNode.GetComponent<BuildingNode>().powerType == BuildingNode.PowerSupply.DC) //left
        {
            placingVector = new Vector3(Random.Range(0f, -1f), Random.Range(-.1f, 1), 0);
        }

        return placingVector;
    }
    */

    /// <summary>
    /// Calculates closest Node on board to mouse position, and returns given Node
    /// Node may not be occupied by another building.
    /// </summary>
    /// <param name="mousePos"></param>
    /// <returns></returns>
    private GameObject CalcClosestNode(Vector3 mousePos)
    {
        GameObject nodeToReturn = null;
        float dist = 9999;
        var currentMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        foreach (GameObject energyNode in grid.GetComponent<EnergyGrid>().energyNodes)
        {
            float tempDist = Vector3.Distance(energyNode.transform.position, currentMousePos);
            if (tempDist < dist && tempDist < 4)
            {
                dist = tempDist;
                nodeToReturn = energyNode;
            }
        }
        return nodeToReturn;
    }

    private void PlayFailedToBuild()
    {
        failedToBuild.Play();
    }
    private void PlayBuildSound()
    {
        buildingSound.Play();
    }
}