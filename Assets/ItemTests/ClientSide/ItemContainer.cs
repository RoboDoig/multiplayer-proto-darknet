using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public int networkID;
    public bool destroyOnEmpty;
    public List<Item> contents;
    public ItemSpawnManager itemSpawnManager;

    // Ask the item spawn manager to add an item to this container
    public void AddItem(Item item) {
        itemSpawnManager.RequestAddItem(item, this);
    }
}
