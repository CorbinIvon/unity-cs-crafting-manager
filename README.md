Note: This is a work in progress. I will be updating this as I find time to do so. You are free to use these scripts. All I ask is to be attributed in your project.
# How to use
## Setup
### Project File Hierarchy
Please ensure that you have the following folders in your base project Assets folder:
1. Resources
2. Scripts
3. StreamingAssets
    1. Data
    2. Mods

Your prefabs will need to go under the Resources folder. These can be nested under other folders, as long as you have a direct reference in the items.xml file.

All of your xml configurations will go under the StreamingAssets/Data folder. It is structured this way so that you have your base game items in one file, and any mods in their own files. This way, you can easily add and remove mods without affecting your base game items (unless the mods are meant to do so!).

You are free to move the CraftingManager folder to wherever you see fit.

```
Assets
|-- Resources
|   |-- Prefabs
|       |-- ironOre.prefab
|       |-- woodenSword.prefab
|       |-- ironIngot.prefab
|
|-- CraftingManager
|   |-- CraftedItem.cs
|   |-- CraftingManager.cs
|   |-- CraftingStation.cs
|   |-- CraftingStationData.cs
|   |-- CraftingTask.cs
|   |-- IngredientData.cs
|   |-- ItemData.cs
|   |-- ItemLoader.cs
|
|-- StreamingAssets
|   |-- Data
|   |   |-- items.xml
|   |-- Mods
|       |-- Mod1
|       |   |-- items.xml
|       |-- Mod2
|           |-- items.xml
```

## How to use
Simply attach the `CraftingStation.cs` script to a GameObject in your scene. You can do this by selecting your crafting station, and choosing the `Add Component` option at the bottom and searching for `CraftingStation`.
Set the `Station Type` (i.e. furnace, forge, workbench, etc.) in the inspector. This will allow the item to find where they can be crafted at.

Add prefabs to your `Resources/` folder, and add them to your items.xml file. You can add as many items as you want, and they can have as many ingredients as you want. The only requirement is that the prefab name matches the `name` attribute in the xml file.

Example items.xml file:
```xml
<items>
  <item value="woodenSword"> <!-- The unique name that will be attributed to the object -->
    <prefabPath value="Prefabs/woodenSword" /> <!-- The path to the prefab in the Resources folder -->
    <craftingStations> <!-- The crafting stations that this item can be crafted at -->
      <station value="workbench"> <!-- The type of crafting station. Must match what you set for the station type -->
        <amount value="1" /> <!-- The amount of items that will be crafted -->
        <craftTime value="1" /> <!-- The time it takes to craft the item -->
      </station>
      <station value="forge"> <!-- Second station to show that the one object can be crafted at more than one station -->
        <amount value="5" />
        <craftTime value="5" />
      </station>
    </craftingStations>
  </item>
  <item value="ironOre">
    <prefabPath value="Prefabs/ironOre" />
    <craftingStations>
      <station value="ironOreNode">
        <amount value="1" />
        <spawnOffsetPositionRelative value="1,1,0" /> <!-- The offset from the node that the item will spawn at -->
        <spawnOffsetRotation value="0,0,0" /> <!-- The rotation of the item when it spawns -->
        <spawnOffsetScale value="1,1,1" /> <!-- The scale of the item when it spawns -->
        <craftTime value="0" />
      </station>
    </craftingStations>
  </item>
  <item value="ironIngot">
    <prefabPath value="Prefabs/ironIngot" />
    <craftingStations>
      <station value="forge">
        <craftAmount value="1" />
        <craftTime value="5" />
        <ingredients>
          <ingredient value="ironOre" amount="1" />
          <ingredient value="coal" amount="1" />
        </ingredients>
      </station>
    </craftingStations>
  </item>
</items>
```

