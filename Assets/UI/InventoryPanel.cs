using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    bool isActive = false;
    public GameObject inventorySlot;

    void Start() {
        gameObject.SetActive(isActive);
    }

    public void ToggleView() {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void ClearItems() {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void UpdateItems(ItemContainer itemContainer) {
        ClearItems();

        GameObject obj;
        foreach (Item item in itemContainer.contents) {
            obj = Instantiate(inventorySlot);
            obj.transform.SetParent(transform, false);
            obj.GetComponent<InventorySlot>().parentInventoryPanel = this;
        }
    }
}
