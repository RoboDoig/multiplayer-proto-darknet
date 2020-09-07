using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static Dictionary<string, ItemDefinition> allItems = new Dictionary<string, ItemDefinition>();

    [Header("Resources")]
    public List<ItemDefinitionResource> resources;

    [Header("Tools")]
    public List<ItemDefinitionTool> tools;

    void Awake() {
        // Add all resources
        foreach (ItemDefinition itemDefinition in resources) {
            allItems.Add(itemDefinition.nameID, itemDefinition);
        }

        // Add all tools
        foreach (ItemDefinition itemDefinition in tools) {
            allItems.Add(itemDefinition.nameID, itemDefinition);
        }

        // Etc.
    }
}
