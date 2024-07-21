using UnityEngine;
using System.Collections.Generic;

public class CraftedItem : MonoBehaviour
{
    public string itemName;
    public List<IngredientData> ingredients;
    public Vector3 spawnOffsetPositionRelative;
    public Vector3 spawnOffsetRotation;
    public Vector3 spawnOffsetScale;

    // New properties
    public string craftingStationName;
    public Vector3 craftingStationPosition;
    public float craftTime;
    public float craftingDuration;
    public System.DateTime craftedAt;

    public void Initialize(
        string name,
        List<IngredientData> itemIngredients,
        Vector3 positionOffset,
        Vector3 rotationOffset,
        Vector3 scaleOffset,
        string stationName,
        Vector3 stationPosition,
        float duration,
        System.DateTime timeCrafted)
    {
        itemName = name;
        ingredients = itemIngredients;
        spawnOffsetPositionRelative = positionOffset;
        spawnOffsetRotation = rotationOffset;
        spawnOffsetScale = scaleOffset;
        craftingStationName = stationName;
        craftingStationPosition = stationPosition;
        craftingDuration = duration;
        craftedAt = timeCrafted;
    }
}
