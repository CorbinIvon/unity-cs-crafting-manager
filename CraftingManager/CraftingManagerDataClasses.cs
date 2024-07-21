using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string itemName;
    public string prefabPath;
    public List<CraftingStationData> craftingStations = new List<CraftingStationData>();
}

public class CraftingStationData
{
    public string itemName; // The name of the item being crafted
    public string stationName;
    public int amount;
    public float craftTime;
    public Vector3 spawnOffsetPositionRelative = Vector3.zero;
    public Vector3 spawnOffsetRotation = Vector3.zero;
    public Vector3 spawnOffsetScale = Vector3.one;
    public List<IngredientData> ingredients = new List<IngredientData>();
    public string prefabPath; // Path to the prefab for the crafted item
}

public class IngredientData
{
    public string ingredientName;
    public int amount;
}
