using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNode : MonoBehaviour {
    
    [Header("Other Gameobject References")]
    public GameObject neighbour;
    public GameObject[] connectedBuildings;
    public SpriteRenderer background;

    [Header("Stats")]
    public bool onLand;
    public bool far;
    public PowerSupply powerType;

    [Header("Upgrade Status")]
    public bool pvUpgraded = false;
    public bool carsUpgraded = false;

    public enum PowerSupply
    {
        UNDEFINED,
        AC,
        DC
    }

    public void SortSprite(Transform sprite)
    {
        sprite.GetComponent<SpriteRenderer>().sortingLayerName = "Buildings";
        sprite.GetComponent<SpriteRenderer>().sortingOrder = (int)Camera.main.WorldToScreenPoint(sprite.GetComponent<SpriteRenderer>().bounds.min).y * -1;
    }

    /// <summary>
    /// Scales sprite of passed sprite accordingly to relative y-Dimension of position in worldspace.
    /// </summary>
    /// <param name="sprite"></param>
    public void ScaleSprite(Transform sprite)
    {
        float backgroundExtents = background.bounds.extents.y;
        float scalingFac;

        Vector3 scalingVec = new Vector3(1, 1, 1);

        scalingFac = CalcScalingFactor(backgroundExtents, sprite.position.y);

        scalingVec = scalingVec * scalingFac;
        sprite.transform.localScale = scalingVec;
    }

    private float CalcScalingFactor(float bgExtents, float spritePosY)
    {
        float scalingFac = 0;

        if (spritePosY < 0) //y is negative -> lower on screen -> bigger
        {
            float yPosNormalized = Mathf.Abs(spritePosY) + bgExtents;
            scalingFac = yPosNormalized / (bgExtents * 2);
        }
        else if (spritePosY == 0)
        {
            scalingFac = 1;
        }
        else // y is positive -> higher on screen -> smaller
        {
            float yPosNormalized = Mathf.Abs(spritePosY - bgExtents);
            scalingFac = yPosNormalized / (bgExtents * 2);
        }
        //Debug.Log(scalingFac);
        return scalingFac;
    }
}