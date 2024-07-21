using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    // Singleton instance
    private static CraftingManager _instance;

    // Lock object for thread-safety
    private static readonly object _lock = new object();

    // Dictionary to store crafting stations and their current tasks
    private Dictionary<int, CraftingStation> _craftingStations;

    // Queue to handle crafting tasks efficiently
    private Queue<CraftingTask> _craftingQueue;

    // Task processing flag
    private bool _isProcessingTasks = false;

    // Map of item names to GameObject prefabs
    private Dictionary<string, GameObject> _itemPrefabs;

    // Map of crafting stations to the items they can craft
    private Dictionary<string, List<CraftingStationData>> _craftingStationMap;

    public static CraftingManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    GameObject craftingManagerObject = new GameObject("CraftingManager");
                    _instance = craftingManagerObject.AddComponent<CraftingManager>();
                    DontDestroyOnLoad(craftingManagerObject);
                    _instance.InitializeItemLoader();
                }
                return _instance;
            }
        }
    }

    private void Awake()
    {
        // Initialize crafting stations, queue, item prefab map, and crafting station map
        _craftingStations = new Dictionary<int, CraftingStation>();
        _craftingQueue = new Queue<CraftingTask>();
        _itemPrefabs = new Dictionary<string, GameObject>();
        _craftingStationMap = new Dictionary<string, List<CraftingStationData>>();
    }

    private void InitializeItemLoader()
    {
        GameObject itemLoaderObject = new GameObject("ItemLoader");
        ItemLoader itemLoader = itemLoaderObject.AddComponent<ItemLoader>();
        DontDestroyOnLoad(itemLoaderObject);

        List<ItemData> items = itemLoader.LoadItems();
        foreach (var item in items)
        {
            GameObject prefab = Resources.Load<GameObject>(item.prefabPath);
            if (prefab != null)
            {
                AddItemPrefab(item.itemName, prefab);
                foreach (var station in item.craftingStations)
                {
                    if (!_craftingStationMap.ContainsKey(station.stationName))
                    {
                        _craftingStationMap[station.stationName] = new List<CraftingStationData>();
                    }
                    _craftingStationMap[station.stationName].Add(station);
                }
            }
            else
            {
                Debug.LogError($"Prefab not found at path: {item.prefabPath}");
            }
        }
    }

    public void RegisterCraftingStation(CraftingStation station)
    {
        if (!_craftingStations.ContainsKey(station.Id))
        {
            _craftingStations.Add(station.Id, station);
        }
    }

    public void UnregisterCraftingStation(int id)
    {
        if (_craftingStations.ContainsKey(id))
        {
            _craftingStations.Remove(id);
        }
    }

    public void AddCraftingTask(CraftingTask task)
    {
        lock (_craftingQueue)
        {
            _craftingQueue.Enqueue(task);
            if (!_isProcessingTasks)
            {
                _isProcessingTasks = true;
                ProcessCraftingTasks();
            }
        }
    }

    private async void ProcessCraftingTasks()
    {
        while (true)
        {
            CraftingTask task = null;
            lock (_craftingQueue)
            {
                if (_craftingQueue.Count > 0)
                {
                    task = _craftingQueue.Dequeue();
                }
                else
                {
                    _isProcessingTasks = false;
                    return;
                }
            }

            if (task != null)
            {
                // Perform crafting task
                await task.PerformTask();
            }
        }
    }

    public GameObject GetItemPrefab(string itemName)
    {
        if (_itemPrefabs.TryGetValue(itemName, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }

    public void AddItemPrefab(string itemName, GameObject prefab)
    {
        if (!_itemPrefabs.ContainsKey(itemName))
        {
            _itemPrefabs.Add(itemName, prefab);
        }
    }

    public List<CraftingStationData> GetItemsForStation(string stationType)
    {
        if (_craftingStationMap.TryGetValue(stationType, out List<CraftingStationData> items))
        {
            return items;
        }
        return new List<CraftingStationData>();
    }
}
