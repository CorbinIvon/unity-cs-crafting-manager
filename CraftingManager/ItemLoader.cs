using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    private string dataPath = "StreamingAssets/Data";
    private string modsPath = "StreamingAssets/Mods";

    public List<ItemData> LoadItems()
    {
        List<ItemData> allItems = new List<ItemData>();

        // Load base game items
        string dataFilePath = Path.Combine(Application.dataPath, dataPath, "items.xml");
        LoadItemsFromFile(dataFilePath, allItems);

        // Load mod items
        string modsDirectoryPath = Path.Combine(Application.dataPath, modsPath);
        if (Directory.Exists(modsDirectoryPath))
        {
            string[] modDirectories = Directory.GetDirectories(modsDirectoryPath);
            foreach (var modDirectory in modDirectories)
            {
                string modFilePath = Path.Combine(modDirectory, "items.xml");
                LoadItemsFromFile(modFilePath, allItems);
            }
        }

        return allItems;
    }

    private void LoadItemsFromFile(string filePath, List<ItemData> itemList)
    {
        if (File.Exists(filePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNodeList itemNodes = xmlDoc.SelectNodes("//item");
            foreach (XmlNode itemNode in itemNodes)
            {
                ItemData itemData = new ItemData
                {
                    itemName = itemNode.Attributes["value"].Value,
                    prefabPath = itemNode["prefabPath"].Attributes["value"].Value
                };

                XmlNodeList stationNodes = itemNode.SelectNodes("craftingStations/station");
                foreach (XmlNode stationNode in stationNodes)
                {
                    CraftingStationData stationData = new CraftingStationData
                    {
                        itemName = itemData.itemName, // Set the item name here
                        stationName = stationNode.Attributes["value"].Value,
                        amount = stationNode.SelectSingleNode("amount") != null ? int.Parse(stationNode.SelectSingleNode("amount").Attributes["value"].Value) : 1,
                        craftTime = stationNode.SelectSingleNode("craftTime") != null ? float.Parse(stationNode.SelectSingleNode("craftTime").Attributes["value"].Value) : 0f,
                        spawnOffsetPositionRelative = stationNode.SelectSingleNode("spawnOffsetPositionRelative") != null ? ParseVector3(stationNode.SelectSingleNode("spawnOffsetPositionRelative").Attributes["value"].Value) : Vector3.zero,
                        spawnOffsetRotation = stationNode.SelectSingleNode("spawnOffsetRotation") != null ? ParseVector3(stationNode.SelectSingleNode("spawnOffsetRotation").Attributes["value"].Value) : Vector3.zero,
                        spawnOffsetScale = stationNode.SelectSingleNode("spawnOffsetScale") != null ? ParseVector3(stationNode.SelectSingleNode("spawnOffsetScale").Attributes["value"].Value) : Vector3.one,
                        prefabPath = itemData.prefabPath // Assign the prefab path for the crafted item
                    };

                    XmlNodeList ingredientNodes = stationNode.SelectNodes("ingredients/ingredient");
                    foreach (XmlNode ingredientNode in ingredientNodes)
                    {
                        IngredientData ingredientData = new IngredientData
                        {
                            ingredientName = ingredientNode.Attributes["value"].Value,
                            amount = int.Parse(ingredientNode.Attributes["amount"].Value)
                        };
                        stationData.ingredients.Add(ingredientData);
                    }

                    itemData.craftingStations.Add(stationData);
                }

                itemList.Add(itemData);
            }
        }
        else
        {
            Debug.LogWarning($"Item file not found at {filePath}");
        }
    }

    private Vector3 ParseVector3(string value)
    {
        string[] values = value.Split(',');
        return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
    }
}
