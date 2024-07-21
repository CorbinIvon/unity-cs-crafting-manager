using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    public int Id { get; private set; }
    public static CraftingManager craftingManager;
    public string stationType; // Set this to the type of the crafting station (e.g., "Forge")

    private List<CraftingStationData> _craftableItems;
    private Dictionary<string, CraftingStationData> _itemDataMap;

    private void Awake()
    {
        Id = GetInstanceID(); // Use instance ID as a unique identifier
        craftingManager = CraftingManager.Instance;
        craftingManager.RegisterCraftingStation(this);

        // Get the list of items this station can craft
        _craftableItems = craftingManager.GetItemsForStation(stationType);
        _itemDataMap = new Dictionary<string, CraftingStationData>();

        foreach (var item in _craftableItems)
        {
            _itemDataMap[item.itemName] = item;
            Debug.Log($"This {stationType} can craft: {item.itemName} with amount {item.amount} and craft time {item.craftTime}");
        }
    }

    private void OnDestroy()
    {
        craftingManager.UnregisterCraftingStation(Id);
    }

    public void TestCraft()
    {
        Debug.Log($"Crafting started at station {Id}");
        // Example crafting task
        if (_craftableItems.Count > 0)
        {
            CraftingStationData itemToCraft = _craftableItems[0]; // For example, craft the first item in the list
            CraftingTask task = new CraftingTask(Id, itemToCraft.craftTime, itemToCraft.itemName, this);
            craftingManager.AddCraftingTask(task);
        }
        else
        {
            Debug.Log($"No items to craft at this {stationType}.");
        }
    }

    public void OnCraftingTaskCompleted(string itemName)
    {
        Debug.Log($"Crafting completed at station {Id}: {itemName}");
        
        if (_itemDataMap.TryGetValue(itemName, out CraftingStationData itemData))
        {
            // Get the prefab for the crafted item
            GameObject itemPrefab = Resources.Load<GameObject>(itemData.prefabPath);
            if (itemPrefab != null)
            {
                // Get the offsets.
                Vector3 spawnOffsetPosition = transform.position + itemData.spawnOffsetPositionRelative;
                Vector3 spawnOffsetRotation = itemData.spawnOffsetRotation;
                Vector3 spawnOffsetScale = itemData.spawnOffsetScale;

                // Instantiate the item on top of the crafting station
                GameObject itemInstance = Instantiate(itemPrefab, spawnOffsetPosition, Quaternion.Euler(spawnOffsetRotation));
                itemInstance.transform.localScale = spawnOffsetScale;

                // Attach the CraftedItem script and initialize it
                CraftedItem craftedItem = itemInstance.AddComponent<CraftedItem>();
                craftedItem.Initialize(
                    itemName,
                    itemData.ingredients,
                    itemData.spawnOffsetPositionRelative,
                    itemData.spawnOffsetRotation,
                    itemData.spawnOffsetScale,
                    stationType,
                    transform.position,
                    itemData.craftTime,
                    System.DateTime.Now
                );
            }
            else
            {
                Debug.LogWarning($"Item prefab not found at path: {itemData.prefabPath}");
            }
        }
        else
        {
            Debug.LogWarning($"Item data not found for {itemName}");
        }
    }
}
