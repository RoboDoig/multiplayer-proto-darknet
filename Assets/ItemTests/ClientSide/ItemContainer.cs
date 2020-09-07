using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public bool destroyOnEmpty;
    public List<Item> contents;

    public void AddItem(Item item) {
        foreach(Item containerItem in contents) {
            if (item.itemDefinition == containerItem.itemDefinition) {
                containerItem.amount += item.amount;
                return;
            }
        }
        contents.Add(new Item(item.itemDefinition, item.amount));
    }

    public void RemoveItem(Item item) {
        foreach(Item containerItem in contents) {
            if (item.itemDefinition == containerItem.itemDefinition) {
                containerItem.amount -= item.amount;
            }
        }

        CheckEmpty();
    }

    public void TransferItem(Item item, ItemContainer toContainer) {
        toContainer.AddItem(item);
        RemoveItem(item);
    }

    public void TransferAllItems(ItemContainer toContainer) {
        foreach(Item containerItem in contents) {
            TransferItem(containerItem, toContainer);
        }
    }

    public bool HasItem(Item item) {
        foreach(Item containerItem in contents) {
            if (containerItem.itemDefinition == item.itemDefinition) {
                if (containerItem.amount >= item.amount) {
                    return true;
                }
            }
        }

        return false;
    }

    public void CheckEmpty() {
        int totalItems = 0;
        foreach(Item containerItem in contents) {
            totalItems += containerItem.amount;
        }

        if (totalItems <= 0 && destroyOnEmpty) {
            Destroy(this.gameObject);
        }
    }
}
