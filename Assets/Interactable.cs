using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    ItemContainer itemContainer;

    void Awake() {
        itemContainer = GetComponent<ItemContainer>();
    }

    public virtual void OnInteract(Player player) {
        //itemContainer.AddItem(new Item(ItemManager.allItems["item.resource.gold"], 1));
        itemContainer.DeleteItem(new Item(ItemManager.allItems["item.resource.gold"], 1));
    }
}
