using System.Threading.Tasks;
using UnityEngine;
// Handles a single crafting task at a crafting station.
public class CraftingTask
{
    public int StationId { get; private set; }
    public float Duration { get; private set; }
    public string ItemToCraft { get; private set; }
    private CraftingStation _craftingStation;

    public CraftingTask(int stationId, float duration, string itemToCraft, CraftingStation craftingStation)
    {
        StationId = stationId;
        Duration = duration;
        ItemToCraft = itemToCraft;
        _craftingStation = craftingStation;
    }

    public async Task PerformTask()
    {
        // Simulate crafting duration
        await Task.Delay((int)(Duration * 1000));
        // Notify the crafting station that the task is complete
        _craftingStation.OnCraftingTaskCompleted(ItemToCraft);
    }
}
